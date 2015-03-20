using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Extension
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    public sealed class OpExtInst : Instruction
    {
        public override bool IsExtension => true;
        public override OpCode OpCode => OpCode.ExtInst;
        public ID ResultType;
        public ID Result;
        public ID Set;
        public LiteralNumber Instruction;
    }
}
