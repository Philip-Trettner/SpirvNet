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
    /// OpSelect
    /// 
    /// Select between two objects. Results are computed per component.
    /// 
    /// Condition must be a Boolean type scalar or vector.
    /// 
    /// Object 1 is selected as the result if Condition is true.
    /// 
    /// Object 2 is selected as the result if Condition is false.
    /// 
    /// Result Type, the type of Object 1, and the type of Object 2 must all be the same. Condition must have the same number of components as the operands.
    /// </summary>
    public sealed class OpSelect : RelationalAndLogicalInstruction
    {
        public override bool IsRelationalAndLogical => true;
        public override OpCode OpCode => OpCode.Select;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Condition;
        public ID Object1;
        public ID Object2;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Condition) + ", " + StrOf(Object1) + ", " + StrOf(Object2) + ")";
        public override string ArgString => "Condition: " + StrOf(Condition) + ", " + "Object1: " + StrOf(Object1) + ", " + "Object2: " + StrOf(Object2);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Select);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Condition = new ID(codes[i++]);
            Object1 = new ID(codes[i++]);
            Object2 = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Condition.Value);
            code.Add(Object1.Value);
            code.Add(Object2.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Condition;
                yield return Object1;
                yield return Object2;
            }
        }
        #endregion
    }
}
