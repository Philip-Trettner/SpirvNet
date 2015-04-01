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
    /// OpTextureFetchTexelOffset
    /// 
    /// Fetch a single offset texel from a texture.
    /// 
    /// Result Type must be a vector of four components of the same type as Sampled Type of Sampler&#8217;s type.
    /// 
    /// Sampler must be an object of a type made by OpTypeSampler. It must have a Dimensionality of 1D, 2D, 3D, or Rect. It cannot have depth-comparison type (the type&#8217;s Compare operand must be 0).
    /// 
    /// Coordinate is an integer scalar or vector containing (u[, v] &#8230; [, array layer]) as needed by the definiton of Sampler.
    /// 
    /// Offset is added to (u, v, w) before texel lookup. It must be an &lt;id&gt; of an integer-based constant instruction of scalar or vector type. It is a compile-time error if these fall outside a target-dependent allowed range. The number of components in Offset must equal the number of components in Coordinate, minus the array layer component, if present.
    /// </summary>
    [DependsOn(LanguageCapability.Shader)]
    public sealed class OpTextureFetchTexelOffset : TextureInstruction
    {
        public override bool IsTexture => true;
        public override OpCode OpCode => OpCode.TextureFetchTexelOffset;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Sampler;
        public ID Coordinate;
        public ID Offset;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Sampler) + ", " + StrOf(Coordinate) + ", " + StrOf(Offset) + ")";
        public override string ArgString => "Sampler: " + StrOf(Sampler) + ", " + "Coordinate: " + StrOf(Coordinate) + ", " + "Offset: " + StrOf(Offset);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TextureFetchTexelOffset);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Sampler = new ID(codes[i++]);
            Coordinate = new ID(codes[i++]);
            Offset = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Sampler.Value);
            code.Add(Coordinate.Value);
            code.Add(Offset.Value);
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
            }
        }
        #endregion
    }
}
