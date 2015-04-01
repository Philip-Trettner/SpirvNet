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
    /// OpTextureSampleProj
    /// 
    /// Sample a texture with a projective coordinate using an implicit level of detail.
    /// 
    /// Result Type&#8217;s component type must be the same as Sampled Type of Sampler&#8217;s type. Result Type must be scalar if the Sampler&#8217;s type sets depth-comparison, and must be a vector of four components if the Sampler&#8217;s type does not set depth-comparison.
    /// 
    /// Sampler must be an object of a type made by OpTypeSampler.
    /// 
    /// Coordinate is a floating-point vector of four components containing (u [, v] [, Dref], q) or (u [, v] [, w], q), as needed by the definiton of Sampler, with the q component consumed for the projective division. That is, the actual sample coordinate will be (u/q [, v/q] [,Dref/q]) or (u/q [, v/q] [, w/q]), as needed by the definiton of Sampler.
    /// 
    /// Bias is an optional operand.  If present, it is used as a bias to the implicit level of detail.
    /// 
    /// This instruction is only allowed under the Fragment Execution Model. In addition, it consumes an implicit derivative that can be affected by code motion.
    /// </summary>
    [DependsOn(LanguageCapability.Shader)]
    public sealed class OpTextureSampleProj : TextureInstruction
    {
        public override bool IsTexture => true;
        public override OpCode OpCode => OpCode.TextureSampleProj;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Sampler;
        public ID Coordinate;
        public ID? Bias;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Sampler) + ", " + StrOf(Coordinate) + ", " + StrOf(Bias) + ")";
        public override string ArgString => "Sampler: " + StrOf(Sampler) + ", " + "Coordinate: " + StrOf(Coordinate) + ", " + "Bias: " + StrOf(Bias);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TextureSampleProj);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Sampler = new ID(codes[i++]);
            Coordinate = new ID(codes[i++]);
            if (i - start < WordCount)
                Bias = new ID(codes[i++]);
            else
                Bias = null;
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Sampler.Value);
            code.Add(Coordinate.Value);
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
                if (Bias.HasValue)
                    yield return Bias.Value;
            }
        }
        #endregion
    }
}
