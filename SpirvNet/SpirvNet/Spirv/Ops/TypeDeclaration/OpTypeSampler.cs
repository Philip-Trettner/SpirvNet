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
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    public sealed class OpTypeSampler : TypeDeclarationInstruction
    {
        public override bool IsTypeDeclaration => true;
        public override OpCode OpCode => OpCode.TypeSampler;

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
