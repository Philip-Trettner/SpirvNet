using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Validation;

namespace SpirvNet.GLSL
{
    /// <summary>
    /// Statement for a ValidatedBlock (excluding branches)
    /// </summary>
    class BlockStatement : Statement
    {
        /// <summary>
        /// Represented block
        /// </summary>
        public readonly ValidatedBlock Block;

        public BlockStatement(ValidatedBlock block)
        {
            Block = block;
        }

        public override IEnumerable<string> CodeLines
        {
            get
            {
                // TODO
                yield return "// Block " + Block.BlockID;
                yield return "// TODO";
            }
        }
    }
}
