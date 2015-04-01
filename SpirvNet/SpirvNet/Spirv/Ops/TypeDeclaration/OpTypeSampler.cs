using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.TypeDeclaration
{
    /// <summary>
    /// OpTypeSampler
    /// 
    /// Declare a new sampler type. Consumed, for example, by OpTextureSample.This type is opaque: values of this type have no defined physical size or bit pattern.
    /// 
    /// Sampled Type is a scalar type, of the type of the components resulting from sampling or loading through this sampler.
    /// 
    /// Dim is the texture dimensionality.
    /// 
    /// Content must be one of the following indicated values:
    /// 0 indicates a texture, no filter (no sampling state)
    /// 1 indicates an image
    /// 2 indicates both a texture and filter (sampling state), see OpTypeFilter
    /// 
    /// Arrayed must be one of the following indicated values:
    /// 0 indicates non-arrayed content
    /// 1 indicates arrayed content
    /// 
    /// Compare must be one of the following indicated values:
    /// 0 indicates depth comparisons are not done
    /// 1 indicates depth comparison are done
    /// 
    /// MS is multisampled and must be one of the following indicated values:
    /// 0 indicates single-sampled content
    /// 1 indicates multisampled content
    /// 
    /// Qualifier is an image access qualifier. See Access Qualifier.
    /// 
    /// Result &lt;id&gt; is the &lt;id&gt; of the new sampler type.
    /// </summary>
    public sealed class OpTypeSampler : TypeDeclarationInstruction
    {
        public override bool IsTypeDeclaration => true;
        public override OpCode OpCode => OpCode.TypeSampler;
        public override ID? ResultID => Result;

        public ID Result;
        public ID SampledType;
        public Dim Dim;
        public LiteralNumber Content;
        public LiteralNumber Arrayed;
        public LiteralNumber Compare;
        public LiteralNumber MS;
        public ID? Qualifier;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Result) + ", " + StrOf(SampledType) + ", " + StrOf(Dim) + ", " + StrOf(Content) + ", " + StrOf(Arrayed) + ", " + StrOf(Compare) + ", " + StrOf(MS) + ", " + StrOf(Qualifier) + ")";
        public override string ArgString => "SampledType: " + StrOf(SampledType) + ", " + "Dim: " + StrOf(Dim) + ", " + "Content: " + StrOf(Content) + ", " + "Arrayed: " + StrOf(Arrayed) + ", " + "Compare: " + StrOf(Compare) + ", " + "MS: " + StrOf(MS) + ", " + "Qualifier: " + StrOf(Qualifier);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TypeSampler);
            var i = start + 1;
            Result = new ID(codes[i++]);
            SampledType = new ID(codes[i++]);
            Dim = (Dim)codes[i++];
            Content = new LiteralNumber(codes[i++]);
            Arrayed = new LiteralNumber(codes[i++]);
            Compare = new LiteralNumber(codes[i++]);
            MS = new LiteralNumber(codes[i++]);
            if (i - start < WordCount)
                Qualifier = new ID(codes[i++]);
            else
                Qualifier = null;
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Result.Value);
            code.Add(SampledType.Value);
            code.Add((uint)Dim);
            code.Add(Content.Value);
            code.Add(Arrayed.Value);
            code.Add(Compare.Value);
            code.Add(MS.Value);
            if (Qualifier.HasValue)
                code.Add(Qualifier.Value.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Result;
                yield return SampledType;
                if (Qualifier.HasValue)
                    yield return Qualifier.Value;
            }
        }
        #endregion
    }
}
