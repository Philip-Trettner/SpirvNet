using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.FlowControl
{
    /// <summary>
    /// OpBranchConditional
    /// 
    /// If Condition is true, branch to True Label, otherwise branch to False Label.
    /// 
    /// Condition must be a Boolean type scalar.
    /// 
    /// True Label must be an OpLabel in the current function.
    /// 
    /// False Label must be an OpLabel in the current function.
    /// 
    /// Branch weights are unsigned 32-bit integer literals. There must be either no Branch Weights or exactly two branch weights. If present, the first is the weight for branching to True Label, and the second is the weight for branching to False Label. The implied probability that a branch is taken is its weight divided by the sum of the two Branch weights.
    /// 
    /// This instruction must be the last instruction in a block.
    /// </summary>
    public sealed class OpBranchConditional : FlowControlInstruction
    {
        public override bool IsFlowControl => true;
        public override OpCode OpCode => OpCode.BranchConditional;

        public ID Condition;
        public ID TrueLabel;
        public ID FalseLabel;
        public LiteralNumber[] BranchWeights = { };

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Condition) + ", " + StrOf(TrueLabel) + ", " + StrOf(FalseLabel) + ", " + StrOf(BranchWeights) + ")";
        public override string ArgString => "Condition: " + StrOf(Condition) + ", " + "TrueLabel: " + StrOf(TrueLabel) + ", " + "FalseLabel: " + StrOf(FalseLabel) + ", " + "BranchWeights: " + StrOf(BranchWeights);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.BranchConditional);
            var i = start + 1;
            Condition = new ID(codes[i++]);
            TrueLabel = new ID(codes[i++]);
            FalseLabel = new ID(codes[i++]);
            var length = WordCount - (i - start);
            BranchWeights = new LiteralNumber[length];
            for (var k = 0; k < length; ++k)
                BranchWeights[k] = new LiteralNumber(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Condition.Value);
            code.Add(TrueLabel.Value);
            code.Add(FalseLabel.Value);
            if (BranchWeights != null)
                foreach (var val in BranchWeights)
                    code.Add(val.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Condition;
                yield return TrueLabel;
                yield return FalseLabel;
            }
        }
        #endregion
    }
}
