using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SpirvNet.DotNet.CFG;
using SpirvNet.Helper;
using SpirvNet.Spirv;
using SpirvNet.Spirv.Ops.Arithmetic;
using SpirvNet.Spirv.Ops.ConstantCreation;
using SpirvNet.Spirv.Ops.Conversion;
using SpirvNet.Spirv.Ops.FlowControl;
using SpirvNet.Spirv.Ops.Function;
using SpirvNet.Spirv.Ops.RelationalLogical;
using Instruction = SpirvNet.Spirv.Instruction;

namespace SpirvNet.DotNet.SSA
{
    /// <summary>
    /// State of a method frame at a given CFG vertex
    /// </summary>
    public class MethodFrameState
    {
        /// <summary>
        /// Vertex for this CFG State
        /// </summary>
        public readonly Vertex Vertex;

        /// <summary>
        /// Parent frame
        /// </summary>
        public readonly MethodFrame Frame;

        /// <summary>
        /// Incoming state
        /// </summary>
        public readonly List<MethodFrameState> Incoming = new List<MethodFrameState>();
        /// <summary>
        /// Outgoing states
        /// </summary>
        public readonly List<MethodFrameState> Outgoing = new List<MethodFrameState>();

        /// <summary>
        /// Local var locations
        /// </summary>
        public readonly TypedLocation[] LocalVars;
        /// <summary>
        /// Incoming Local var locations
        /// </summary>
        public readonly TypedLocation[] LocalVarsIncoming;
        /// <summary>
        /// Stack locations
        /// </summary>
        public readonly TypedLocation[] StackLocations;
        /// <summary>
        /// Incoming stack
        /// </summary>
        public readonly TypedLocation[] StackLocationsIncoming;

        /// <summary>
        /// Creates a new typed loc
        /// </summary>
        public TypedLocation CreateLocation(SpirvType type) => Frame.CreateLocation(type);
        /// <summary>
        /// Creates a _new_ typed loc
        /// </summary>
        public TypedLocation CreateLocation(TypedLocation loc) => Frame.CreateLocation(loc.Type);
        /// <summary>
        /// Creates a new typed loc
        /// </summary>
        public TypedLocation CreateLocation(Type type) => Frame.CreateLocation(CreateType(type));
        /// <summary>
        /// Create a spirv type for a typed ref
        /// </summary>
        public SpirvType CreateType(TypeReference type) => Frame.TypeBuilder.Create(type);
        /// <summary>
        /// Create a spirv type for a type
        /// </summary>
        public SpirvType CreateType(Type type) => Frame.TypeBuilder.Create(type);

        /// <summary>
        /// Phi function on entry
        /// </summary>
        public readonly List<PhiFunction> PhiFunctions = new List<PhiFunction>();

        public readonly bool IsEntryPoint;
        public readonly bool IsExitPoint;
        public readonly bool IsLinearFlow;
        public readonly bool IsMergingFlow;
        public readonly bool IsDivergingFlow;

        /// <summary>
        /// Op block label
        /// </summary>
        public readonly OpLabel BlockLabel;
        /// <summary>
        /// ID of the block label (throws if non-block start)
        /// </summary>
        public ID BlockID => BlockLabel.Result;

        /// <summary>
        /// Position in stack (points to the first free pos)
        /// </summary>
        public int StackPosition { get; set; }

        /// <summary>
        /// Top of stack
        /// </summary>
        public TypedLocation StackTop => StackLocations[StackPosition - 1];

        /// <summary>
        /// List of instruction generators (can use typedlocation)
        /// </summary>
        public readonly List<Spirv.Instruction> Instructions = new List<Spirv.Instruction>();

        private bool decoded = false; // true iff already decoded

        /// <summary>
        /// Returns the next (default for switch) state
        /// </summary>
        public MethodFrameState NextState => Outgoing.Count > 0 ? Outgoing[0] : null;
        /// <summary>
        /// Returns the target state for branches
        /// </summary>
        public MethodFrameState TargetState => Outgoing.Count > 1 ? Outgoing[1] : null;

        /// <summary>
        /// Assigned block
        /// </summary>
        public MethodBlock Block { get; private set; }


        public MethodFrameState(Vertex vertex, MethodFrame frame)
        {
            Vertex = vertex;
            Frame = frame;

            LocalVars = new TypedLocation[frame.VarCount];
            LocalVarsIncoming = new TypedLocation[frame.VarCount];
            StackLocations = new TypedLocation[frame.StackSize];
            StackLocationsIncoming = new TypedLocation[frame.StackSize];

            var cin = Vertex.Incoming.Count;
            var cout = Vertex.Outgoing.Count;

            if (cin == 0) IsEntryPoint = true;
            if (cout == 0) IsExitPoint = true;
            if (cin > 1) IsMergingFlow = true;
            if (cout > 1) IsDivergingFlow = true;
            if (cin <= 1 && cout <= 1) IsLinearFlow = true;

            // create block label for branch targets
            if (vertex.IsBranchTarget || IsEntryPoint)
                Instructions.Add(BlockLabel = new OpLabel { Result = Frame.Allocator.CreateID() });
        }

