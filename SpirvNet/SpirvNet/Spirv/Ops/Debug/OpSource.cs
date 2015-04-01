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
    /// OpSource
    /// 
    /// Document what source language this module was translated from. This has no semantic impact and can safely be removed from a module.
    /// 
    /// Version is the version of the source language.
    /// </summary>
    public sealed class OpSource : DebugInstruction
    {
        public override bool IsDebug => true;
        public override OpCode OpCode => OpCode.Source;

        public SourceLanguage SourceLanguage;
        public LiteralNumber Version;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(SourceLanguage) + ", " + StrOf(Version) + ")";
        public override string ArgString => "SourceLanguage: " + StrOf(SourceLanguage) + ", " + "Version: " + StrOf(Version);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Source);
            var i = start + 1;
            SourceLanguage = (SourceLanguage)codes[i++];
            Version = new LiteralNumber(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add((uint)SourceLanguage);
            code.Add(Version.Value);
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
