using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Primitive
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    [DependsOn(LanguageCapability.Geom)]
    public sealed class OpEndPrimitive : Instruction
    {
        public override bool IsPrimitive => true;
        public override OpCode OpCode => OpCode.EndPrimitive;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ')';
    }
}
