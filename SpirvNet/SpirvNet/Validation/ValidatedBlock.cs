using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv;
using SpirvNet.Spirv.Ops.FlowControl;

namespace SpirvNet.Validation
{
    /// <summary>
    /// A validated and analysed block
    /// </summary>
    public class ValidatedBlock
    {
        /// <summary>
        /// Each block starts exactly with one label
        /// </summary>
        public readonly OpLabel BlockLabel;

        /// <summary>
        /// Parent function
        /// </summary>
        public readonly ValidatedFunction Function;

        /// <summary>
        /// Branching control flow at the end of a block
        /// </summary>
        public FlowControlInstruction BlockEnd { get; set; }

        /// <summary>
        /// List of instructions (without starting label)
        /// </summary>
        public readonly List<Instruction> Instructions = new List<Instruction>();

        public ValidatedBlock(OpLabel blockLabel, ValidatedFunction function)
        {
            BlockLabel = blockLabel;
            Function = function;
        }

        /// <summary>
        /// Block ID
        /// </summary>
        public ID BlockID => BlockLabel.Result;

        /// <summary>
        /// Adds an instruction
        /// </summary>
        public void AddInstruction(Instruction op)
        {
            Instructions.Add(op);
        }
    }
}
