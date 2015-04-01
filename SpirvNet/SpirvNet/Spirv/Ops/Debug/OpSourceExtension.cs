using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Debug
{
    /// <summary>
    /// OpSourceExtension
    /// 
    /// Document an extension to the source language. This has no semantic impact and can safely be removed from a module.
    /// 
    /// Extension is a string describing a source-language extension. Its form is dependent on the how the source language describes extensions.
    /// </summary>
    public sealed class OpSourceExtension : DebugInstruction
    {
        public override bool IsDebug => true;
        public override OpCode OpCode => OpCode.SourceExtension;

        public LiteralString Extension;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Extension) + ")";
        public override string ArgString => "Extension: " + StrOf(Extension);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.SourceExtension);
            var i = start + 1;
            Extension = LiteralString.FromCode(codes, ref i);
        }

        protected override void WriteCode(List<uint> code)
        {
            Extension.WriteCode(code);
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
