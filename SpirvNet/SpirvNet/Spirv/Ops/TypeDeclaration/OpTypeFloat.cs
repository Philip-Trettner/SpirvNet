using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.TypeDeclaration
{
    /// <summary>
    /// OpTypeFloat
    /// 
    /// Declare a new floating-point type.
    /// 
    /// Width specifies how many bits wide the type is. The bit pattern of a floating-point value is as described by the IEEE 754 standard.
    /// 
    /// Result &lt;id&gt; is the &lt;id&gt; of the new floating-point type.
    /// </summary>
    public sealed class OpTypeFloat : TypeDeclarationInstruction
    {
        public override bool IsTypeDeclaration => true;
        public override OpCode OpCode => OpCode.TypeFloat;
        public override ID? ResultID => Result;

        public ID Result;
        public LiteralNumber Width;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Result) + ", " + StrOf(Width) + ")";
        public override string ArgString => "Width: " + StrOf(Width);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TypeFloat);
            var i = start + 1;
            Result = new ID(codes[i++]);
            Width = new LiteralNumber(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Result.Value);
            code.Add(Width.Value);
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
