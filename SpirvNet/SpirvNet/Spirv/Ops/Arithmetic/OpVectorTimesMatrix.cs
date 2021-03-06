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
    /// OpVectorTimesMatrix
    /// 
    /// Linear-algebraic Vector X Matrix.
    /// 
    /// Vector must have a floating-point vector type.
    /// 
    /// Matrix must have a floating-point matrix type.
    /// 
    /// Result Type must be a vector whose size is the number of columns in the matrix.
    /// </summary>
    [DependsOn(LanguageCapability.Matrix)]
    public sealed class OpVectorTimesMatrix : ArithmeticInstruction
    {
        public override bool IsArithmetic => true;
        public override OpCode OpCode => OpCode.VectorTimesMatrix;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Vector;
        public ID Matrix;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Vector) + ", " + StrOf(Matrix) + ")";
        public override string ArgString => "Vector: " + StrOf(Vector) + ", " + "Matrix: " + StrOf(Matrix);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.VectorTimesMatrix);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Vector = new ID(codes[i++]);
            Matrix = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Vector.Value);
            code.Add(Matrix.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Vector;
                yield return Matrix;
            }
        }
        #endregion
    }
}
