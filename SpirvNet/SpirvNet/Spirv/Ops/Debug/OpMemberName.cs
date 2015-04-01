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
    /// OpMemberName
    /// 
    /// Name a member of a structure type. This has no semantic impact and can safely be removed from a module.
    /// 
    /// Type is the &lt;id&gt; from an OpTypeStruct instruction.
    /// 
    /// Member is the number of the member to name in the structure. The first member is member 0, the next is member 1, &#8230;
    /// 
    /// Name is the string to name the member with.
    /// </summary>
    public sealed class OpMemberName : DebugInstruction
    {
        public override bool IsDebug => true;
        public override OpCode OpCode => OpCode.MemberName;

        public ID Type;
        public LiteralNumber Member;
        public LiteralString Name;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Type) + ", " + StrOf(Member) + ", " + StrOf(Name) + ")";
        public override string ArgString => "Type: " + StrOf(Type) + ", " + "Member: " + StrOf(Member) + ", " + "Name: " + StrOf(Name);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.MemberName);
            var i = start + 1;
            Type = new ID(codes[i++]);
            Member = new LiteralNumber(codes[i++]);
            Name = LiteralString.FromCode(codes, ref i);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Type.Value);
            code.Add(Member.Value);
            Name.WriteCode(code);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Type;
            }
        }
        #endregion
    }
}
