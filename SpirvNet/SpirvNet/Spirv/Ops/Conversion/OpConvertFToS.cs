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
    /// OpConvertFToS
    /// 
    /// Convert (value preserving) Float Value from floating point to signed integer, with round toward 0.0.
    /// 
    ///  Results are computed per component. The operand&#8217;s type and Result Type must     have the same number of components.
    /// </summary>
    public sealed class OpConvertFToS : ConversionInstruction
    {
        public override bool IsConversion => true;
        public override OpCode OpCode => OpCode.ConvertFToS;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID FloatValue;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(FloatValue) + ")";
        public override string ArgString => "FloatValue: " + StrOf(FloatValue);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.ConvertFToS);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            FloatValue = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(FloatValue.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return FloatValue;
            }
        }
        #endregion
    }
}
