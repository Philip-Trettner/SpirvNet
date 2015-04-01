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
    /// OpLogicalXor
    /// 
    /// Result is true if exactly one of Operand 1 or Operand 2 is true. Result is false if Operand 1 and Operand 2 have the same value.
    /// 
    ///  Operand 1 and Operand 2 must both be scalars or vectors of Boolean type.
    /// 
    ///  Result Type must be a scalar or vector of Boolean type, with the same number of components as the operands. Results are computed per component. The operands' types and Result Type must all have the same number of components.
    /// </summary>
    public sealed class OpLogicalXor : RelationalAndLogicalInstruction
    {
        public override bool IsRelationalAndLogical => true;
        public override OpCode OpCode => OpCode.LogicalXor;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Operand1;
        public ID Operand2;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Operand1) + ", " + StrOf(Operand2) + ")";
        public override string ArgString => "Operand1: " + StrOf(Operand1) + ", " + "Operand2: " + StrOf(Operand2);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.LogicalXor);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Operand1 = new ID(codes[i++]);
            Operand2 = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Operand1.Value);
            code.Add(Operand2.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Operand1;
                yield return Operand2;
            }
        }
        #endregion
    }
}
