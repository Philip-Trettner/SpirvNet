using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpirvNet.Validation
{
    /// <summary>
    /// A sub-graph of the block network
    /// </summary>
    class BlockSubnode
    {
        /// <summary>
        /// Incoming nodes
        /// </summary>
        public readonly List<BlockSubnode> Incoming = new List<BlockSubnode>();
        /// <summary>
        /// Outgoing nodes
        /// </summary>
        public readonly List<BlockSubnode> Outgoing = new List<BlockSubnode>();

        /// <summary>
        /// Represented block
        /// </summary>
        public readonly ValidatedBlock Block;

        // see http://en.wikipedia.org/wiki/Tarjan%27s_strongly_connected_components_algorithm
        public int Index = -1;
        public int Lowlink = -1;
        public bool OnStack = false;

        /// <summary>
        /// True iff this is an entry node
        /// </summary>
        public bool IsEntry = false;
        /// <summary>
        /// True iff this is an exit node
        /// </summary>
        public bool IsExit = false;
        /// <summary>
        /// True iff this node loops to an entry node (such connections are not contained in outgoing)
        /// </summary>
        public bool LoopsToEntry = false;

        public BlockSubnode(ValidatedBlock block)
        {
            Block = block;
        }

        /// <summary>
        /// Set of all reachables blocks
        /// </summary>
        public HashSet<BlockSubnode> Reachables
        {
            get
            {
                var blocks = new HashSet<BlockSubnode>();
                var s = new Stack<BlockSubnode>(Outgoing);
                while (s.Count > 0)
                {
                    var b = s.Pop();
                    if (blocks.Add(b))
                        foreach (var ob in b.Outgoing)
                            s.Push(ob);
                }
                return blocks;
            }
        }
    }
}
