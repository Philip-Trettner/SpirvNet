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

        public BlockSubnode(ValidatedBlock block)
        {
            Block = block;
        }
    }
}
