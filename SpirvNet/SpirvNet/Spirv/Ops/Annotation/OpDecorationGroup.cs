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
    /// OpDecorationGroup
    /// 
    /// A collector of decorations from OpDecorate instructions. All such instructions must precede this instruction. Subsequent OpGroupDecorate and OpGroupMemberDecorate instructions can consume the Result &lt;id&gt; to apply multiple decorations to multiple target &lt;id&gt;s. Those are the only instructions allowed to consume the Result &lt;id&gt;.
    /// </summary>
    public sealed class OpDecorationGroup : AnnotationInstruction
    {
        public override bool IsAnnotation => true;
        public override OpCode OpCode => OpCode.DecorationGroup;
        public override ID? ResultID => Result;

        public ID Result;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Result) + ")";
        public override string ArgString => "";

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.DecorationGroup);
            var i = start + 1;
            Result = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Result.Value);
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
