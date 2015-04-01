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
    /// OpTextureSampleProjGradOffset
    /// 
    /// Sample a texture with an offset from a projective coordinate and an explicit gradient.
    /// 
    /// Result Type&#8217;s component type must be the same as Sampled Type of Sampler&#8217;s type. Result Type must be scalar if the Sampler&#8217;s type sets depth-comparison, and must be a vector of four components if the Sampler&#8217;s type does not set depth-comparison.
    /// 
    /// Sampler must be an object of a type made by OpTypeSampler.
    /// 
    /// Coordinate is a floating-point vector of four components containing (u [, v] [, Dref], q) or (u [, v] [, w], q), as needed by the definiton of Sampler, with the q component consumed for the projective division. That is, the actual sample coordinate will be (u/q [, v/q] [,Dref/q]) or (u/q [, v/q] [, w/q]), as needed by the definiton of Sampler.
    /// 
    /// dx and dy are explicit derivatives in the x and y direction to use in computing level of detail. Each is a scalar or vector containing (du/dx[, dv/dx] [, dw/dx]) and (du/dy[, dv/dy] [, dw/dy]). The number of components of each must equal the number of components in Coordinate, minus the array layer component, if present.
    /// 
    /// Offset is added to (u, v, w) before texel lookup. It must be an &lt;id&gt; of an integer-based constant instruction of scalar or vector type. It is a compile-time error if these fall outside a target-dependent allowed range. The number of components in Offset must equal the number of components in Coordinate, minus the array layer component, if present.
    /// </summary>
    [DependsOn(LanguageCapability.Shader)]
    public sealed class OpTextureSampleProjGradOffset : TextureInstruction
    {
        public override bool IsTexture => true;
        public override OpCode OpCode => OpCode.TextureSampleProjGradOffset;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Sampler;
        public ID Coordinate;
        public ID Dx;
        public ID Dy;
        public ID Offset;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Sampler) + ", " + StrOf(Coordinate) + ", " + StrOf(Dx) + ", " + StrOf(Dy) + ", " + StrOf(Offset) + ")";
        public override string ArgString => "Sampler: " + StrOf(Sampler) + ", " + "Coordinate: " + StrOf(Coordinate) + ", " + "Dx: " + StrOf(Dx) + ", " + "Dy: " + StrOf(Dy) + ", " + "Offset: " + StrOf(Offset);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TextureSampleProjGradOffset);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Sampler = new ID(codes[i++]);
            Coordinate = new ID(codes[i++]);
            Dx = new ID(codes[i++]);
            Dy = new ID(codes[i++]);
            Offset = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Sampler.Value);
            code.Add(Coordinate.Value);
            code.Add(Dx.Value);
            code.Add(Dy.Value);
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
                yield return Dx;
                yield return Dy;
                yield return Offset;
            }
        }
        #endregion
    }
}
