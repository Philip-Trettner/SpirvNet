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
    /// OpMatrixTimesVector
    /// 
    /// Linear-algebraic Vector X Matrix.
    /// 
    /// Matrix must have a floating-point matrix type.
    /// 
    /// Vector must have a floating-point vector type.
    /// 
    /// Result Type must be a vector whose size is the number of rows in the matrix.
    /// </summary>
    [DependsOn(LanguageCapability.Matrix)]
    public sealed class OpMatrixTimesVector : ArithmeticInstruction
    {
        public override bool IsArithmetic => true;
        public override OpCode OpCode => OpCode.MatrixTimesVector;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Matrix;
        public ID Vector;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Matrix) + ", " + StrOf(Vector) + ")";
        public override string ArgString => "Matrix: " + StrOf(Matrix) + ", " + "Vector: " + StrOf(Vector);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.MatrixTimesVector);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Matrix = new ID(codes[i++]);
            Vector = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Matrix.Value);
            code.Add(Vector.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Matrix;
                yield return Vector;
            }
        }
        #endregion
    }
}
