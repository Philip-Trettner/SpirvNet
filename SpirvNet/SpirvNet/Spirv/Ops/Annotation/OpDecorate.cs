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
    public sealed class OpDecorate : AnnotationInstruction
    {
        public override bool IsAnnotation => true;
        public override OpCode OpCode => OpCode.Decorate;

        public ID Target;
        public Decoration Decoration;
        public LiteralNumber[] Args = { };

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Target) + ", " + StrOf(Decoration) + ", " + StrOf(Args) + ")";
        public override string ArgString => "Target: " + StrOf(Target) + ", " + "Decoration: " + StrOf(Decoration) + ", " + "Args: " + StrOf(Args);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Decorate);
            var i = start + 1;
            Target = new ID(codes[i++]);
            Decoration = (Decoration)codes[i++];
            var length = WordCount - (i - start);
            Args = new LiteralNumber[length];
            for (var k = 0; k < length; ++k)
                Args[k] = new LiteralNumber(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Target.Value);
            code.Add((uint)Decoration);
            if (Args != null)
                foreach (var val in Args)
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
