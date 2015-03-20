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
    public sealed class OpMatrixTimesMatrix : Instruction
    {
        public override bool IsArithmetic => true;
        public override OpCode OpCode => OpCode.MatrixTimesMatrix;
        public ID ResultType;
        public ID Result;
        public ID LeftMatrix;
        public ID RightMatrix;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + ResultType + ", " + Result + ", " + LeftMatrix + ", " + RightMatrix + ')';
    }
}
