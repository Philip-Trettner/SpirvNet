using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Helper;
using SpirvNet.Spirv;
using SpirvNet.Spirv.Ops.FlowControl;

namespace SpirvNet.Validation
{
    /// <summary>
    /// A validated and analysed block
    /// </summary>
    public class ValidatedBlock : IEquatable<ValidatedBlock>
    {
        /// <summary>
        /// Each block starts exactly with one label
        /// </summary>
        public readonly OpLabel BlockLabel;
        /// <summary>
        /// Block index (zero-based, no holes)
        /// </summary>
        public readonly int Index;

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

        /// <summary>
        /// Set of dominator nodes (blocks that are definitely executed before this one)
        /// </summary>
        public HashSet<ValidatedBlock> Dominators { get; internal set; }
        /// <summary>
        /// True iff b dominates this
        /// </summary>
        public bool DominatedBy(ValidatedBlock b) => Dominators.Contains(b);
        /// <summary>
        /// True iff b != this and b dominates this
        /// </summary>
        public bool StrictlyDominatedBy(ValidatedBlock b) => b != this && Dominators.Contains(b);
        /// <summary>
        /// Block that strictly dominates this block but not any other block that strictly dominates this
        /// </summary>
        public ValidatedBlock ImmediateDominator { get; internal set; }

        /// <summary>
        /// Ordered list of components that this block is part of (from outer to inner)
        /// </summary>
        public readonly List<ValidatedComponent> Components = new List<ValidatedComponent>();
        /// <summary>
        /// Innermost component
        /// </summary>
        public ValidatedComponent InnerComponent => Components.LastOrDefault();
        /// <summary>
        /// Outermost component
        /// </summary>
        public ValidatedComponent OuterComponent => Components.FirstOrDefault();

        public ValidatedBlock(OpLabel blockLabel, ValidatedFunction function, int index)
        {
            BlockLabel = blockLabel;
            Index = index;
            Function = function;
        }

        /// <summary>
        /// Dot node
        /// </summary>
        public string DotNode
        {
            get
            {
                var name = "";
                var vmod = Function.Module;
                var instrs = new[] { BlockLabel }.Concat(Instructions).ToArray();
                {
                    name += "{";
                    name += instrs.Select(i => vmod.IDStr(i.ResultID)).Aggregated("|");
                    name += "}|{";
                    name += instrs.Select(i => vmod.IDStr(i.ResultTypeID)).Aggregated("|");
                    name += "}|{";
                    name += instrs.Select(i => i.OpCode.ToString()).Aggregated("|");
                    name += "}|{";
                    name += instrs.Select(i => i.ArgString).Aggregated("|");
                    name += "}";
                }

                var attr = new List<string>
                {
                    string.Format("label=\"{0}\"", name),
                    "shape=record"
                };

                return string.Format("  b{0} [{1}]", BlockID.Value, attr.Aggregated(","));
            }
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

        public bool Equals(ValidatedBlock other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Index == other.Index;
        }

        public static bool operator ==(ValidatedBlock left, ValidatedBlock right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValidatedBlock left, ValidatedBlock right)
        {
            return !Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ValidatedBlock)obj);
        }

        public override int GetHashCode()
        {
            return (int)Index;
        }
    }
}
