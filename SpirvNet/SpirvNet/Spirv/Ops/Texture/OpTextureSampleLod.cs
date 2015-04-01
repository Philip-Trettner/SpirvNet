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
    /// OpTextureSampleLod
    /// 
    /// Sample a texture using an explicit level of detail.
    /// 
    /// Result Type&#8217;s component type must be the same as Sampled Type of Sampler&#8217;s type. Result Type must be scalar if the Sampler&#8217;s type sets depth-comparison, and must be a vector of four components if the Sampler&#8217;s type does not set depth-comparison.
    /// 
    /// Sampler must be an object of a type made by OpTypeSampler.
    /// 
    /// Coordinate is a floating-point scalar or vector containing (u[, v] &#8230; [, array layer] [, Dref]) as needed by the definiton of Sampler. It may be a vector larger than needed, but all unused components will appear after all used components.
    /// 
    /// Level of Detail explicitly controls the level of detail used when sampling.
    /// </summary>
    [DependsOn(LanguageCapability.Shader)]
    public sealed class OpTextureSampleLod : TextureInstruction
    {
        public override bool IsTexture => true;
        public override OpCode OpCode => OpCode.TextureSampleLod;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Sampler;
        public ID Coordinate;
        public ID LevelOfDetail;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Sampler) + ", " + StrOf(Coordinate) + ", " + StrOf(LevelOfDetail) + ")";
        public override string ArgString => "Sampler: " + StrOf(Sampler) + ", " + "Coordinate: " + StrOf(Coordinate) + ", " + "LevelOfDetail: " + StrOf(LevelOfDetail);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TextureSampleLod);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Sampler = new ID(codes[i++]);
            Coordinate = new ID(codes[i++]);
            LevelOfDetail = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Sampler.Value);
            code.Add(Coordinate.Value);
            code.Add(LevelOfDetail.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Sampler;
                yield return Coordinate;
                yield return LevelOfDetail;
            }
        }
        #endregion
    }
}
