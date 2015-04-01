using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.ConstantCreation
{
    /// <summary>
    /// OpConstant
    /// 
    /// Declare a new Integer-type or Floating-point-type scalar constant.
    /// 
    /// Value is the bit pattern for the constant. Types 32 bits wide or smaller take one word. Larger types take multiple words, with low-order words appearing first.
    /// 
    /// Result Type must be a scalar Integer type or Floating-point type.
    /// </summary>
    public sealed class OpConstant : ConstantCreationInstruction
    {
        public override bool IsConstantCreation => true;
        public override OpCode OpCode => OpCode.Constant;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public LiteralNumber[] Value = { };

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Value) + ")";
        public override string ArgString => "Value: " + StrOf(Value);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Constant);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            var length = WordCount - (i - start);
            Value = new LiteralNumber[length];
            for (var k = 0; k < length; ++k)
                Value[k] = new LiteralNumber(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            if (Value != null)
                foreach (var val in Value)
                    code.Add(val.Value);
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
