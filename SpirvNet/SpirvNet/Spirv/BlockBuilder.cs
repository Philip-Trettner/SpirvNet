using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Ops.FlowControl;

namespace SpirvNet.Spirv
{
    /// <summary>
    /// Build helper for a function block
    /// </summary>
    public class BlockBuilder
    {
        /// <summary>
        /// Start label
        /// </summary>
        private OpLabel opLabel;

        /// <summary>
        /// variable declarations
        /// </summary>
        private readonly List<Instruction> declarations = new List<Instruction>(); 

        /// <summary>
        /// block instructions
        /// </summary>
        private readonly List<Instruction> instructions = new List<Instruction>();

        /// <summary>
        /// Generate block instructions (opLabel - branchOp)
        /// </summary>
        public IEnumerable<Instruction> GenerateInstructions()
        {
            yield return opLabel;
            foreach (var op in declarations)
                yield return op;
            foreach (var op in instructions)
                yield return op;
        }
    }
}
