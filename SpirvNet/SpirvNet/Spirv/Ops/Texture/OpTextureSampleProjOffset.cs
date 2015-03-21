using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Texture
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    [DependsOn(LanguageCapability.Shader)]
    public sealed class OpTextureSampleProjOffset : Instruction
    {
        public override bool IsTexture => true;
        public override OpCode OpCode => OpCode.TextureSampleProjOffset;

        public ID ResultType;
        public ID Result;
        public ID Sampler;
        public ID Coordinate;
        public ID Offset;
        public ID? Bias;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + ResultType + ", " + Result + ", " + Sampler + ", " + Coordinate + ", " + Offset + ", " + Bias + ')';

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TextureSampleProjOffset);
            var i = 1;
            ResultType = new ID(codes[start + i++]);
            Result = new ID(codes[start + i++]);
            Sampler = new ID(codes[start + i++]);
            Coordinate = new ID(codes[start + i++]);
            Offset = new ID(codes[start + i++]);
            if (i < WordCount)
                Bias = new ID(codes[start + i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Sampler.Value);
            code.Add(Coordinate.Value);
            code.Add(Offset.Value);
            if (Bias.HasValue)
                code.Add(Bias.Value.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Sampler;
                yield return Coordinate;
                yield return Offset;
                if (Bias.HasValue)
                    yield return Bias.Value;
            }
        }
    }
}
