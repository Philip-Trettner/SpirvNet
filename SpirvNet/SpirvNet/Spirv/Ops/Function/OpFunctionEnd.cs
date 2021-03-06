using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Function
{
    /// <summary>
    /// OpFunctionEnd
    /// 
    /// Last instruction of a function definition.
    /// </summary>
    public sealed class OpFunctionEnd : FunctionInstruction
    {
        public override bool IsFunction => true;
        public override OpCode OpCode => OpCode.FunctionEnd;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ")";
        public override string ArgString => "";

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.FunctionEnd);
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
