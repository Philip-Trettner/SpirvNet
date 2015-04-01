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
    /// OpMatrixTimesMatrix
    /// 
    /// Linear-algebraic multiply of LeftMatrix X RightMatrix.
    /// 
    /// LeftMatrix and RightMatrix must both have a floating-point matrix type. The number of columns of LeftMatrix must equal the number of rows of RightMatrix.
    /// 
    /// Result Type must be a matrix whose number of columns is the number of columns in RightMatrix and whose number of rows is the number of rows of LeftMatrix.
    /// </summary>
    [DependsOn(LanguageCapability.Matrix)]
    public sealed class OpMatrixTimesMatrix : ArithmeticInstruction
    {
        public override bool IsArithmetic => true;
        public override OpCode OpCode => OpCode.MatrixTimesMatrix;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID LeftMatrix;
        public ID RightMatrix;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(LeftMatrix) + ", " + StrOf(RightMatrix) + ")";
        public override string ArgString => "LeftMatrix: " + StrOf(LeftMatrix) + ", " + "RightMatrix: " + StrOf(RightMatrix);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.MatrixTimesMatrix);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            LeftMatrix = new ID(codes[i++]);
            RightMatrix = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(LeftMatrix.Value);
            code.Add(RightMatrix.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return LeftMatrix;
                yield return RightMatrix;
            }
        }
        #endregion
    }
}
