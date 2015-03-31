using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpirvNet.Validation
{
    /// <summary>
    /// A strongly connected component of blocks
    /// May contain sub-components
    /// </summary>
    public class ValidatedComponent
    {
        /// <summary>
        /// List of contained blocks
        /// </summary>
        public readonly List<ValidatedBlock> Blocks = new List<ValidatedBlock>();

        /// <summary>
        /// An arbitrary entry point
        /// </summary>
        public ValidatedBlock EntryBlock { get; private set; }

        /// <summary>
        /// All exit blocks
        /// </summary>
        public readonly List<ValidatedBlock> ExitBlocks = new List<ValidatedBlock>();

        /// <summary>
        /// List of sub-components
        /// </summary>
        public readonly List<ValidatedComponent> SubComponents = new List<ValidatedComponent>();

        /// <summary>
        /// Parent component (can be null for TLCs)
        /// </summary>
        public readonly ValidatedComponent ParentComponent;

        /// <summary>
        /// Parent functions
        /// </summary>
        public readonly ValidatedFunction Function;

        public ValidatedComponent(ValidatedFunction function, ValidatedComponent parentComponent)
        {
            Function = function;
            ParentComponent = parentComponent;
        }

        /// <summary>
        /// Defines entry and exit blocks
        /// </summary>
        private void DefineEntryAndExit()
        {
            if (SubComponents.Count > 0)
                throw new InvalidOperationException("Does not work after sub-SCCs exist");

            // arbitrary entry
            EntryBlock = Blocks.FirstOrDefault(b => b.IncomingBlocks.Any(b2 => b2.InnerComponent != this)) ??
                         Function.StartBlock;

            // all exits
            ExitBlocks.AddRange(Blocks.Where(b => b.OutgoingBlocks.Any(b2 => b2.InnerComponent != this)));
        }

        /// <summary>
        /// Creates SCCs from a subgraph of blocks
        /// Annotates components in blocks
        /// </summary>
        internal static List<ValidatedComponent> FromBlockGraph(List<BlockSubnode> blocks, ValidatedFunction function, ValidatedComponent parent)
        {
            var sccs = new List<ValidatedComponent>();

            // calc SCCs
            // see http://en.wikipedia.org/wiki/Tarjan%27s_strongly_connected_components_algorithm
            var index = 0;
            var stack = new Stack<BlockSubnode>();

            // connect all blocks
            foreach (var block in blocks)
                if (block.Index == -1)
                    StrongConnect(block, stack, ref index, sccs, function, parent);

            // annotates SCCs
            foreach (var scc in sccs)
                foreach (var block in scc.Blocks)
                    block.Components.Add(scc);

            // define entry and exit points
            foreach (var scc in sccs)
                scc.DefineEntryAndExit();

            return sccs;
        }

        /// <summary>
        /// Helper for SCC
        /// </summary>
        internal static void StrongConnect(BlockSubnode v, Stack<BlockSubnode> stack, ref int index, List<ValidatedComponent> sccs, ValidatedFunction function, ValidatedComponent parent)
        {
            // Set the depth index for v to the smallest unused index
            v.Index = index;
            v.Lowlink = index;
            ++index;
            stack.Push(v);
            v.OnStack = true;

            // Consider successors of v
            foreach (var w in v.Outgoing)
            {
                if (w.Index == -1)
                {
                    // Successor w has not yet been visited; recurse on it
                    StrongConnect(w, stack, ref index, sccs, function, parent);
                    if (w.Lowlink < v.Lowlink)
                        v.Lowlink = w.Lowlink;
                }
                else if (w.OnStack)
                {
                    // Successor w is in stack S and hence in the current SCC
                    if (w.Index < v.Lowlink)
                        v.Lowlink = w.Index;
                }
            }

            // If v is a root node, pop the stack and generate an SCC
            if (v.Lowlink == v.Index)
            {
                var scc = new ValidatedComponent(function, parent);
                BlockSubnode w;
                do
                {
                    w = stack.Pop();
                    w.OnStack = false;
                    scc.Blocks.Add(w.Block);
                } while (w.Block != v.Block);

                // ignore non-looping single vertices
                if (scc.Blocks.Count == 1 && !scc.Blocks[0].OutgoingBlocks.Contains(scc.Blocks[0]))
                    return;
                
                sccs.Add(scc);
            }
        }

        /// <summary>
        /// Returns the induced subgraph of blocks (does not work after sub-SCCs were calculated)
        /// </summary>
        internal List<BlockSubnode> InducedSubgraph
        {
            get
            {
                if (SubComponents.Count > 0)
                    throw new InvalidOperationException("Does not work after sub-SCCs exist");
                if (EntryBlock?.InnerComponent != this)
                    throw new InvalidOperationException("Invalid entry point");

                var blocks = new List<BlockSubnode>();

                // add blocks
                foreach (var block in Blocks)
                {
                    // reserve nulls
                    while (blocks.Count <= block.Index)
                        blocks.Add(null);

                    // alloc block
                    blocks[block.Index] = new BlockSubnode(block);
                }

                // connect blocks
                foreach (var b1 in Blocks)
                    foreach (var b2 in b1.OutgoingBlocks)
                        if (b2.InnerComponent == this &&
                            b2 != EntryBlock) // do not connect to entry point
                        {
                            blocks[b1.Index].Outgoing.Add(blocks[b2.Index]);
                            blocks[b2.Index].Incoming.Add(blocks[b1.Index]);
                        }

                // remove empty entries
                blocks.RemoveAll(b => b == null);
                return blocks;
            }
        }

        /// <summary>
        /// Lines for a DOT cluster
        /// </summary>
        public IEnumerable<string> DotGraph
        {
            get
            {
                yield return string.Format("subgraph cluster{0}_{1} {{", Blocks[0].Index, Blocks.Count);

                foreach (var block in Blocks)
                    if (block.InnerComponent == this)
                        yield return string.Format("  b{0} [label=\"Block {1}\"];", block.Index, block.BlockID);

                foreach (var comp in SubComponents)
                    foreach (var line in comp.DotGraph)
                        yield return "  " + line;

                yield return "}";
            }
        }

        /// <summary>
        /// Performs recursive analysis of SCCs
        /// </summary>
        public void ComponentAnalysis()
        {
            if (SubComponents.Count > 0)
                throw new InvalidOperationException("Cannot analyse sub-SCCs more than once");

            // generate sub-SCCs
            SubComponents.AddRange(FromBlockGraph(InducedSubgraph, Function, this));

            // recursive analysis
            foreach (var component in SubComponents)
                component.ComponentAnalysis();
        }
    }
}
