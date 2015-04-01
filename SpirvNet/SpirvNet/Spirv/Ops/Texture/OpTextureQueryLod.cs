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
    /// OpTextureQueryLod
    /// 
    /// Query the mipmap level and the level of detail for a hypothetical sampling of Sampler at Coordinate using an implicit level of detail.
    /// 
    /// Result Type must be a two-component floating-point type vector.
    /// The first component of the result will contain the mipmap array layer.
    /// The second component of the result will contain the implicit level of detail relative to the base level.
    /// 
    /// TBD: Does this need the GLSL pseudo code for computing array layer and LoD?
    /// 
    /// Sampler must be an object of a type made by OpTypeSampler. Sampler must have a type with Dimensionality of 1D, 2D, 3D, or Cube.
    /// 
    /// Coordinate is a floating-point scalar or vector containing (u[, v] &#8230; [, array layer]) as needed by the definiton of Sampler.
    /// 
    /// If called on an incomplete texture, the results are undefined.
    /// 
    /// This instruction is only allowed under the Fragment Execution Model. In addition, it consumes an implicit derivative that can be affected by code motion.
    /// </summary>
    [DependsOn(LanguageCapability.Shader)]
    public sealed class OpTextureQueryLod : TextureInstruction
    {
        public override bool IsTexture => true;
        public override OpCode OpCode => OpCode.TextureQueryLod;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Sampler;
        public ID Coordinate;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Sampler) + ", " + StrOf(Coordinate) + ")";
        public override string ArgString => "Sampler: " + StrOf(Sampler) + ", " + "Coordinate: " + StrOf(Coordinate);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TextureQueryLod);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Sampler = new ID(codes[i++]);
            Coordinate = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Sampler.Value);
            code.Add(Coordinate.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Sampler;
                yield return Coordinate;
            }
        }
        #endregion
    }
}
