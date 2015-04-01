using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.RelationalAndLogical
{
    /// <summary>
    /// OpUnordered
    /// 
    /// Result is true if either x or y is an IEEE NaN, otherwise result is false.
    /// 
    ///  Result Type must be a scalar or vector of Boolean type, with the same number of components as the operands. Results are computed per component. The operands' types and Result Type must all have the same number of components.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpUnordered : RelationalAndLogicalInstruction
    {
        public override bool IsRelationalAndLogical => true;
        public override OpCode OpCode => OpCode.Unordered;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID X;
        public ID Y;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(X) + ", " + StrOf(Y) + ")";
        public override string ArgString => "X: " + StrOf(X) + ", " + "Y: " + StrOf(Y);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Unordered);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            X = new ID(codes[i++]);
            Y = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(X.Value);
            code.Add(Y.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return X;
                yield return Y;
            }
        }
        #endregion
    }
}
