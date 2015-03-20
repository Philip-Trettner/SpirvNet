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
        public bool IsBranching => Outgoing.Count > 1;

        /// <summary>
        /// Outgoing vertices
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

            // next op
            if (Instruction.Next != null &&
                OpCode.FlowControl != FlowControl.Branch)
            {
                var nextIdx = cfg.OffsetToIndex[Instruction.Next.Offset];
                ConnectTo(cfg.Vertices[nextIdx]);
            }

            // branching
            switch (OpCode.FlowControl)
            {
                case FlowControl.Branch:
                case FlowControl.Cond_Branch:
                    var operand = Instruction.Operand as Instruction;
                    if (operand != null)
                        ConnectTo(cfg.Vertices[cfg.OffsetToIndex[operand.Offset]]);
                    else
                    {
                        var instructions = Instruction.Operand as Instruction[];
                        if (instructions != null)
                            foreach (var instruction in instructions)
                                ConnectTo(cfg.Vertices[cfg.OffsetToIndex[instruction.Offset]]);
                        else
                            throw new NotSupportedException("Unknown operand");
                    }
                    break;
            }
        }

        private void ConnectTo(Vertex v)
        {
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
