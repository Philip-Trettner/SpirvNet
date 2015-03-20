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
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpTypePipe : Instruction
    {
        public override bool IsTypeDeclaration => true;
        public override OpCode OpCode => OpCode.TypePipe;
        public ID Result;
        public ID Type;
        public AccessQualifier AccessQualifier;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + Result + ", " + Type + ", " + AccessQualifier + ')';
    }
}
