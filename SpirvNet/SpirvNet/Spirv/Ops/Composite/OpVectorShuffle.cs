using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Composite
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    public sealed class OpVectorShuffle : Instruction
    {
        public override bool IsComposite => true;
        public override OpCode OpCode => OpCode.VectorShuffle;

        public ID ResultType;
        public ID Result;
        public ID Vector1;
        public ID Vector2;
        public LiteralNumber[] Components = new LiteralNumber[] { };

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + ResultType + ", " + Result + ", " + Vector1 + ", " + Vector2 + ", " + Components + ')';

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.VectorShuffle);
            var i = 1;
            ResultType = new ID(codes[start + i++]);
            Result = new ID(codes[start + i++]);
            Vector1 = new ID(codes[start + i++]);
            Vector2 = new ID(codes[start + i++]);
            var length = WordCount - i + 1;
            Components = new LiteralNumber[length];
            for (var k = 0; k < length; ++k)
                Components[k] = new LiteralNumber(codes[start + i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Vector1.Value);
            code.Add(Vector2.Value);
            foreach (var val in Components)
                code.Add(val.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Vector1;
                yield return Vector2;
            }
        }
    }
}
