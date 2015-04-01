using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Composite
{
    /// <summary>
    /// OpTranspose
    /// 
    /// Transpose a matrix.
    /// 
    /// Matrix must be an intermediate &lt;id&gt; whose type comes from an OpTypeMatrix instruction.
    /// 
    /// Result Type must be an &lt;id&gt; from an OpTypeMatrix instruction, where the number of columns and the column size is the reverse of those of the type of Matrix.
    /// </summary>
    [DependsOn(LanguageCapability.Matrix)]
    public sealed class OpTranspose : CompositeInstruction
    {
        public override bool IsComposite => true;
        public override OpCode OpCode => OpCode.Transpose;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Matrix;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Matrix) + ")";
        public override string ArgString => "Matrix: " + StrOf(Matrix);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Transpose);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Matrix = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Matrix.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Matrix;
            }
        }
        #endregion
    }
}
