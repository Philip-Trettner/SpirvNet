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

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.MemoryBarrier);
            var i = 1;
            Scope = (ExecutionScope)codes[start + i++];
            Semantics = (MemorySemantics)codes[start + i++];
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add((uint)Scope);
            code.Add((uint)Semantics);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield break;
            }
        }
    }
}
