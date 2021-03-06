using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Conversion
{
    /// <summary>
    /// OpConvertUToPtr
    /// 
    /// Converts Integer value to a pointer. A Result Type width smaller than the width of Integer value pointer will truncate. A Result Type width larger than the width of Integer value pointer will zero extend. For same-width source and target, this is the same as OpBitCast.
    /// </summary>
    [DependsOn(LanguageCapability.Addr)]
    public sealed class OpConvertUToPtr : ConversionInstruction
    {
        public override bool IsConversion => true;
        public override OpCode OpCode => OpCode.ConvertUToPtr;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID IntegerValue;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(IntegerValue) + ")";
        public override string ArgString => "IntegerValue: " + StrOf(IntegerValue);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.ConvertUToPtr);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            IntegerValue = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(IntegerValue.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return IntegerValue;
            }
        }
        #endregion
    }
}
