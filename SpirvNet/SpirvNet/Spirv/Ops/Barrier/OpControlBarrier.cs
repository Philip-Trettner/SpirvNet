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
    /// OpControlBarrier
    /// 
    /// Wait for other invocations of this module to reach this same point of execution.
    /// 
    /// All invocations of this module within Scope must reach this point of execution before any will proceed beyond it.
    /// 
    /// This instruction is only guaranteed to work correctly if placed strictly within dynamically uniform control flow within Scope. This ensures that if any invocation executes it, all invocations will execute it. If placed elsewhere, an invocation may stall indefinitely.
    /// 
    /// It is only valid to use this instruction with Tessellation, Compute, or Kernel execution models.
    /// </summary>
    public sealed class OpControlBarrier : BarrierInstruction
    {
        public override bool IsBarrier => true;
        public override OpCode OpCode => OpCode.ControlBarrier;

        public ExecutionScope Scope;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Scope) + ")";
        public override string ArgString => "Scope: " + StrOf(Scope);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.ControlBarrier);
            var i = start + 1;
            Scope = (ExecutionScope)codes[i++];
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add((uint)Scope);
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
