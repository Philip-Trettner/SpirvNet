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
    /// OpTextureQueryLevels
    /// 
    /// Query the number of mipmap levels accessible through Sampler.
    /// 
    /// Result Type must be a scalar integer type. The result is the number of mipmap levels, as defined by the API specification.
    /// 
    /// Sampler must be an object of a type made by OpTypeSampler. Sampler must have a type with Dimensionality of 1D, 2D, 3D, or Cube.
    /// 
    /// TBD: The value zero will be returned if no texture or an incomplete texture is associated with Sampler.
    /// </summary>
    [DependsOn(LanguageCapability.Shader)]
    public sealed class OpTextureQueryLevels : TextureInstruction
    {
        public override bool IsTexture => true;
        public override OpCode OpCode => OpCode.TextureQueryLevels;
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
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TextureQueryLevels);
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
