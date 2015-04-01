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
    /// OpEndStreamPrimitive
    /// 
    /// Finish the current primitive and start a new one.  No vertex is emitted.
    /// 
    /// Stream must be an &lt;id&gt; of a constant instruction with a scalar integer type.  It is the stream the primitive is on.
    /// 
    /// This instruction can only be used when multiple streams are present.
    /// </summary>
    [DependsOn(LanguageCapability.Geom)]
    public sealed class OpEndStreamPrimitive : PrimitiveInstruction
    {
        public override bool IsPrimitive => true;
        public override OpCode OpCode => OpCode.EndStreamPrimitive;

        public ID Stream;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Stream) + ")";
        public override string ArgString => "Stream: " + StrOf(Stream);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.EndStreamPrimitive);
            var i = start + 1;
            Stream = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Stream.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Stream;
            }
        }
        #endregion
    }
}
