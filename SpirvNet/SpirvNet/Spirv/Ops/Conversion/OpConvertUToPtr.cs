using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Conversion
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    [DependsOn(LanguageCapability.Addr)]
    public sealed class OpConvertUToPtr : Instruction
    {
        public override bool IsConversion => true;
        public override OpCode OpCode => OpCode.ConvertUToPtr;
        public ID ResultType;
        public ID Result;
        public ID IntegerValue;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + ResultType + ", " + Result + ", " + IntegerValue + ')';
    }
}
