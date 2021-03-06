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
    /// OpEmitVertex
    /// 
    /// Emits the current values of all output variables to the current output primitive. After execution, the values of all output variables are undefined.
    /// 
    /// This instruction can only be used when only one stream is present.
    /// </summary>
    [DependsOn(LanguageCapability.Geom)]
    public sealed class OpEmitVertex : PrimitiveInstruction
    {
        public override bool IsPrimitive => true;
        public override OpCode OpCode => OpCode.EmitVertex;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ")";
        public override string ArgString => "";

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.EmitVertex);
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
