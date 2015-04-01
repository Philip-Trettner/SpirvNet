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
    /// OpTypeInt
    /// 
    /// Declare a new integer type.
    /// 
    /// Width specifies how many bits wide the type is. The bit pattern of a signed integer value is two&#8217;s complement.
    /// 
    /// Signedness specifies whether there are signed semantics to preserve or validate.
    /// 0 indicates unsigned, or no signedness semantics
    /// 1 indicates signed semantics.
    /// In all cases, the type of operation of an instruction comes from the instruction&#8217;s opcode, not the signedness of the operands.
    /// 
    /// Result &lt;id&gt; is the &lt;id&gt; of the new integer type.
    /// </summary>
    public sealed class OpTypeInt : TypeDeclarationInstruction
    {
        public override bool IsTypeDeclaration => true;
        public override OpCode OpCode => OpCode.TypeInt;
        public override ID? ResultID => Result;

        public ID Result;
        public LiteralNumber Width;
        public LiteralNumber Signedness;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Result) + ", " + StrOf(Width) + ", " + StrOf(Signedness) + ")";
        public override string ArgString => "Width: " + StrOf(Width) + ", " + "Signedness: " + StrOf(Signedness);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TypeInt);
            var i = start + 1;
            Result = new ID(codes[i++]);
            Width = new LiteralNumber(codes[i++]);
            Signedness = new LiteralNumber(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Result.Value);
            code.Add(Width.Value);
            code.Add(Signedness.Value);
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
