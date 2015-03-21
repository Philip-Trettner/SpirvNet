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
    public sealed class OpGroupMemberDecorate : AnnotationInstruction
    {
        public override bool IsAnnotation => true;
        public override OpCode OpCode => OpCode.GroupMemberDecorate;

        public ID DecorationGroup;
        public ID[] Targets = { };

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(DecorationGroup) + ", " + StrOf(Targets) + ")";

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.GroupMemberDecorate);
            var i = start + 1;
            DecorationGroup = new ID(codes[i++]);
            var length = WordCount - (i - start);
            Targets = new ID[length];
            for (var k = 0; k < length; ++k)
                Targets[k] = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(DecorationGroup.Value);
            if (Targets != null)
                foreach (var val in Targets)
                    code.Add(val.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return DecorationGroup;
                if (Targets != null)
                    foreach (var id in Targets)
                        yield return id;
            }
        }
        #endregion
    }
}
