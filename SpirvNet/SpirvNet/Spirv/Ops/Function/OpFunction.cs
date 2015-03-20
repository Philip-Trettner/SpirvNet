using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Function
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    public sealed class OpFunction : Instruction
    {
        public override bool IsFunction => true;
        public override OpCode OpCode => OpCode.Function;
        public ID ResultType;
        public ID Result;
        public FunctionControlMask FunctionControlMask;
        public ID FunctionType;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + ResultType + ", " + Result + ", " + FunctionControlMask + ", " + FunctionType + ')';
    }
}
