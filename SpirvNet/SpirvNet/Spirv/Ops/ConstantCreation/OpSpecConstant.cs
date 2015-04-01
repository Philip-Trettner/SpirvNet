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
    /// OpSpecConstant
    /// 
    /// Declare a new Integer-type or Floating-point-type scalar specialization constant.
    /// 
    /// Value is the bit pattern for the default value of the constant. Types 32 bits wide or smaller take one word. Larger types take multiple words, with low-order words appearing first.
    /// 
    /// This instruction can be specialized to become an OpConstant instruction.
    /// 
    /// Result Type must be a scalar Integer type or Floating-point type.
    /// 
    /// See Specialization.
    /// </summary>
    [DependsOn(LanguageCapability.Shader)]
    public sealed class OpSpecConstant : ConstantCreationInstruction
    {
        public override bool IsConstantCreation => true;
        public override OpCode OpCode => OpCode.SpecConstant;
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
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.SpecConstant);
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
