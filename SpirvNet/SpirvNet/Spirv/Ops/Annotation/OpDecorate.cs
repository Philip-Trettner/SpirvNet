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
    /// OpDecorate
    /// 
    /// Add a decoration to another &lt;id&gt;.
    /// 
    /// Target is the &lt;id&gt; to decorate.  It can potentially be any &lt;id&gt; that is a forward reference. A set of decorations can be grouped together by having multiple OpDecorate instructions target the same OpDecorationGroup instruction.
    /// </summary>
    public sealed class OpDecorate : AnnotationInstruction
    {
        public override bool IsAnnotation => true;
        public override OpCode OpCode => OpCode.Decorate;

        public ID Target;
        public Decoration Decoration;
        public LiteralNumber[] ExtraOperands = { };

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Target) + ", " + StrOf(Decoration) + ", " + StrOf(ExtraOperands) + ")";
        public override string ArgString => "Target: " + StrOf(Target) + ", " + "Decoration: " + StrOf(Decoration) + ", " + "ExtraOperands: " + StrOf(ExtraOperands);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Decorate);
            var i = start + 1;
            Target = new ID(codes[i++]);
            Decoration = (Decoration)codes[i++];
            var length = WordCount - (i - start);
            ExtraOperands = new LiteralNumber[length];
            for (var k = 0; k < length; ++k)
                ExtraOperands[k] = new LiteralNumber(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Target.Value);
            code.Add((uint)Decoration);
            if (ExtraOperands != null)
                foreach (var val in ExtraOperands)
                    code.Add(val.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Target;
            }
        }
        #endregion
    }
}
