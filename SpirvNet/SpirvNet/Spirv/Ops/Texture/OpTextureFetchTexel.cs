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
    /// OpTextureFetchTexel
    /// 
    /// Fetch a single texel from a texture.
    /// 
    /// Result Type must be a vector of four components of the same type as Sampled Type of Sampler&#8217;s type.
    /// 
    /// Sampler must be an object of a type made by OpTypeSampler. It must have a Dimensionality of 1D, 2D, 3D, or Rect. It cannot have depth-comparison type (the type&#8217;s Compare operand must be 0).
    /// 
    /// Coordinate is an integer scalar or vector containing (u[, v] &#8230; [, array layer]) as needed by the definiton of Sampler.
    /// 
    /// Level of Detail explicitly controls the level of detail used when sampling.
    /// </summary>
    [DependsOn(LanguageCapability.Shader)]
    public sealed class OpTextureFetchTexel : TextureInstruction
    {
        public override bool IsTexture => true;
        public override OpCode OpCode => OpCode.TextureFetchTexel;
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
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TextureFetchTexel);
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
