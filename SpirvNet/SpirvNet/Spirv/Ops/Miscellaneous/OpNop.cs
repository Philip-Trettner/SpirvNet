using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Miscellaneous
{
    /// <summary>
    /// OpNop
    /// 
    /// Use is invalid.
    /// </summary>
    public sealed class OpNop : MiscellaneousInstruction
    {
        public override bool IsMiscellaneous => true;
        public override OpCode OpCode => OpCode.Nop;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ")";
        public override string ArgString => "";

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Nop);
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