        /// <summary>
        /// Build state connectivity
        /// </summary>
        public void BuildConnections()
        {
            foreach (var v in Vertex.Incoming)
                Incoming.Add(Frame.FromVertex(v));
            foreach (var v in Vertex.Outgoing)
                Outgoing.Add(Frame.FromVertex(v));
        }

        /// <summary>
        /// Pushes a typed location of the stack
        /// CAUTION: DOES NOT INTRODUCE NEW LOC
        /// </summary>
        private void Push(TypedLocation location)
        {
            if (location == null)
                throw new InvalidOperationException("Cannot push empty location");

            StackLocations[StackPosition] = location;
            ++StackPosition;
        }

        /// <summary>
        /// Pops a value from the stack (returns the type)
        /// </summary>
        private TypedLocation Pop()
        {
            --StackPosition;
            var loc = StackLocations[StackPosition];
            if (loc == null)
                throw new InvalidOperationException("null-location on stack loc");
            StackLocations[StackPosition] = null;
            return loc;
        }

        /// <summary>
        /// Ensure type of local var
        /// </summary>
        private void EnsureLocalVarType(int loc)
        {
            var currLoc = LocalVars[loc];
            var currType = currLoc.Type;
            var targetType = Frame.LocalVarTypes[loc];

            // requires conversion?
            if (currType != targetType)
            {
                var res = CreateLocation(targetType);

                if (targetType.IsBoolean)
                {
                    var constID = Frame.TypeBuilder.ConstantZero(currType);

                    // convert to bool
                    switch (currType.TypeEnum)
                    {
                        case SpirvTypeEnum.Integer:
                            Instructions.Add(new OpINotEqual
                            {
                                Result = res.ID,
                                ResultType = targetType.TypeID,
                                Operand1 = currLoc.ID,
                                Operand2 = constID
                            });
                            break;

                        case SpirvTypeEnum.Floating:
                            Instructions.Add(new OpFOrdNotEqual
                            {
                                Result = res.ID,
                                ResultType = targetType.TypeID,
                                Operand1 = currLoc.ID,
                                Operand2 = constID
                            });
                            break;

                        default:
                            throw new NotSupportedException("type " + currType);
                    }
                }
                else if (currType.IsBoolean)
                {
                    var zeroID = Frame.TypeBuilder.ConstantZero(currType);
                    var oneID = Frame.TypeBuilder.ConstantUnit(currType);

                    // convert from bool
                    switch (targetType.TypeEnum)
                    {
                        case SpirvTypeEnum.Integer:
                        case SpirvTypeEnum.Floating:
                            Instructions.Add(new OpSelect
                            {
                                Condition = currLoc.ID,
                                ResultType = targetType.TypeID,
                                Object1 = oneID,
                                Object2 = zeroID
                            });
                            break;

                        default:
                            throw new NotSupportedException("type " + currType);
                    }
                }
                else throw new NotImplementedException("Type conversion from " + currType + " to " + targetType);

                LocalVars[loc] = res;
            }
        }

