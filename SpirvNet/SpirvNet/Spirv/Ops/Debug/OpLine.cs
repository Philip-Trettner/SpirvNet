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
    public sealed class OpLine : Instruction
    {
        public override bool IsDebug => true;
        public override OpCode OpCode => OpCode.Line;

        public ID Target;
        public ID File;
        public LiteralNumber Line;
        public LiteralNumber Column;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + Target + ", " + File + ", " + Line + ", " + Column + ')';

        public override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Line);
            var i = 1;
            Target = new ID(codes[start + i++]);
            File = new ID(codes[start + i++]);
            Line = new LiteralNumber(codes[start + i++]);
            Column = new LiteralNumber(codes[start + i++]);
        }

        public override void WriteCode(List<uint> code)
        {
            code.Add(Target.Value);
            code.Add(File.Value);
            code.Add(Line.Value);
            code.Add(Column.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Target;
                yield return File;
            }
        }
    }
}
