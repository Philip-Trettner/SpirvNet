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

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Target) + ", " + StrOf(File) + ", " + StrOf(Line) + ", " + StrOf(Column) + ")";

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Line);
            var i = start + 1;
            Target = new ID(codes[i++]);
            File = new ID(codes[i++]);
            Line = new LiteralNumber(codes[i++]);
            Column = new LiteralNumber(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
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
        #endregion
    }
}
