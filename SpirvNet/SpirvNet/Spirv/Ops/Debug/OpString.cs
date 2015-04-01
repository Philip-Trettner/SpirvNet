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
    /// OpString
    /// 
    /// Name a string for use with other debug instructions (see OpLine). This has no semantic impact and can safely be removed from a module.
    /// 
    /// String is the literal string being assigned a Result &lt;id&gt;. It has no result type and no storage.
    /// </summary>
    public sealed class OpString : DebugInstruction
    {
        public override bool IsDebug => true;
        public override OpCode OpCode => OpCode.String;
        public override ID? ResultID => Result;

        public ID Result;
        public LiteralString String;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Result) + ", " + StrOf(String) + ")";
        public override string ArgString => "String: " + StrOf(String);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.String);
            var i = start + 1;
            Result = new ID(codes[i++]);
            String = LiteralString.FromCode(codes, ref i);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Result.Value);
            String.WriteCode(code);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Result;
            }
        }
        #endregion
    }
}
