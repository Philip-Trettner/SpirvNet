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
    /// OpGroupDecorate
    /// 
    /// Add a group of decorations to another &lt;id&gt;.
    /// 
    /// Decoration group is the &lt;id&gt; of an OpDecorationGroup instruction.
    /// 
    /// Target, &#8230; are the target &lt;id&gt;s to decorate with the groups of decorations.
    /// </summary>
    public sealed class OpGroupDecorate : AnnotationInstruction
    {
        public override bool IsAnnotation => true;
        public override OpCode OpCode => OpCode.GroupDecorate;

        public ID DecorationGroup;
        public ID[] Targets = { };

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(DecorationGroup) + ", " + StrOf(Targets) + ")";
        public override string ArgString => "DecorationGroup: " + StrOf(DecorationGroup) + ", " + "Targets: " + StrOf(Targets);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.GroupDecorate);
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
