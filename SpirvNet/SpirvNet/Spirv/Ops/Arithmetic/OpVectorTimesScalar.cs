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
    /// OpVectorTimesScalar
    /// 
    /// Scale a floating-point vector.
    /// 
    /// Vector must have a floating-point vector type.
    /// 
    /// Scalar must be a floating-point scalar.
    /// 
    /// Result Type must be the same as the type of Vector.
    /// </summary>
    public sealed class OpVectorTimesScalar : ArithmeticInstruction
    {
        public override bool IsArithmetic => true;
        public override OpCode OpCode => OpCode.VectorTimesScalar;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Vector;
        public ID Scalar;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Vector) + ", " + StrOf(Scalar) + ")";
        public override string ArgString => "Vector: " + StrOf(Vector) + ", " + "Scalar: " + StrOf(Scalar);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.VectorTimesScalar);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Vector = new ID(codes[i++]);
            Scalar = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Vector.Value);
            code.Add(Scalar.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Vector;
                yield return Scalar;
            }
        }
        #endregion
    }
}
