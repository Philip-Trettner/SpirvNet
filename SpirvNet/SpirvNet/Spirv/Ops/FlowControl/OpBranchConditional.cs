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
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    public sealed class OpBranchConditional : Instruction
    {
        public override bool IsFlowControl => true;
        public override OpCode OpCode => OpCode.BranchConditional;

        public ID Condition;
        public ID TrueLabel;
        public ID FalseLabel;
        public LiteralNumber[] BranchWeights = new LiteralNumber[] { };

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + Condition + ", " + TrueLabel + ", " + FalseLabel + ", " + BranchWeights + ')';

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.BranchConditional);
            var i = 1;
            Condition = new ID(codes[start + i++]);
            TrueLabel = new ID(codes[start + i++]);
            FalseLabel = new ID(codes[start + i++]);
            var length = WordCount - i + 1;
            BranchWeights = new LiteralNumber[length];
            for (var k = 0; k < length; ++k)
                BranchWeights[k] = new LiteralNumber(codes[start + i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Condition.Value);
            code.Add(TrueLabel.Value);
            code.Add(FalseLabel.Value);
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
    }
}
