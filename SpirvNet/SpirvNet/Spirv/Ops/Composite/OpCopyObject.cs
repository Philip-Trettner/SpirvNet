using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Composite
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    public sealed class OpCopyObject : Instruction
    {
        public override bool IsComposite => true;
        public override OpCode OpCode => OpCode.CopyObject;
        public ID ResultType;
        public ID Result;
        public ID Operand;
    }
}
