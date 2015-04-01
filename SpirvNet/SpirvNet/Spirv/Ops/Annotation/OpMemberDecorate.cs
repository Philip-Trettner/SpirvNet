using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Annotation
{
    /// <summary>
    /// OpMemberDecorate
    /// 
    /// Add a decoration to a member of a structure type.
    /// 
    /// Structure type is the &lt;id&gt; of a type from OpTypeStruct.
    /// 
    /// Member is the number of the member to decorate in the structure. The first member is member 0, the next is member 1, &#8230;
    /// </summary>
    public sealed class OpMemberDecorate : AnnotationInstruction
    {
        public override bool IsAnnotation => true;
        public override OpCode OpCode => OpCode.MemberDecorate;

        public ID StructureType;
        public LiteralNumber Member;
        public Decoration Decoration;
        public LiteralNumber[] ExtraOperands = { };

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(StructureType) + ", " + StrOf(Member) + ", " + StrOf(Decoration) + ", " + StrOf(ExtraOperands) + ")";
        public override string ArgString => "StructureType: " + StrOf(StructureType) + ", " + "Member: " + StrOf(Member) + ", " + "Decoration: " + StrOf(Decoration) + ", " + "ExtraOperands: " + StrOf(ExtraOperands);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.MemberDecorate);
            var i = start + 1;
            StructureType = new ID(codes[i++]);
            Member = new LiteralNumber(codes[i++]);
            Decoration = (Decoration)codes[i++];
            var length = WordCount - (i - start);
            ExtraOperands = new LiteralNumber[length];
            for (var k = 0; k < length; ++k)
                ExtraOperands[k] = new LiteralNumber(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(StructureType.Value);
            code.Add(Member.Value);
            code.Add((uint)Decoration);
            if (ExtraOperands != null)
                foreach (var val in ExtraOperands)
                    code.Add(val.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return StructureType;
            }
        }
        #endregion
    }
}
