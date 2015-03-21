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
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    public sealed class OpMemberDecorate : Instruction
    {
        public override bool IsAnnotation => true;
        public override OpCode OpCode => OpCode.MemberDecorate;

        public ID StructureType;
        public LiteralNumber Member;
        public Decoration Decoration;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(StructureType) + ", " + StrOf(Member) + ", " + StrOf(Decoration) + ")";

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.MemberDecorate);
            var i = start + 1;
            StructureType = new ID(codes[i++]);
            Member = new LiteralNumber(codes[i++]);
            Decoration = (Decoration)codes[i++];
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(StructureType.Value);
            code.Add(Member.Value);
            code.Add((uint)Decoration);
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
