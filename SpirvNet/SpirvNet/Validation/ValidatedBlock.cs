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

        /// <summary>
        /// Blocks that branch into this one
        /// </summary>
        public readonly List<ValidatedBlock> IncomingBlocks = new List<ValidatedBlock>();
        /// <summary>
        /// Blocks that this one branches into
        /// </summary>
        public readonly List<ValidatedBlock> OutgoingBlocks = new List<ValidatedBlock>();

        /// <summary>
        /// Literal to target (OpSwitch)
        /// 1 -> TrueLabel, 0 -> FalseLabel (OpBranchConditional)
        /// </summary>
        public readonly Dictionary<uint, ValidatedBlock> LiteralTargets = new Dictionary<uint, ValidatedBlock>();
        /// <summary>
        /// Unconditional target (OpBranch)
        /// Default target (OpSwitch)
        /// </summary>
        public ValidatedBlock DefaultTarget { get; private set; }

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

        /// <summary>
        /// Adds a branch target block
        /// </summary>
        public void AddBranchTarget(ValidatedBlock block, uint? literal, Instruction op)
        {
            OutgoingBlocks.Add(block);
            block.IncomingBlocks.Add(this);

            if (literal.HasValue)
            {
                if (LiteralTargets.ContainsKey(literal.Value))
                    throw new ValidationException(op, "Literal " + literal + " is specified multiple times.");
                LiteralTargets.Add(literal.Value, block);
            }
            else
            {
                if (DefaultTarget != null)
                    throw new ValidationException(op, "Unconditional/default target specified multiple times.");
                DefaultTarget = block;
            }
        }
    }
}
