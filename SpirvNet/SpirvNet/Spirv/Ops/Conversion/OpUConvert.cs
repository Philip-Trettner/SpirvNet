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
    /// OpUConvert
    /// 
    /// Convert (value preserving) the width of Unsigned value.  This is either a truncate or a zero extend.
    /// 
    ///  Results are computed per component. The operand&#8217;s type and Result Type must     have the same number of components. The widths of the components of the operand and the Result Type must be different. Result Type cannot be a signed integer type.
    /// </summary>
    public sealed class OpUConvert : ConversionInstruction
    {
        public override bool IsConversion => true;
        public override OpCode OpCode => OpCode.UConvert;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID UnsignedValue;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(UnsignedValue) + ")";
        public override string ArgString => "UnsignedValue: " + StrOf(UnsignedValue);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.UConvert);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            UnsignedValue = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(UnsignedValue.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return UnsignedValue;
            }
        }
        #endregion
    }
}
