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
    /// OpMemoryBarrier
    /// 
    /// Control the order that memory accesses are observed.
    /// 
    /// Ensures that memory accesses issued before this instruction will be observed before memory accesses issued after this instruction. This control is ensured only for memory accesses issued by this invocation and observed by another invocation executing within Scope.
    /// 
    /// Semantics declares what kind of memory is being controlled and what kind of control to apply.
    /// </summary>
    public sealed class OpMemoryBarrier : BarrierInstruction
    {
        public override bool IsBarrier => true;
        public override OpCode OpCode => OpCode.MemoryBarrier;

        public ExecutionScope Scope;
        public MemorySemantics Semantics;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Scope) + ", " + StrOf(Semantics) + ")";
        public override string ArgString => "Scope: " + StrOf(Scope) + ", " + "Semantics: " + StrOf(Semantics);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.MemoryBarrier);
            var i = start + 1;
            Scope = (ExecutionScope)codes[i++];
            Semantics = (MemorySemantics)codes[i++];
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
        #endregion
    }
}
