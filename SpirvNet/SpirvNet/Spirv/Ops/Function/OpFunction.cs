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
    /// OpFunction
    /// 
    /// Define a function body.  This instruction must be immediately followed by one OpFunctionParameter instruction per each formal parameter of this function. This function&#8217;s body will terminate with the next OpFunctionEnd instruction.
    /// 
    /// Function Type is the result of an OpTypeFunction, which declares the types of the return value and parameters of the function.
    /// 
    /// Result Type must be the same as the Return Type declared in Function Type.
    /// </summary>
    public sealed class OpFunction : FunctionInstruction
    {
        public override bool IsFunction => true;
        public override OpCode OpCode => OpCode.Function;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public FunctionControlMask FunctionControlMask;
        public ID FunctionType;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(FunctionControlMask) + ", " + StrOf(FunctionType) + ")";
        public override string ArgString => "FunctionControlMask: " + StrOf(FunctionControlMask) + ", " + "FunctionType: " + StrOf(FunctionType);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Function);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            FunctionControlMask = (FunctionControlMask)codes[i++];
            FunctionType = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add((uint)FunctionControlMask);
            code.Add(FunctionType.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return FunctionType;
            }
        }
        #endregion
    }
}
