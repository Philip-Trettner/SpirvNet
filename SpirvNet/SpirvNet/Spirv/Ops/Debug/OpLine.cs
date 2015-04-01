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
    /// OpLine
    /// 
    /// Add source-level location information. This has no semantic impact and can safely be removed from a module.
    /// 
    /// Target is the Result &lt;id&gt; to locate. It can be the Result &lt;id&gt; of any other instruction; a variable, function, type, intermediate result, etc.
    /// 
    /// File is the &lt;id&gt; from an OpString instruction and is the source-level file name.
    /// 
    /// Line is the source-level line number.
    /// 
    /// Column is the source-level column number.
    /// </summary>
    public sealed class OpLine : DebugInstruction
    {
        public override bool IsDebug => true;
        public override OpCode OpCode => OpCode.Line;

        public ID Target;
        public ID File;
        public LiteralNumber Line;
        public LiteralNumber Column;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Target) + ", " + StrOf(File) + ", " + StrOf(Line) + ", " + StrOf(Column) + ")";
        public override string ArgString => "Target: " + StrOf(Target) + ", " + "File: " + StrOf(File) + ", " + "Line: " + StrOf(Line) + ", " + "Column: " + StrOf(Column);

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
