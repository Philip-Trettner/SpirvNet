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
    }
}
