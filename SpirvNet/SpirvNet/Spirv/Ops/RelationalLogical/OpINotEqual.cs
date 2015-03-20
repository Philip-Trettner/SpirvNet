using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.RelationalLogical
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    public sealed class OpINotEqual : Instruction
    {
        public override bool IsRelationalLogical => true;
        public override OpCode OpCode => OpCode.INotEqual;
        public ID ResultType;
        public ID Result;
        public ID Operand1;
        public ID Operand2;
    }
}
