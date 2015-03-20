using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Memory
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    [DependsOn(LanguageCapability.Addr)]
    public sealed class OpCopyMemorySized : Instruction
    {
        public override bool IsMemory => true;
        public override OpCode OpCode => OpCode.CopyMemorySized;
        public ID Target;
        public ID Source;
        public ID Size;
        public MemoryAccess[] MemoryAccess;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + Target + ", " + Source + ", " + Size + ", " + MemoryAccess + ')';
    }
}
