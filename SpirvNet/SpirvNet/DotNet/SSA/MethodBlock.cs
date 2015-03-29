using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Ops.FlowControl;

namespace SpirvNet.DotNet.SSA
{
    /// <summary>
    /// A block inside a method
    /// Starts with a branch-target instruction and ends with a branching instruction
    /// </summary>
    public class MethodBlock
    {
        /// <summary>
        /// Labeled block start
        /// </summary>
        public MethodFrameState BlockStart => States.First();
        /// <summary>
        /// Block end (is not required to be branching in .NET but must be in SPIR-V)
        /// </summary>
        public MethodFrameState BlockEnd => States.Last();

        /// <summary>
        /// List of method states
        /// </summary>
        public readonly List<MethodFrameState> States = new List<MethodFrameState>();

        /// <summary>
        /// Incoming blocks
        /// </summary>
        public readonly List<MethodBlock> Incoming = new List<MethodBlock>();
        /// <summary>
        /// Outgoing blocks
        /// </summary>
        public readonly List<MethodBlock> Outgoing = new List<MethodBlock>();

        /// <summary>
        /// Throws if block is invalid
        /// </summary>
        public void Validate()
        {
            if (States.Count == 0)
                throw new InvalidOperationException("No states");

            if (BlockStart.BlockLabel == null)
                throw new InvalidOperationException("No label on start");

            foreach (var state in States)
            {
                if (state.BlockLabel != null && state != BlockStart)
                    throw new InvalidOperationException("Label on non-start state");

                if (state.Vertex.IsBranching && state != BlockEnd)
                    throw new InvalidOperationException("Branching on non-end state");
            }
        }

        /// <summary>
        /// Adds a connection between two blocks
        /// </summary>
        public static void AddConnection(MethodFrameState from, MethodFrameState to)
        {
            if (from.Block == to.Block)
                return; // no-op

            if (!from.Block.Outgoing.Contains(to.Block))
                from.Block.Outgoing.Add(to.Block);

            if (!to.Block.Incoming.Contains(from.Block))
                to.Block.Incoming.Add(from.Block);
        }

        /// <summary>
        /// Add missing branch operations
        /// </summary>
        public void AddMissingBranches()
        {
            if (!BlockEnd.Vertex.IsBranching && !BlockEnd.IsExitPoint)
            {
                if (Outgoing.Count != 1)
                    throw new InvalidOperationException("Non-branching non-exit state with more or less than 1 successor?");

                // add branch
                BlockEnd.Instructions.Add(new OpBranch { TargetLabel = Outgoing[0].BlockStart.BlockLabel.Result });
            }
        }
    }
}
