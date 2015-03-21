using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.ConstantCreation
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpConstantSampler : Instruction
    {
        public override bool IsConstantCreation => true;
        public override OpCode OpCode => OpCode.ConstantSampler;

        public ID ResultType;
        public ID Result;
        public LiteralNumber Mode;
        public LiteralNumber Param;
        public LiteralNumber Filter;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + ResultType + ", " + Result + ", " + Mode + ", " + Param + ", " + Filter + ')';

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.ConstantSampler);
            var i = 1;
            ResultType = new ID(codes[start + i++]);
            Result = new ID(codes[start + i++]);
            Mode = new LiteralNumber(codes[start + i++]);
            Param = new LiteralNumber(codes[start + i++]);
            Filter = new LiteralNumber(codes[start + i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Mode.Value);
            code.Add(Param.Value);
            code.Add(Filter.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
            }
        }
    }
}
