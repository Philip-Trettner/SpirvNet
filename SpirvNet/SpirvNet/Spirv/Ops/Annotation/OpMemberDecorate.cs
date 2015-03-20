using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Annotation
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    public sealed class OpMemberDecorate : Instruction
    {
        public override bool IsAnnotation => true;
        public override OpCode OpCode => OpCode.MemberDecorate;
        public ID StructureType;
        public LiteralNumber Member;
        public Decoration Decoration;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + StructureType + ", " + Member + ", " + Decoration + ')';
    }
}
