using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.ConstantCreation
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpConstantSampler : Instruction
    {
        public override bool IsConstantCreation => true;
        public override OpCode OpCode => OpCode.ConstantSampler;
        public ID ResultType;
        public ID Result;
        public LiteralNumber Mode;
        public LiteralNumber Param;
        public LiteralNumber Filter;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + ResultType + ", " + Result + ", " + Mode + ", " + Param + ", " + Filter + ')';
    }
}
