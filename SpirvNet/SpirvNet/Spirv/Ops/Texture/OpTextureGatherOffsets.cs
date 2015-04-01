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
    /// OpTextureGatherOffsets
    /// 
    /// Gathers the requested component from four offset sampled texels.
    /// 
    /// Result Type must be a vector of four components of the same type as Sampled Type of Sampler&#8217;s type. The result has one component per gathered texel.
    /// 
    /// Sampler must be an object of a type made by OpTypeSampler. It must have a Dimensionality of 2D or Rect.
    /// 
    /// Coordinate is a floating-point scalar or vector containing (u[, v] &#8230; [, array layer] [, Dref]) as needed by the definiton of Sampler.
    /// 
    /// Component is component number that will be gathered from all four texels.  It must be 0, 1, 2 or 3.
    /// 
    /// Offsets must be an &lt;id&gt; of a constant instruction making an array of size four of vectors of two integer components. Each gathered texel is identified by adding one of these array elements to the (u, v) sampled location. It is a compile-time error if this falls outside a target-dependent allowed range.
    /// </summary>
    [DependsOn(LanguageCapability.Shader)]
    public sealed class OpTextureGatherOffsets : TextureInstruction
    {
        public override bool IsTexture => true;
        public override OpCode OpCode => OpCode.TextureGatherOffsets;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Sampler;
        public ID Coordinate;
        public ID Component;
        public ID Offsets;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Sampler) + ", " + StrOf(Coordinate) + ", " + StrOf(Component) + ", " + StrOf(Offsets) + ")";
        public override string ArgString => "Sampler: " + StrOf(Sampler) + ", " + "Coordinate: " + StrOf(Coordinate) + ", " + "Component: " + StrOf(Component) + ", " + "Offsets: " + StrOf(Offsets);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TextureGatherOffsets);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Sampler = new ID(codes[i++]);
            Coordinate = new ID(codes[i++]);
            Component = new ID(codes[i++]);
            Offsets = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Sampler.Value);
            code.Add(Coordinate.Value);
            code.Add(Component.Value);
            code.Add(Offsets.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Sampler;
                yield return Coordinate;
                yield return Component;
                yield return Offsets;
            }
        }
        #endregion
    }
}
