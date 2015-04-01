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
    /// OpTextureQuerySize
    /// 
    /// Query the dimensions of the texture for Sampler, with no level of detail.
    /// 
    /// Result Type must be an integer type scalar or vector.  The number of components must be
    /// 1 for Buffer Dimensionality,
    /// 2 for 2D and Rect Dimensionalities,
    /// plus 1 more if the sampler type is arrayed. This vector is filled in with (width [, height] [, elements]) where elements is the number of layers in a texture array.
    /// 
    /// Sampler must be an object of a type made by OpTypeSampler. Sampler must have a type with Dimensionality of Rect or Buffer, or be multisampled 2D. Sampler cannot have a texture with levels of detail; there is no implicit level-of-detail consumed by this instruction. See OpTextureQuerySizeLod for querying textures having level of detail.
    /// </summary>
    [DependsOn(LanguageCapability.Shader)]
    public sealed class OpTextureQuerySize : TextureInstruction
    {
        public override bool IsTexture => true;
        public override OpCode OpCode => OpCode.TextureQuerySize;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Sampler;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Sampler) + ")";
        public override string ArgString => "Sampler: " + StrOf(Sampler);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TextureQuerySize);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Sampler = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Sampler.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Sampler;
            }
        }
        #endregion
    }
}
