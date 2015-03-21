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
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    public sealed class OpSource : Instruction
    {
        public override bool IsDebug => true;
        public override OpCode OpCode => OpCode.Source;

        public SourceLanguage SourceLanguage;
        public LiteralNumber Version;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + SourceLanguage + ", " + Version + ')';

        public override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Source);
            var i = 1;
            SourceLanguage = (SourceLanguage)codes[start + i++];
            Version = new LiteralNumber(codes[start + i++]);
        }

        public override void WriteCode(List<uint> code)
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
    }
}
