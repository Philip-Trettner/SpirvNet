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
    /// OpBranch
    /// 
    /// Unconditional branch to Target Label.
    /// 
    /// Target Label must be the Result &lt;id&gt; of an OpLabel instruction in the current function.
    /// 
    /// This instruction must be the last instruction in a block.
    /// </summary>
    public sealed class OpBranch : FlowControlInstruction
    {
        public override bool IsFlowControl => true;
        public override OpCode OpCode => OpCode.Branch;

        public ID TargetLabel;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(TargetLabel) + ")";
        public override string ArgString => "TargetLabel: " + StrOf(TargetLabel);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Branch);
            var i = start + 1;
            TargetLabel = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(TargetLabel.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return TargetLabel;
            }
        }
        #endregion
    }
}
