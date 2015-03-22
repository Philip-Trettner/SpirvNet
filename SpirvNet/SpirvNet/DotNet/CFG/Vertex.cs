using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SpirvNet.Spirv.Ops.Composite;

namespace SpirvNet.DotNet.CFG
{
    /// <summary>
    /// A vertex of a CFG Graph
    /// </summary>
    sealed class Vertex
    {
        /// <summary>
        /// Represented instruction
        /// </summary>
        public Instruction Instruction;
        /// <summary>
        /// OpCode
        /// </summary>
        public OpCode OpCode;

        /// <summary>
        /// Vertex index
        /// </summary>
        public readonly int Index;

        /// <summary>
        /// True iff vertex is branching
        /// </summary>
        public bool IsBranching => IsUnconditionalBranch | IsConditionalBranch || IsSwitch;
        public bool IsUnconditionalBranch { get; private set; }
        public bool IsConditionalBranch { get; private set; }
        public bool IsSwitch { get; private set; }

        /// <summary>
        /// If true, this vertex is target of a branch (and thus requires an OpLabel)
        /// </summary>
        public bool IsBranchTarget { get; private set; }

        /// <summary>
        /// Outgoing vertices
        /// Ordering is:
        ///   Branch: [Target]
        ///   BranchCond: [Next, Target]
        ///   Switch: [Default, C1, C2, ...]
        /// </summary>
        public readonly List<Vertex> Outgoing = new List<Vertex>();
        /// <summary>
        /// Incoming vertices
        /// </summary>
        public readonly List<Vertex> Incoming = new List<Vertex>();

        public Vertex(Instruction instruction, int index)
        {
            Instruction = instruction;
            OpCode = Instruction.OpCode;
            Index = index;
        }

        /// <summary>
        /// Build CFG graph
        /// </summary>
        public void Build(ControlFlowGraph cfg)
        {
            if (OpCode.FlowControl == FlowControl.Return)
                return; // return has no outgoing

            Instruction operand;
            int nextIdx;

            switch (OpCode.Code)
            {
                // unconditional branches
                case Code.Br:
                case Code.Br_S:
                    // taken
                    operand = (Instruction)Instruction.Operand;
                    ConnectTo(cfg.Vertices[cfg.OffsetToIndex[operand.Offset]], true);

                    IsUnconditionalBranch = true;
                    break;

                // conditional branches
                case Code.Brfalse:
                case Code.Brfalse_S:
                case Code.Brtrue:
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
                    // not taken
                    nextIdx = cfg.OffsetToIndex[Instruction.Next.Offset];
                    ConnectTo(cfg.Vertices[nextIdx], true);

                    // taken
                    operand = (Instruction)Instruction.Operand;
                    ConnectTo(cfg.Vertices[cfg.OffsetToIndex[operand.Offset]], true);

                    IsConditionalBranch = true;
                    break;

                // switch
                case Code.Switch:
                    // default
                    nextIdx = cfg.OffsetToIndex[Instruction.Next.Offset];
                    ConnectTo(cfg.Vertices[nextIdx], true);

                    // cases
                    var instructions = (Instruction[])Instruction.Operand;
                    foreach (var instruction in instructions)
                        ConnectTo(cfg.Vertices[cfg.OffsetToIndex[instruction.Offset]], true);
                    break;

                // not branching
                default:
                    if (OpCode.FlowControl == FlowControl.Branch ||
                        OpCode.FlowControl == FlowControl.Cond_Branch)
                        throw new InvalidOperationException("No branching here");

                    nextIdx = cfg.OffsetToIndex[Instruction.Next.Offset];
                    ConnectTo(cfg.Vertices[nextIdx], false);
                    break;
            }
        }

        private void ConnectTo(Vertex v, bool branchTarget)
        {
            if (branchTarget)
                v.IsBranchTarget = true;

            v.Incoming.Add(this);
            this.Outgoing.Add(v);
        }

        public IEnumerable<string> DotLines
        {
            get
            {
                var attr = new List<string> { string.Format("label=\"{0}\"", ToString()) };

                if (OpCode.FlowControl == FlowControl.Call)
                    attr.Add("shape=box");
                if (OpCode.FlowControl == FlowControl.Return)
                    attr.Add("fillcolor=red");
                if (OpCode.FlowControl == FlowControl.Branch)
                    attr.Add("fillcolor=yellow");
                if (OpCode.FlowControl == FlowControl.Cond_Branch)
                    attr.Add("fillcolor=lime");

                yield return string.Format("v{0} [{1}];", Index, attr.Aggregate((s1, s2) => s1 + "," + s2));

                foreach (var v in Outgoing)
                    yield return string.Format("v{0} -> v{1};", Index, v.Index);
            }
        }

        public override string ToString()
        {
            var s = string.Format("[{1}] {0}", Instruction.OpCode.Code, Instruction.Offset);

            if (OpCode.FlowControl == FlowControl.Call)
                s += ": " + Instruction.Operand;
            else if (Instruction.Operand is ParameterDefinition)
                s += " (param: " + Instruction.Operand + ")";
            else if (Instruction.Operand is VariableDefinition)
                s += " (var: " + Instruction.Operand + ")";
            else if (Instruction.Operand is MethodReference)
                s += " (method: " + Instruction.Operand + ")";
            else if (Instruction.Operand == null)
            { /* no-op */ }
            else if (Instruction.Operand.GetType().IsValueType)
                s += " (const: " + Instruction.Operand + ")";

            return s;
        }
    }
}
