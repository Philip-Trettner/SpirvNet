using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Primitive
{
    /// <summary>
    /// OpEndPrimitive
    /// 
    /// Finish the current primitive and start a new one.  No vertex is emitted.
    /// 
    /// This instruction can only be used when only one stream is present.
    /// </summary>
    [DependsOn(LanguageCapability.Geom)]
    public sealed class OpEndPrimitive : PrimitiveInstruction
    {
        public override bool IsPrimitive => true;
        public override OpCode OpCode => OpCode.EndPrimitive;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ")";
        public override string ArgString => "";

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.EndPrimitive);
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
