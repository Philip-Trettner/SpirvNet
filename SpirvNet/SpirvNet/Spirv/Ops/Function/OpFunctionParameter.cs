using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Function
{
    /// <summary>
    /// OpFunctionParameter
    /// 
    /// Declare the &lt;id&gt; for a formal parameter belonging to the current function.
    /// 
    /// This instruction must immediately follow an OpFunction or OpFunctionParameter instruction. The order of contiguous OpFunctionParameter instructions is the same order arguments will be listed in an OpFunctionCall instruction to this function. It is also the same order in which Parameter Type operands are listed in the OpTypeFunction of the Function Type operand for this function&#8217;s OpFunction instruction.
    /// 
    /// Result Type for all the OpFunctionParameter instructions for a function must be the same as, in order, the Parameter Type operands listed in the OpTypeFunction of the Function Type operand for this function&#8217;s OpFunction instruction.
    /// </summary>
    public sealed class OpFunctionParameter : FunctionInstruction
    {
        public override bool IsFunction => true;
        public override OpCode OpCode => OpCode.FunctionParameter;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ")";
        public override string ArgString => "";

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.FunctionParameter);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
            }
        }
        #endregion
    }
}
