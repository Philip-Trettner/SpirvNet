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
    /// OpFNegate
    /// 
    /// Floating-point subtract of Operand from zero. The operand&#8217;s type and Result Type must both be scalars or vectors of floating-point types with the same number of components and the same component widths.
    /// </summary>
    public sealed class OpFNegate : ArithmeticInstruction
    {
        public override bool IsArithmetic => true;
        public override OpCode OpCode => OpCode.FNegate;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Operand;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Operand) + ")";
        public override string ArgString => "Operand: " + StrOf(Operand);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.FNegate);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Operand = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Operand.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Operand;
            }
        }
        #endregion
    }
}
