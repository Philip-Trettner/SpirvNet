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
    public sealed class OpTypeSampler : Instruction
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

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + Result + ", " + SampledType + ", " + Dim + ", " + Content + ", " + Arrayed + ", " + Compare + ", " + MS + ')';

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TypeSampler);
            var i = 1;
            Result = new ID(codes[start + i++]);
            SampledType = new ID(codes[start + i++]);
            Dim = (Dim)codes[start + i++];
            Content = new LiteralNumber(codes[start + i++]);
            Arrayed = new LiteralNumber(codes[start + i++]);
            Compare = new LiteralNumber(codes[start + i++]);
            MS = new LiteralNumber(codes[start + i++]);
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
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Result;
                yield return SampledType;
            }
        }
    }
}
