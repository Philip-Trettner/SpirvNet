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
    /// OpExtInstImport
    /// 
    /// Import an extended set of instructions. It can be later referenced by the Result &lt;id&gt;.
    /// 
    /// Name is the extended instruction-set&#8217;s name string.
    /// 
    /// See Extended Instruction Sets for more information.
    /// </summary>
    public sealed class OpExtInstImport : ExtensionInstruction
    {
        public override bool IsExtension => true;
        public override OpCode OpCode => OpCode.ExtInstImport;
        public override ID? ResultID => Result;

        public ID Result;
        public LiteralString Name;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Result) + ", " + StrOf(Name) + ")";
        public override string ArgString => "Name: " + StrOf(Name);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.ExtInstImport);
            var i = start + 1;
            Result = new ID(codes[i++]);
            Name = LiteralString.FromCode(codes, ref i);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Result.Value);
            Name.WriteCode(code);
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
