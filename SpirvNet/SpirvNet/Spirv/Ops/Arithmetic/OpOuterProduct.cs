using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Arithmetic
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    [DependsOn(LanguageCapability.Matrix)]
    public sealed class OpOuterProduct : Instruction
    {
        public override bool IsArithmetic => true;
        public override OpCode OpCode => OpCode.OuterProduct;
        public ID ResultType;
        public ID Result;
        public ID Vector1;
        public ID Vector2;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + ResultType + ", " + Result + ", " + Vector1 + ", " + Vector2 + ')';
    }
}
