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
    /// OpTextureQuerySizeLod
    /// 
    /// Query the dimensions of the texture for Sampler for mipmap level for Level of Detail.
    /// 
    /// Result Type must be an integer type scalar or vector.  The number of components must be
    /// 1 for 1D Dimensionality,
    /// 2 for 2D, and Cube Dimensionalities,
    /// 3 for 3D Dimensionality,
    /// plus 1 more if the sampler type is arrayed. This vector is filled in with (width [, height] [, depth] [, elements]) where elements is the number of layers in a texture array, or the number of cubes in a cube-map array.
    /// 
    /// Sampler must be an object of a type made by OpTypeSampler. Sampler must have a type with Dimensionality of 1D, 2D, 3D, or Cube. Sampler cannot have a multisampled type. See OpTextureQuerySize for querying texture types lacking level of detail.
    /// 
    /// Level of Detail is used to compute which mipmap level to query, as described in the API specification.
    /// </summary>
    [DependsOn(LanguageCapability.Shader)]
    public sealed class OpTextureQuerySizeLod : TextureInstruction
    {
        public override bool IsTexture => true;
        public override OpCode OpCode => OpCode.TextureQuerySizeLod;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Sampler;
        public ID LevelOfDetail;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Sampler) + ", " + StrOf(LevelOfDetail) + ")";
        public override string ArgString => "Sampler: " + StrOf(Sampler) + ", " + "LevelOfDetail: " + StrOf(LevelOfDetail);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TextureQuerySizeLod);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Sampler = new ID(codes[i++]);
            LevelOfDetail = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Sampler.Value);
            code.Add(LevelOfDetail.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Sampler;
                yield return LevelOfDetail;
            }
        }
        #endregion
    }
}
