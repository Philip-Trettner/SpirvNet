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
    /// OpDot
    /// 
    /// Dot product of Vector 1 and Vector 2.
    /// 
    /// The operands' types must be floating-point vectors with the same component type and the same number of components.
    /// 
    /// Result Type must be a scalar of the same type as the operands' component type.
    /// </summary>
    public sealed class OpDot : ArithmeticInstruction
    {
        public override bool IsArithmetic => true;
        public override OpCode OpCode => OpCode.Dot;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Vector1;
        public ID Vector2;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Vector1) + ", " + StrOf(Vector2) + ")";
        public override string ArgString => "Vector1: " + StrOf(Vector1) + ", " + "Vector2: " + StrOf(Vector2);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Dot);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Vector1 = new ID(codes[i++]);
            Vector2 = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Vector1.Value);
            code.Add(Vector2.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Vector1;
                yield return Vector2;
            }
        }
        #endregion
    }
}
