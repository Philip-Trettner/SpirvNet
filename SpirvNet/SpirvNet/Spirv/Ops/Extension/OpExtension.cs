using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Extension
{
    /// <summary>
    /// OpExtension
    /// 
    /// Declare use of an extension to SPIR-V. This allows validation of additional instructions, tokens, semantics, etc.
    /// 
    /// Name is the extension&#8217;s name string.
    /// </summary>
    public sealed class OpExtension : ExtensionInstruction
    {
        public override bool IsExtension => true;
        public override OpCode OpCode => OpCode.Extension;

        public LiteralString Name;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Name) + ")";
        public override string ArgString => "Name: " + StrOf(Name);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Extension);
            var i = start + 1;
            Name = LiteralString.FromCode(codes, ref i);
        }

        protected override void WriteCode(List<uint> code)
        {
            Name.WriteCode(code);
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
