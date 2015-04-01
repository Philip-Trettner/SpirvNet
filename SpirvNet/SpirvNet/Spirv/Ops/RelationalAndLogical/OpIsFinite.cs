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
    /// OpIsFinite
    /// 
    /// Result is true if x is an IEEE finite number, otherwise result is false.
    /// 
    ///  Result Type must be a scalar or vector of Boolean type, with the same number of components as the operand. Results are computed per component. The operand&#8217;s type and Result Type must     have the same number of components.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpIsFinite : RelationalAndLogicalInstruction
    {
        public override bool IsRelationalAndLogical => true;
        public override OpCode OpCode => OpCode.IsFinite;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID X;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(X) + ")";
        public override string ArgString => "X: " + StrOf(X);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.IsFinite);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            X = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(X.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return X;
            }
        }
        #endregion
    }
}
