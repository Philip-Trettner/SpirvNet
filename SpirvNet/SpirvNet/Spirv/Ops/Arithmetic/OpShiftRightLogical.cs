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
    /// OpShiftRightLogical
    /// 
    /// Shift the bits in Operand 1 right by the number of bits specified in Operand 2.  The most-significant bits will be zero filled. Operand 2 is consumed as an unsigned integer. The result is undefined if Operand 2 is greater than the bit width of the components of Operand 1. 
    /// 
    /// The number of components and bit width of Result Type must match those of Operand 1 type. All types must be integer types. Works with any mixture of signedness.
    /// </summary>
    public sealed class OpShiftRightLogical : ArithmeticInstruction
    {
        public override bool IsArithmetic => true;
        public override OpCode OpCode => OpCode.ShiftRightLogical;
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
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.ShiftRightLogical);
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
