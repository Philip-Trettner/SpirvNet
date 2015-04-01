using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.FlowControl
{
    /// <summary>
    /// OpUnreachable
    /// 
    /// Declares that this block is not reachable in the CFG.
    /// 
    /// This instruction must be the last instruction in a block.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpUnreachable : FlowControlInstruction
    {
        public override bool IsFlowControl => true;
        public override OpCode OpCode => OpCode.Unreachable;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ")";
        public override string ArgString => "";

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Unreachable);
        }

        protected override void WriteCode(List<uint> code)
        {
            // no-op
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
