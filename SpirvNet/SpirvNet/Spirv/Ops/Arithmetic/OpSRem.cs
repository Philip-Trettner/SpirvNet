using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Arithmetic
{
    /// <summary>
    /// OpSRem
    /// 
    /// Signed remainder operation of Operand 1 divided by Operand 2.  The sign of a non-0 result comes from Operand 1. The operands' types and Result Type must  all be scalars or vectors of integer        types with the same number of components and the same component widths. Works with any mixture of signedness. The resulting value is undefined if Operand 2 is 0.
    /// </summary>
    public sealed class OpSRem : ArithmeticInstruction
    {
        public override bool IsArithmetic => true;
        public override OpCode OpCode => OpCode.SRem;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Operand1;
        public ID Operand2;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Operand1) + ", " + StrOf(Operand2) + ")";
        public override string ArgString => "Operand1: " + StrOf(Operand1) + ", " + "Operand2: " + StrOf(Operand2);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.SRem);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Operand1 = new ID(codes[i++]);
            Operand2 = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Operand1.Value);
            code.Add(Operand2.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Operand1;
                yield return Operand2;
            }
        }
        #endregion
    }
}