        /// <summary>
        /// Decode operation
        /// </summary>
        public void DecodeOp(MethodFrameState incomingState)
        {
            if (decoded)
                return; // already decoded

            if (IsEntryPoint) // entry points take local vars from frame
            {
                for (var i = 0; i < LocalVars.Length; ++i)
                    LocalVarsIncoming[i] = Frame.LocalVars[i];
            }
            else if (IsMergingFlow) // merging flow gets new locations
            {
                for (var i = 0; i < LocalVars.Length; ++i)
                    LocalVarsIncoming[i] = CreateLocation(Frame.LocalVarTypes[i]);

                for (var i = 0; i < StackLocations.Length; ++i)
                    if (incomingState.StackLocations[i] != null)
                        StackLocationsIncoming[i] = CreateLocation(incomingState.StackLocations[i]);

                StackPosition = incomingState.StackPosition;
            }
            else // non-merging flow copies previous locations
            {
                for (var i = 0; i < LocalVars.Length; ++i)
                    LocalVarsIncoming[i] = incomingState.LocalVars[i];
                for (var i = 0; i < StackLocations.Length; ++i)
                    StackLocationsIncoming[i] = incomingState.StackLocations[i];

                StackPosition = incomingState.StackPosition;
            }

            // block connectivity
            if (Vertex.IsBranchTarget || IsEntryPoint) // new block
            {
                Block = new MethodBlock();
                Frame.Blocks.Add(Block);
            }
            else
            {
                Block = incomingState.Block;
                if (Block == null)
                    throw new InvalidOperationException("Empty previous block");
            }
            Block.States.Add(this);

            // copy incoming stack, vars
            for (var i = 0; i < StackLocations.Length; ++i)
                StackLocations[i] = StackLocationsIncoming[i];
            for (var i = 0; i < LocalVars.Length; ++i)
                LocalVars[i] = LocalVarsIncoming[i];

            var opc = Vertex.OpCode;
            var ins = Vertex.Instruction;

            TypedLocation loc, t1, t2, t, res, tmp;
            ID tl, fl, id;
            VariableDefinition vardef;
            ParameterDefinition argdef;
            SpirvType type;

            switch (opc.Code)
            {
                case Code.Nop:
                    break;

                // load arg into stack
                case Code.Ldarg_0:
                    Push(Frame.ArgLocations[0]);
                    break;
                case Code.Ldarg_1:
                    Push(Frame.ArgLocations[1]);
                    break;
                case Code.Ldarg_2:
                    Push(Frame.ArgLocations[2]);
                    break;
                case Code.Ldarg_3:
                    Push(Frame.ArgLocations[3]);
                    break;
                case Code.Ldarg_S:
                    argdef = (ParameterDefinition)ins.Operand;
                    Push(Frame.ArgLocations[argdef.Index]);
                    break;

                // Load from local var
                case Code.Ldloc_0:
                    Push(LocalVars[0]);
                    break;
                case Code.Ldloc_1:
                    Push(LocalVars[1]);
                    break;
                case Code.Ldloc_2:
                    Push(LocalVars[2]);
                    break;
                case Code.Ldloc_3:
                    Push(LocalVars[3]);
                    break;
                case Code.Ldloc_S:
                    vardef = (VariableDefinition)ins.Operand;
                    Push(LocalVars[vardef.Index]);
                    break;

                // Store in local var
                case Code.Stloc_0:
                    LocalVars[0] = Pop();
                    EnsureLocalVarType(0);
                    break;
                case Code.Stloc_1:
                    LocalVars[1] = Pop();
                    EnsureLocalVarType(1);
                    break;
                case Code.Stloc_2:
                    LocalVars[2] = Pop();
                    EnsureLocalVarType(2);
                    break;
                case Code.Stloc_3:
                    LocalVars[3] = Pop();
                    EnsureLocalVarType(3);
                    break;
                case Code.Stloc_S:
                    vardef = (VariableDefinition)ins.Operand;
                    LocalVars[vardef.Index] = Pop();
                    EnsureLocalVarType(vardef.Index);
                    break;

                // arithmetics
                case Code.Add:
                    t2 = Pop();
                    t1 = Pop();
                    if (t1.Type != t2.Type)
                        throw new NotSupportedException("incompatible types " + t1 + ", " + t2);
                    res = CreateLocation(t1.Type);
                    Push(res);
                    if (t1.Type.IsInteger)
                        Instructions.Add(new OpIAdd { Operand1 = t1.ID, Operand2 = t2.ID, Result = res.ID, ResultType = res.Type.TypeID });
                    else if (t1.Type.IsFloating)
                        Instructions.Add(new OpFAdd { Operand1 = t1.ID, Operand2 = t2.ID, Result = res.ID, ResultType = res.Type.TypeID });
                    else throw new NotSupportedException("Unsupported add: " + t1 + ", " + t2);
                    break;
                case Code.Sub:
                    t2 = Pop();
                    t1 = Pop();
                    if (t1.Type != t2.Type)
                        throw new NotSupportedException("incompatible types " + t1 + ", " + t2);
                    res = CreateLocation(t1.Type);
                    Push(res);
                    if (t1.Type.IsInteger)
                        Instructions.Add(new OpISub { Operand1 = t1.ID, Operand2 = t2.ID, Result = res.ID, ResultType = res.Type.TypeID });
                    else if (t1.Type.IsFloating)
                        Instructions.Add(new OpFSub { Operand1 = t1.ID, Operand2 = t2.ID, Result = res.ID, ResultType = res.Type.TypeID });
                    else throw new NotSupportedException("Unsupported sub: " + t1 + ", " + t2);
                    break;
                case Code.Mul:
                    t2 = Pop();
                    t1 = Pop();
                    if (t1.Type != t2.Type)
                        throw new NotSupportedException("incompatible types " + t1 + ", " + t2);
                    res = CreateLocation(t1.Type);
                    Push(res);
                    if (t1.Type.IsInteger)
                        Instructions.Add(new OpIMul { Operand1 = t1.ID, Operand2 = t2.ID, Result = res.ID, ResultType = res.Type.TypeID });
                    else if (t1.Type.IsFloating)
                        Instructions.Add(new OpFMul { Operand1 = t1.ID, Operand2 = t2.ID, Result = res.ID, ResultType = res.Type.TypeID });
                    else throw new NotSupportedException("Unsupported mul: " + t1 + ", " + t2);
                    break;
                case Code.Div:
                    t2 = Pop();
                    t1 = Pop();
                    if (t1.Type != t2.Type)
                        throw new NotSupportedException("incompatible types " + t1 + ", " + t2);
                    res = CreateLocation(t1.Type);
                    Push(res);
                    if (t1.Type.IsInteger)
                        Instructions.Add(new OpSDiv { Operand1 = t1.ID, Operand2 = t2.ID, Result = res.ID, ResultType = res.Type.TypeID });
                    else if (t1.Type.IsFloating)
                        Instructions.Add(new OpFDiv { Operand1 = t1.ID, Operand2 = t2.ID, Result = res.ID, ResultType = res.Type.TypeID });
                    else throw new NotSupportedException("Unsupported div: " + t1 + ", " + t2);
                    break;
                case Code.Div_Un:
                    t2 = Pop();
                    t1 = Pop();
                    if (t1.Type != t2.Type)
                        throw new NotSupportedException("incompatible types " + t1 + ", " + t2);
                    res = CreateLocation(t1.Type);
                    Push(res);
                    if (t1.Type.IsInteger)
                        Instructions.Add(new OpUDiv { Operand1 = t1.ID, Operand2 = t2.ID, Result = res.ID, ResultType = res.Type.TypeID });
                    else throw new NotSupportedException("Unsupported unsigned div: " + t1 + ", " + t2);
                    break;

                // not/negate
                case Code.Neg:
                    t = Pop();
                    res = CreateLocation(t.Type);
                    Push(res);
                    if (t.Type.IsInteger)
                        Instructions.Add(new OpSNegate { Operand = t.ID, Result = res.ID, ResultType = res.Type.TypeID });
                    else if (t.Type.IsFloating)
                        Instructions.Add(new OpFNegate { Operand = t.ID, Result = res.ID, ResultType = res.Type.TypeID });
                    else throw new NotSupportedException("Unsupported negate: " + t);
                    break;
                case Code.Not:
                    t = Pop();
                    res = CreateLocation(t.Type);
                    Push(res);
                    if (t.Type.IsInteger)
                        Instructions.Add(new OpNot { Operand = t.ID, Result = res.ID, ResultType = res.Type.TypeID });
                    else throw new NotSupportedException("Unsupported not: " + t);
                    break;

                // remainders
                case Code.Rem:
                    t2 = Pop();
                    t1 = Pop();
                    if (t1.Type != t2.Type)
                        throw new NotSupportedException("incompatible types " + t1 + ", " + t2);
                    res = CreateLocation(t1.Type);
                    Push(res);
                    if (t1.Type.IsInteger && t1.Type.IsSigned)
                        Instructions.Add(new OpSRem { Operand1 = t1.ID, Operand2 = t2.ID, Result = res.ID, ResultType = res.Type.TypeID });
                    else if (t1.Type.IsFloating)
                        Instructions.Add(new OpFRem { Operand1 = t1.ID, Operand2 = t2.ID, Result = res.ID, ResultType = res.Type.TypeID });
                    else throw new NotSupportedException("Unsupported remainder: " + t1 + ", " + t2);
                    break;
                case Code.Rem_Un:
                    t2 = Pop();
                    t1 = Pop();
                    if (t1.Type != t2.Type)
                        throw new NotSupportedException("incompatible types " + t1 + ", " + t2);
                    res = CreateLocation(t1.Type);
                    Push(res);
                    if (t1.Type.IsInteger && !t1.Type.IsSigned)
                        Instructions.Add(new OpUMod { Operand1 = t1.ID, Operand2 = t2.ID, Result = res.ID, ResultType = res.Type.TypeID });
                    else throw new NotSupportedException("Unsupported remainder: " + t1 + ", " + t2);
                    break;

                case Code.And:
                case Code.Or:
                case Code.Xor:
                case Code.Shl:
                case Code.Shr:
                case Code.Shr_Un:
                    throw new NotImplementedException("unsupported arithmetic op");

                // constant loading
                case Code.Ldc_I4:
                    Push(new TypedLocation(Frame.TypeBuilder.ConstantInt32((int)ins.Operand), typeof(int), Frame.TypeBuilder));
                    break;
                case Code.Ldc_I8:
                    Push(new TypedLocation(Frame.TypeBuilder.ConstantInt64((long)ins.Operand), typeof(long), Frame.TypeBuilder));
                    break;
                case Code.Ldc_R4:
                    Push(new TypedLocation(Frame.TypeBuilder.ConstantFloat32((float)ins.Operand), typeof(float), Frame.TypeBuilder));
                    break;
                case Code.Ldc_R8:
                    Push(new TypedLocation(Frame.TypeBuilder.ConstantFloat64((double)ins.Operand), typeof(double), Frame.TypeBuilder));
                    break;

                // return value
                case Code.Ret:
                    switch (StackPosition)
                    {
                        case 0:
                            Instructions.Add(new OpReturn());
                            break;
                        case 1:
                            loc = Pop();
                            Instructions.Add(new OpReturnValue { Value = loc.ID });
                            break;
                        default:
                            throw new InvalidOperationException("Non-empty stack at return");
                    }
                    break;

                // unconditional branching
                case Code.Br:
                case Code.Br_S:
                    if (Outgoing.Count != 1)
                        throw new InvalidOperationException("Unconditional branch with multiple outgoing");
                    if (Outgoing[0].BlockLabel == null)
                        throw new InvalidOperationException("Target has no block label");
                    Instructions.Add(new OpBranch { TargetLabel = Outgoing[0].BlockLabel.Result });
                    break;

                // conditional boolean branching
                case Code.Brfalse:
                case Code.Brfalse_S:
                case Code.Brtrue:
                case Code.Brtrue_S:
                    if (Outgoing.Count != 2)
                        throw new InvalidOperationException("Conditional branch with other than two outgoing");

                    if (opc.Code == Code.Brfalse || opc.Code == Code.Brfalse_S)
                    {
                        fl = TargetState.BlockID;
                        tl = NextState.BlockID;
                    }
                    else
                    {
                        fl = NextState.BlockID;
                        tl = TargetState.BlockID;
                    }

                    loc = Pop();
                    if (loc.Type.IsBoolean)
                        Instructions.Add(new OpBranchConditional { Condition = loc.ID, FalseLabel = fl, TrueLabel = tl });
                    else
                        throw new NotSupportedException("Condition of type " + loc.Type + " not supported");
                    break;

                // conditional branches
                case Code.Beq_S:
                case Code.Bge_S:
                case Code.Bgt_S:
                case Code.Ble_S:
                case Code.Blt_S:
                case Code.Bne_Un_S:
                case Code.Bge_Un_S:
                case Code.Bgt_Un_S:
                case Code.Ble_Un_S:
                case Code.Blt_Un_S:
                case Code.Beq:
                case Code.Bge:
                case Code.Bgt:
                case Code.Ble:
                case Code.Blt:
                case Code.Bne_Un:
                case Code.Bge_Un:
                case Code.Bgt_Un:
                case Code.Ble_Un:
                case Code.Blt_Un:
                    t2 = Pop();
                    t1 = Pop();
                    if (t1.Type != t2.Type)
                        throw new NotSupportedException("incompatible types " + t1 + ", " + t2);
                    tmp = CreateLocation(typeof(bool));

                    // comparison
                    if (t1.Type.IsInteger)
                        switch (opc.Code)
                        {
                            case Code.Beq:
                            case Code.Beq_S:
                                Instructions.Add(new OpIEqual { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                break;
                            case Code.Bne_Un:
                            case Code.Bne_Un_S:
                                Instructions.Add(new OpINotEqual { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                break;
                            case Code.Bgt:
                            case Code.Bgt_Un:
                            case Code.Bgt_S:
                            case Code.Bgt_Un_S:
                                if (t1.Type.IsSigned)
                                    Instructions.Add(new OpSGreaterThan { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                else Instructions.Add(new OpUGreaterThan { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                break;
                            case Code.Bge:
                            case Code.Bge_Un:
                            case Code.Bge_S:
                            case Code.Bge_Un_S:
                                if (t1.Type.IsSigned)
                                    Instructions.Add(new OpSGreaterThanEqual { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                else Instructions.Add(new OpUGreaterThanEqual { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                break;
                            case Code.Blt:
                            case Code.Blt_Un:
                            case Code.Blt_S:
                            case Code.Blt_Un_S:
                                if (t1.Type.IsSigned)
                                    Instructions.Add(new OpSLessThan { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                else Instructions.Add(new OpULessThan { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                break;
                            case Code.Ble:
                            case Code.Ble_Un:
                            case Code.Ble_S:
                            case Code.Ble_Un_S:
                                if (t1.Type.IsSigned)
                                    Instructions.Add(new OpSLessThan { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                else Instructions.Add(new OpULessThan { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    else if (t1.Type.IsFloating)
                        switch (opc.Code)
                        {
                            case Code.Beq:
                            case Code.Beq_S:
                                Instructions.Add(new OpFOrdEqual { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                break;
                            case Code.Bne_Un:
                            case Code.Bne_Un_S:
                                Instructions.Add(new OpFUnordNotEqual { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                break;
                            case Code.Bgt:
                            case Code.Bgt_S:
                                Instructions.Add(new OpFOrdGreaterThan { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                break;
                            case Code.Bgt_Un:
                            case Code.Bgt_Un_S:
                                Instructions.Add(new OpFUnordGreaterThan { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                break;
                            case Code.Bge:
                            case Code.Bge_S:
                                Instructions.Add(new OpFOrdGreaterThanEqual { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                break;
                            case Code.Bge_Un:
                            case Code.Bge_Un_S:
                                Instructions.Add(new OpFUnordGreaterThanEqual { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                break;
                            case Code.Blt:
                            case Code.Blt_S:
                                Instructions.Add(new OpFOrdLessThan { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                break;
                            case Code.Blt_Un:
                            case Code.Blt_Un_S:
                                Instructions.Add(new OpFUnordLessThan { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                break;
                            case Code.Ble:
                            case Code.Ble_S:
                                Instructions.Add(new OpFOrdLessThanEqual { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                break;
                            case Code.Ble_Un:
                            case Code.Ble_Un_S:
                                Instructions.Add(new OpFUnordLessThanEqual { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    else throw new NotSupportedException("Unsupported comparison: " + t1 + ", " + t2);

                    // branch
                    Instructions.Add(new OpBranchConditional { Condition = tmp.ID, FalseLabel = NextState.BlockID, TrueLabel = TargetState.BlockID });
                    break;

                // comparisons
                case Code.Ceq:
                case Code.Cgt:
                case Code.Cgt_Un:
                case Code.Clt:
                case Code.Clt_Un:
                    t2 = Pop();
                    t1 = Pop();
                    if (t1.Type != t2.Type)
                        throw new NotSupportedException("incompatible types " + t1 + ", " + t2);
                    tmp = CreateLocation(typeof(bool));
                    res = CreateLocation(typeof(int));

                    // comparison
                    if (t1.Type.IsInteger)
                        switch (opc.Code)
                        {
                            case Code.Ceq:
                                Instructions.Add(new OpIEqual { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                break;
                            case Code.Cgt:
                            case Code.Cgt_Un:
                                if (t1.Type.IsSigned)
                                    Instructions.Add(new OpSGreaterThan { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                else Instructions.Add(new OpUGreaterThan { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                break;
                            case Code.Clt:
                            case Code.Clt_Un:
                                if (t1.Type.IsSigned)
                                    Instructions.Add(new OpSLessThan { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                else Instructions.Add(new OpULessThan { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    else if (t1.Type.IsFloating)
                        switch (opc.Code)
                        {
                            case Code.Ceq:
                                Instructions.Add(new OpFOrdEqual { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                break;
                            case Code.Cgt:
                                Instructions.Add(new OpFOrdGreaterThan { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                break;
                            case Code.Cgt_Un:
                                Instructions.Add(new OpFUnordGreaterThan { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                break;
                            case Code.Clt:
                                Instructions.Add(new OpFOrdLessThan { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                break;
                            case Code.Clt_Un:
                                Instructions.Add(new OpFUnordLessThan { Result = tmp.ID, ResultType = tmp.Type.TypeID, Operand1 = t1.ID, Operand2 = t2.ID });
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    else throw new NotSupportedException("Unsupported comparison: " + t1 + ", " + t2);

                    // conversion
                    Instructions.Add(new OpSelect
                    {
                        Condition = tmp.ID,
                        Result = res.ID,
                        ResultType = res.Type.TypeID,
                        Object1 = Frame.TypeBuilder.ConstantInt32(1),
                        Object2 = Frame.TypeBuilder.ConstantInt32(0),
                    });
                    Push(res);
                    break;

                // const int loads
                case Code.Ldc_I4_M1:
                    Push(new TypedLocation(Frame.TypeBuilder.ConstantInt32(-1), typeof(int), Frame.TypeBuilder));
                    break;
                case Code.Ldc_I4_0:
                    Push(new TypedLocation(Frame.TypeBuilder.ConstantInt32(0), typeof(int), Frame.TypeBuilder));
                    break;
                case Code.Ldc_I4_1:
                    Push(new TypedLocation(Frame.TypeBuilder.ConstantInt32(1), typeof(int), Frame.TypeBuilder));
                    break;
                case Code.Ldc_I4_2:
                    Push(new TypedLocation(Frame.TypeBuilder.ConstantInt32(2), typeof(int), Frame.TypeBuilder));
                    break;
                case Code.Ldc_I4_3:
                    Push(new TypedLocation(Frame.TypeBuilder.ConstantInt32(3), typeof(int), Frame.TypeBuilder));
                    break;
                case Code.Ldc_I4_4:
                    Push(new TypedLocation(Frame.TypeBuilder.ConstantInt32(4), typeof(int), Frame.TypeBuilder));
                    break;
                case Code.Ldc_I4_5:
                    Push(new TypedLocation(Frame.TypeBuilder.ConstantInt32(5), typeof(int), Frame.TypeBuilder));
                    break;
                case Code.Ldc_I4_6:
                    Push(new TypedLocation(Frame.TypeBuilder.ConstantInt32(6), typeof(int), Frame.TypeBuilder));
                    break;
                case Code.Ldc_I4_7:
                    Push(new TypedLocation(Frame.TypeBuilder.ConstantInt32(7), typeof(int), Frame.TypeBuilder));
                    break;
                case Code.Ldc_I4_8:
                    Push(new TypedLocation(Frame.TypeBuilder.ConstantInt32(8), typeof(int), Frame.TypeBuilder));
                    break;
                case Code.Ldc_I4_S:
                    Push(new TypedLocation(Frame.TypeBuilder.ConstantInt32((int)ins.Operand), typeof(int), Frame.TypeBuilder));
                    break;

                // unsupported integer conversion
                case Code.Conv_I1:
                case Code.Conv_I2:
                    throw new NotSupportedException("char and short are not supported");

                // to integer conversion
                case Code.Conv_I4:
                case Code.Conv_I8:
                    tmp = Pop();
                    res = CreateLocation(opc.Code == Code.Conv_R4 ? typeof(int) : typeof(long));
                    switch (tmp.Type.TypeEnum)
                    {
                        case SpirvTypeEnum.Integer:
                            if (tmp.Type.IsSigned)
                                Instructions.Add(new OpSConvert { ResultType = res.Type.TypeID, Result = res.ID, SignedValue = tmp.ID });
                            else throw new InvalidOperationException("Signed-unsigned conversion?");
                            break;
                        case SpirvTypeEnum.Floating:
                            Instructions.Add(new OpConvertFToS { ResultType = res.Type.TypeID, Result = res.ID, FloatValue = tmp.ID });
                            break;
                        default:
                            throw new NotSupportedException("or not implemented");
                    }
                    Push(res);
                    break;
                // to floating conversion
                case Code.Conv_R4:
                case Code.Conv_R8:
                    tmp = Pop();
                    res = CreateLocation(opc.Code == Code.Conv_R4 ? typeof(float) : typeof(double));
                    switch (tmp.Type.TypeEnum)
                    {
                        case SpirvTypeEnum.Integer:
                            if (tmp.Type.IsSigned)
                                Instructions.Add(new OpConvertSToF { ResultType = res.Type.TypeID, Result = res.ID, SignedValue = tmp.ID });
                            else Instructions.Add(new OpConvertUToF { ResultType = res.Type.TypeID, Result = res.ID, UnsignedValue = tmp.ID });
                            break;
                        case SpirvTypeEnum.Floating:
                            Instructions.Add(new OpFConvert { ResultType = res.Type.TypeID, Result = res.ID, FloatValue = tmp.ID });
                            break;
                        default:
                            throw new NotSupportedException("or not implemented");
                    }
                    Push(res);
                    break;
                // to unsigned conversion
                case Code.Conv_U4:
                case Code.Conv_U8:
                    tmp = Pop();
                    res = CreateLocation(opc.Code == Code.Conv_R4 ? typeof(uint) : typeof(ulong));
                    switch (tmp.Type.TypeEnum)
                    {
                        case SpirvTypeEnum.Integer:
                            if (tmp.Type.IsSigned)
                                throw new InvalidOperationException("Signed-unsigned conversion?");
                            else Instructions.Add(new OpUConvert { ResultType = res.Type.TypeID, Result = res.ID, UnsignedValue = tmp.ID });
                            break;
                        case SpirvTypeEnum.Floating:
                            Instructions.Add(new OpConvertFToU { ResultType = res.Type.TypeID, Result = res.ID, FloatValue = tmp.ID });
                            break;
                        default:
                            throw new NotSupportedException("or not implemented");
                    }
                    Push(res);
                    break;

                // calls
                case Code.Call:
                    if (Frame.FunctionProvider == null)
                        throw new InvalidOperationException("Cannot call functions if no function provider registered.");
                    {
                        var kvp = Frame.FunctionProvider.Resolve((MethodDefinition)ins.Operand);
                        id = kvp.Key;
                        type = kvp.Value;
                        if (!type.IsFunction)
                            throw new InvalidOperationException("non-function function type");

                        var args = new List<TypedLocation>();
                        foreach (var p in type.ParameterTypes)
                            args.Add(Pop());
                        args.Reverse();
                        if (StackPosition > 0 && StackTop.Type.IsThis)
                            Pop(); // this workaround
                        res = CreateLocation(type.ReturnType);
                        Push(res);
                        Instructions.Add(new OpFunctionCall
                        {
                            Result = res.ID,
                            ResultType = res.Type.TypeID,
                            Function = id,
                            Arguments = args.Select(a => a.ID).ToArray()
                        });
                    }
                    break;

                // indirect calls
                case Code.Calli:
                    throw new NotSupportedException("Indirect calls are not supported");
                case Code.Callvirt:
                    throw new NotSupportedException("Virtual calls are not supported");

                // duplicate
                case Code.Dup:
                    Push(StackTop);
                    break;

                case Code.Starg_S:
                case Code.Ldarga_S:
                case Code.Ldloca_S:
                case Code.Ldnull:
                case Code.Pop:
                case Code.Jmp:
                case Code.Switch:
                case Code.Ldind_I1:
                case Code.Ldind_U1:
                case Code.Ldind_I2:
                case Code.Ldind_U2:
                case Code.Ldind_I4:
                case Code.Ldind_U4:
                case Code.Ldind_I8:
                case Code.Ldind_I:
                case Code.Ldind_R4:
                case Code.Ldind_R8:
                case Code.Ldind_Ref:
                case Code.Stind_Ref:
                case Code.Stind_I1:
                case Code.Stind_I2:
                case Code.Stind_I4:
                case Code.Stind_I8:
                case Code.Stind_R4:
                case Code.Stind_R8:
                case Code.Cpobj:
                case Code.Ldobj:
                case Code.Ldstr:
                case Code.Newobj:
                case Code.Castclass:
                case Code.Isinst:
                case Code.Conv_R_Un:
                case Code.Unbox:
                case Code.Throw:
                case Code.Ldfld:
                case Code.Ldflda:
                case Code.Stfld:
                case Code.Ldsfld:
                case Code.Ldsflda:
                case Code.Stsfld:
                case Code.Stobj:
                case Code.Conv_Ovf_I1_Un:
                case Code.Conv_Ovf_I2_Un:
                case Code.Conv_Ovf_I4_Un:
                case Code.Conv_Ovf_I8_Un:
                case Code.Conv_Ovf_U1_Un:
                case Code.Conv_Ovf_U2_Un:
                case Code.Conv_Ovf_U4_Un:
                case Code.Conv_Ovf_U8_Un:
                case Code.Conv_Ovf_I_Un:
                case Code.Conv_Ovf_U_Un:
                case Code.Box:
                case Code.Newarr:
                case Code.Ldlen:
                case Code.Ldelema:
                case Code.Ldelem_I1:
                case Code.Ldelem_U1:
                case Code.Ldelem_I2:
                case Code.Ldelem_U2:
                case Code.Ldelem_I4:
                case Code.Ldelem_U4:
                case Code.Ldelem_I8:
                case Code.Ldelem_I:
                case Code.Ldelem_R4:
                case Code.Ldelem_R8:
                case Code.Ldelem_Ref:
                case Code.Stelem_I:
                case Code.Stelem_I1:
                case Code.Stelem_I2:
                case Code.Stelem_I4:
                case Code.Stelem_I8:
                case Code.Stelem_R4:
                case Code.Stelem_R8:
                case Code.Stelem_Ref:
                case Code.Ldelem_Any:
                case Code.Stelem_Any:
                case Code.Unbox_Any:
                case Code.Conv_Ovf_I1:
                case Code.Conv_Ovf_U1:
                case Code.Conv_Ovf_I2:
                case Code.Conv_Ovf_U2:
                case Code.Conv_Ovf_I4:
                case Code.Conv_Ovf_U4:
                case Code.Conv_Ovf_I8:
                case Code.Conv_Ovf_U8:
                case Code.Refanyval:
                case Code.Ckfinite:
                case Code.Mkrefany:
                case Code.Ldtoken:
                case Code.Conv_U2:
                case Code.Conv_U1:
                case Code.Conv_I:
                case Code.Conv_Ovf_I:
                case Code.Conv_Ovf_U:
                case Code.Add_Ovf:
                case Code.Add_Ovf_Un:
                case Code.Mul_Ovf:
                case Code.Mul_Ovf_Un:
                case Code.Sub_Ovf:
                case Code.Sub_Ovf_Un:
                case Code.Endfinally:
                case Code.Leave:
                case Code.Leave_S:
                case Code.Stind_I:
                case Code.Conv_U:
                case Code.Arglist:
                case Code.Ldftn:
                case Code.Ldvirtftn:
                case Code.Ldarg:
                case Code.Ldarga:
                case Code.Starg:
                case Code.Ldloc:
                case Code.Ldloca:
                case Code.Stloc:
                case Code.Localloc:
                case Code.Endfilter:
                case Code.Unaligned:
                case Code.Volatile:
                case Code.Tail:
                case Code.Initobj:
                case Code.Constrained:
                case Code.Cpblk:
                case Code.Initblk:
                case Code.No:
                case Code.Rethrow:
                case Code.Sizeof:
                case Code.Refanytype:
                case Code.Readonly:
                case Code.Break:
                    throw new NotSupportedException("Unsupported instruction: " + opc.Code);
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // propagate
            decoded = true;
            foreach (var state in Outgoing)
                state.DecodeOp(this);
        }

        /// <summary>
        /// Create phi functions
        /// </summary>
        public void CreatePhis()
        {
            if (!decoded)
                throw new InvalidOperationException("Unreachable state");

            for (var i = 0; i < StackLocationsIncoming.Length; ++i)
                if (StackLocationsIncoming[i] != null &&
                    Incoming.Any(s => s.StackLocations[i].ID.Value != StackLocationsIncoming[i].ID.Value))
                    Instructions.Add(new OpPhi
                    {
                        Result = StackLocationsIncoming[i].ID,
                        ResultType = StackLocationsIncoming[i].Type.TypeID,
                        IDs = Incoming.Select(s => s.StackLocations[i].ID).ToArray()
                    });

            for (var i = 0; i < Frame.VarCount; ++i)
                if (LocalVarsIncoming[i] != null &&
                    Incoming.Any(s => s.LocalVars[i].ID.Value != LocalVarsIncoming[i].ID.Value))
                    Instructions.Add(new OpPhi
                    {
                        Result = LocalVarsIncoming[i].ID,
                        ResultType = LocalVarsIncoming[i].Type.TypeID,
                        IDs = Incoming.Select(s => s.LocalVars[i].ID).ToArray()
                    });
        }

        /// <summary>
        /// Create all ops
        /// </summary>
        public IEnumerable<Instruction> CreateOps()
        {
            return Instructions;
        }

        public IEnumerable<string> DotLines
        {
            get
            {
                var name = "{" + Vertex.ToString();
                if (Frame.VarCount > 0)
                {
                    name += "|{{";
                    name += Frame.VarCount.ForUpTo(i => "Var_" + i).Aggregate((s1, s2) => s1 + "|" + s2);
                    name += "}|{";
                    name += Frame.VarCount.ForUpTo(i => LocalVars[i]?.ID.ToString()).Aggregate((s1, s2) => s1 + "|" + s2);
                    name += "}|{";
                    name += Frame.VarCount.ForUpTo(i => LocalVars[i]?.Type.ToString()).Aggregate((s1, s2) => s1 + "|" + s2);
                    name += "}}";
                }
                if (StackPosition > 0)
                {
                    name += "|{{";
                    name += StackPosition.ForUpTo(i => "Stack_" + i).Aggregate((s1, s2) => s1 + "|" + s2);
                    name += "}|{";
                    name += StackPosition.ForUpTo(i => StackLocations[i].ID.ToString()).Aggregate((s1, s2) => s1 + "|" + s2);
                    name += "}|{";
                    name += StackPosition.ForUpTo(i => StackLocations[i].Type.ToString()).Aggregate((s1, s2) => s1 + "|" + s2);
                    name += "}}";
                }
                foreach (var instruction in Instructions)
                    name += "|" + instruction;
                name += "}";

                var attr = new List<string>
                {
                    string.Format("label=\"{0}\"", name),
                    "shape=record"
                };


                yield return string.Format("v{0} [{1}];", Vertex.Index, attr.Aggregate((s1, s2) => s1 + "," + s2));
            }
        }
    }
}
