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
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    [DependsOn(LanguageCapability.Matrix)]
    public sealed class OpMatrixTimesScalar : Instruction
    {
        public override bool IsArithmetic => true;
        public override OpCode OpCode => OpCode.MatrixTimesScalar;

        public ID ResultType;
        public ID Result;
        public ID Matrix;
        public ID Scalar;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + ResultType + ", " + Result + ", " + Matrix + ", " + Scalar + ')';

        public override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.MatrixTimesScalar);
            var i = 1;
            ResultType = new ID(codes[start + i++]);
            Result = new ID(codes[start + i++]);
            Matrix = new ID(codes[start + i++]);
            Scalar = new ID(codes[start + i++]);
        }

        public override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Matrix.Value);
            code.Add(Scalar.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Matrix;
                yield return Scalar;
            }
        }
    }
}
