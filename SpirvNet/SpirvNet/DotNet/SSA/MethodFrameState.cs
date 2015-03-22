using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SpirvNet.DotNet.CFG;
using SpirvNet.Spirv;
using SpirvNet.Spirv.Ops.Arithmetic;
using SpirvNet.Spirv.Ops.ConstantCreation;
using SpirvNet.Spirv.Ops.FlowControl;
using Instruction = SpirvNet.Spirv.Instruction;

namespace SpirvNet.DotNet.SSA
{
    /// <summary>
    /// State of a method frame at a given CFG vertex
    /// </summary>
    class MethodFrameState
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
        /// Stack locations
        /// </summary>
        public readonly TypedLocation[] StackLocations;

        /// <summary>
        /// Creates a new typed loc
        /// </summary>
        public TypedLocation CreateLocation(SpirvType type) => Frame.CreateLocation(type);
        /// <summary>
        /// Creates a _new_ typed loc
        /// </summary>
        public TypedLocation CreateLocation(TypedLocation loc) => Frame.CreateLocation(loc.Type);
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

        public MethodFrameState(Vertex vertex, MethodFrame frame)
        {
            Vertex = vertex;
            Frame = frame;

            // create block label for branch targets
            if (vertex.IsBranchTarget)
                Instructions.Add(BlockLabel = new OpLabel { Result = Frame.Allocator.CreateID() });

            LocalVars = new TypedLocation[frame.VarCount];
            StackLocations = new TypedLocation[frame.StackSize];

            var cin = Vertex.Incoming.Count;
            var cout = Vertex.Outgoing.Count;

            if (cin == 0) IsEntryPoint = true;
            if (cout == 0) IsExitPoint = true;
            if (cin > 1) IsMergingFlow = true;
            if (cout > 1) IsDivergingFlow = true;
            if (cin <= 1 && cout <= 1) IsLinearFlow = true;
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
                throw new InvalidOperationException();
            
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
        /// Decode operation
        /// </summary>
        public void DecodeOp(MethodFrameState incomingState)
        {
            if (decoded)
                return; // already decoded

            if (IsEntryPoint) // entry points take local vars from frame
            {
                for (var i = 0; i < LocalVars.Length; ++i)
                    LocalVars[i] = Frame.LocalVars[i];
            }
            else if (IsMergingFlow) // merging flow gets new locations
            {
                for (var i = 0; i < LocalVars.Length; ++i)
                    LocalVars[i] = CreateLocation(Frame.LocalVarTypes[i]);

                for (var i = 0; i < StackLocations.Length; ++i)
                    if (incomingState.StackLocations[i] != null)
                        StackLocations[i] = CreateLocation(incomingState.StackLocations[i]);

                StackPosition = incomingState.StackPosition;
            }
            else // non-merging flow copies previous locations
            {
                for (var i = 0; i < LocalVars.Length; ++i)
                    LocalVars[i] = incomingState.LocalVars[i];
                for (var i = 0; i < StackLocations.Length; ++i)
                    StackLocations[i] = incomingState.StackLocations[i];

                StackPosition = incomingState.StackPosition;
            }

            var opc = Vertex.OpCode;
            var ins = Vertex.Instruction;

            TypedLocation loc, t1, t2;

            switch (opc.Code)
            {
                case Code.Nop:
                    break;

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

                case Code.Stloc_0:
                    LocalVars[0] = Pop();
                    break;
                case Code.Stloc_1:
                    LocalVars[1] = Pop();
                    break;
                case Code.Stloc_2:
                    LocalVars[2] = Pop();
                    break;
                case Code.Stloc_3:
                    LocalVars[3] = Pop();
                    break;

                case Code.Add:
                    t1 = Pop();
                    t2 = Pop();
                    if (t1.Type != t2.Type)
                        throw new NotSupportedException("incompatible types " + t1 + ", " + t2);
                    var res = CreateLocation(t1.Type);
                    Push(res);
                    if (t1.Type.IsInteger)
                        Instructions.Add(new OpIAdd { Operand1 = t1.ID, Operand2 = t2.ID, Result = res.ID, ResultType = res.Type.TypeID });
                    else if (t1.Type.IsFloating)
                        Instructions.Add(new OpFAdd { Operand1 = t1.ID, Operand2 = t2.ID, Result = res.ID, ResultType = res.Type.TypeID });
                    else throw new NotSupportedException("Unsupported add: " + t1 + ", " + t2);
                    break;

                case Code.Ldc_I4:
                    loc = CreateLocation(CreateType(typeof(int)));
                    Instructions.Add(new OpConstant { Result = loc.ID, ResultType = loc.Type.TypeID, Value = LiteralNumber.ArrayFor((int)ins.Operand) });
                    Push(loc);
                    break;
                case Code.Ldc_I8:
                    loc = CreateLocation(CreateType(typeof(long)));
                    Instructions.Add(new OpConstant { Result = loc.ID, ResultType = loc.Type.TypeID, Value = LiteralNumber.ArrayFor((long)ins.Operand) });
                    Push(loc);
                    break;
                case Code.Ldc_R4:
                    loc = CreateLocation(CreateType(typeof(float)));
                    Instructions.Add(new OpConstant { Result = loc.ID, ResultType = loc.Type.TypeID, Value = LiteralNumber.ArrayFor((float)ins.Operand) });
                    Push(loc);
                    break;
                case Code.Ldc_R8:
                    loc = CreateLocation(CreateType(typeof(double)));
                    Instructions.Add(new OpConstant { Result = loc.ID, ResultType = loc.Type.TypeID, Value = LiteralNumber.ArrayFor((double)ins.Operand) });
                    Push(loc);
                    break;

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

                case Code.Br:
                case Code.Br_S:
                    if (Outgoing.Count != 1)
                        throw new InvalidOperationException("Unconditional branch with multiple outgoing");
                    if (Outgoing[0].BlockLabel == null)
                        throw new InvalidOperationException("Target has no block label");
                    Instructions.Add(new OpBranch { TargetLabel = Outgoing[0].BlockLabel.Result });
                    break;

                case Code.Brfalse_S:
                case Code.Brtrue_S:
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
                case Code.Brfalse:
                case Code.Brtrue:
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
                    throw new NotImplementedException("Unimplemented branching op: " + opc.Code);

                case Code.Ldarg_S:
                case Code.Ldarga_S:
                case Code.Starg_S:
                case Code.Ldloc_S:
                case Code.Ldloca_S:
                case Code.Stloc_S:
                case Code.Ldnull:
                case Code.Ldc_I4_M1:
                case Code.Ldc_I4_0:
                case Code.Ldc_I4_1:
                case Code.Ldc_I4_2:
                case Code.Ldc_I4_3:
                case Code.Ldc_I4_4:
                case Code.Ldc_I4_5:
                case Code.Ldc_I4_6:
                case Code.Ldc_I4_7:
                case Code.Ldc_I4_8:
                case Code.Ldc_I4_S:
                case Code.Dup:
                case Code.Pop:
                case Code.Jmp:
                case Code.Call:
                case Code.Calli:
                case Code.Callvirt:
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
                case Code.Sub:
                case Code.Mul:
                case Code.Div:
                case Code.Div_Un:
                case Code.Rem:
                case Code.Rem_Un:
                case Code.And:
                case Code.Or:
                case Code.Xor:
                case Code.Shl:
                case Code.Shr:
                case Code.Shr_Un:
                case Code.Neg:
                case Code.Not:
                case Code.Conv_I1:
                case Code.Conv_I2:
                case Code.Conv_I4:
                case Code.Conv_I8:
                case Code.Conv_R4:
                case Code.Conv_R8:
                case Code.Conv_U4:
                case Code.Conv_U8:
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
                case Code.Ceq:
                case Code.Cgt:
                case Code.Cgt_Un:
                case Code.Clt:
                case Code.Clt_Un:
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
            if (IsMergingFlow)
                for (var i = 0; i < StackLocations.Length; ++i)
                    if (StackLocations[i] != null)
                    {
                        var op = new OpPhi
                        {
                            Result = StackLocations[i].ID,
                            ResultType = StackLocations[i].Type.TypeID,
                            IDs = Incoming.Select(s => s.StackLocations[i].ID).ToArray()
                        };
                        Instructions.Add(op);
                    }
        }

        /// <summary>
        /// Create all ops
        /// </summary>
        public IEnumerable<Instruction> CreateOps()
        {
            return Instructions;
        }
    }
}
