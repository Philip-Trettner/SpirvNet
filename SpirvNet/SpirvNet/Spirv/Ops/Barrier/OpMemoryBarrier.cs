using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Barrier
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    public sealed class OpMemoryBarrier : Instruction
    {
        public override bool IsBarrier => true;
        public override OpCode OpCode => OpCode.MemoryBarrier;
        public ExecutionScope Scope;
        public MemorySemantics Semantics;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + Scope + ", " + Semantics + ')';
    }
}
