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
    public sealed class OpGroupMemberDecorate : Instruction
    {
        public override bool IsAnnotation => true;
        public override OpCode OpCode => OpCode.GroupMemberDecorate;

        public ID DecorationGroup;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(DecorationGroup) + ")";

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.GroupMemberDecorate);
            var i = 1;
            DecorationGroup = new ID(codes[start + i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(DecorationGroup.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return DecorationGroup;
            }
        }
        #endregion
    }
}
