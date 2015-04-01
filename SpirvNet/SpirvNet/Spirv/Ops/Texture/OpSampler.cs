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
    /// OpSampler
    /// 
    /// Create a sampler containing both a filter and texture.
    /// 
    /// Sampler must be an object whose type is from an OpTypeSampler. Its type must have its Content operand set to 0, indicating a texture with no filter.
    /// 
    /// Filter must be an object whose type is OpTypeFilter.
    /// 
    /// Result Type must be an OpTypeSampler whose Sampled Type, Dimensionality, Arrayed, Comparison, and Multisampled operands all equal those of this instruction&#8217;s Sampler operand. Further, the Result Type must have its Content operand set to 2, indicating both a texture and filter are present.
    /// </summary>
    public sealed class OpSampler : TextureInstruction
    {
        public override bool IsTexture => true;
        public override OpCode OpCode => OpCode.Sampler;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Sampler;
        public ID Filter;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Sampler) + ", " + StrOf(Filter) + ")";
        public override string ArgString => "Sampler: " + StrOf(Sampler) + ", " + "Filter: " + StrOf(Filter);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Sampler);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Sampler = new ID(codes[i++]);
            Filter = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Sampler.Value);
            code.Add(Filter.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Sampler;
                yield return Filter;
            }
        }
        #endregion
    }
}
