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
    /// OpFunctionCall
    /// 
    /// Call a function.
    /// 
    /// Function is the &lt;id&gt; of an OpFunction instruction.  This could be a forward reference.
    /// 
    /// Argument N is the &lt;id&gt; of the object to copy to parameter N of Function.
    /// 
    /// Result Type is the type of the return value of the function. 
    /// 
    /// Note: A forward call is possible because there is no missing type information: Result Type must match the Return Type of the function, and the calling argument types must match the formal parameter types.
    /// </summary>
    public sealed class OpFunctionCall : FunctionInstruction
    {
        public override bool IsFunction => true;
        public override OpCode OpCode => OpCode.FunctionCall;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Function;
        public ID[] Arguments = { };

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Function) + ", " + StrOf(Arguments) + ")";
        public override string ArgString => "Function: " + StrOf(Function) + ", " + "Arguments: " + StrOf(Arguments);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.FunctionCall);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Function = new ID(codes[i++]);
            var length = WordCount - (i - start);
            Arguments = new ID[length];
            for (var k = 0; k < length; ++k)
                Arguments[k] = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Function.Value);
            if (Arguments != null)
                foreach (var val in Arguments)
                    code.Add(val.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Function;
                if (Arguments != null)
                    foreach (var id in Arguments)
                        yield return id;
            }
        }
        #endregion
    }
}
