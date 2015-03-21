using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SpirvNet.DotNet.CFG;
using SpirvNet.Spirv.Ops.ConstantCreation;

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
        /// Stack types
        /// </summary>
        public readonly SpirvType[] StackTypes;

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
        /// Position in stack (points to the first free pos)
        /// </summary>
        public int StackPosition { get; set; }
        
        private bool decoded = false; // true iff already decoded

        public MethodFrameState(Vertex vertex, MethodFrame frame)
        {
            Vertex = vertex;
            Frame = frame;

            LocalVars = new TypedLocation[frame.VarCount];
            StackLocations = new TypedLocation[frame.StackSize];
            StackTypes = new SpirvType[frame.StackSize];

            var cin = Vertex.Incoming.Count;
            var cout = Vertex.Outgoing.Count;

            if (cin == 0) IsEntryPoint = true;
            if (cout == 0) IsExitPoint = true;
            if (cin > 1) IsMergingFlow = true;
            if (cout > 1) IsDivergingFlow = true;
            if (cin == 1 && cout == 1) IsLinearFlow = true;
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
        /// Pushes a type of the stack
        /// </summary>
        private void Push(SpirvType type)
        {
            StackTypes[StackPosition] = type;
            StackLocations[StackPosition] = Frame.CreateLocation(type);
            ++StackPosition;
        }
        /// <summary>
        /// Pushes a type of the stack
        /// </summary>
        private void Push(TypeReference type) => Push(Frame.TypeBuilder.Create(type));

        /// <summary>
        /// Pops a value from the stack (returns the type)
        /// </summary>
        private SpirvType Pop()
        {
            var type = StackTypes[StackPosition];
            --StackPosition;
            return type;
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
                    LocalVars[i] = Frame.CreateLocation(Frame.LocalVarTypes[i]);

                for (var i = 0; i < LocalVars.Length; ++i)
                    if (StackTypes[i] != null)
                        StackLocations[i] = Frame.CreateLocation(StackTypes[i]);

                StackPosition = incomingState.StackPosition;
            }
            else // non-merging flow copies previous locations
            {
                for (var i = 0; i < LocalVars.Length; ++i)
                    LocalVars[i] = incomingState.LocalVars[i];
                for (var i = 0; i < LocalVars.Length; ++i)
                    StackLocations[i] = incomingState.StackLocations[i];

                StackPosition = incomingState.StackPosition;
            }

            var opc = Vertex.OpCode;
            var ins = Vertex.Instruction;

            switch (opc.Code)
            {
                case Code.Nop:
                    break;
                case Code.Break:
                case Code.Ldarg_0:
                case Code.Ldarg_1:
                case Code.Ldarg_2:
                case Code.Ldarg_3:
                case Code.Ldloc_0:
                case Code.Ldloc_1:
                case Code.Ldloc_2:
                case Code.Ldloc_3:
                case Code.Stloc_0:
                case Code.Stloc_1:
                case Code.Stloc_2:
                case Code.Stloc_3:
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
                case Code.Ldc_I4:
                case Code.Ldc_I8:
                case Code.Ldc_R4:
                case Code.Ldc_R8:
                case Code.Dup:
                case Code.Pop:
                case Code.Jmp:
                case Code.Call:
                case Code.Calli:
                case Code.Callvirt:
                case Code.Ret:
                case Code.Br_S:
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
                case Code.Br:
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
                case Code.Add:
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
            // TODO
        }
    }
}
