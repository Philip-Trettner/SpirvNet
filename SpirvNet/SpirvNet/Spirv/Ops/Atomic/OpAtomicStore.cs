using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Atomic
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    public sealed class OpAtomicStore : Instruction
    {
        public override bool IsAtomic => true;
        public override OpCode OpCode => OpCode.AtomicStore;
        public ID Pointer;
        public ExecutionScope Scope;
        public MemorySemantics Semantics;
        public ID Value;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + Pointer + ", " + Scope + ", " + Semantics + ", " + Value + ')';
    }
}
