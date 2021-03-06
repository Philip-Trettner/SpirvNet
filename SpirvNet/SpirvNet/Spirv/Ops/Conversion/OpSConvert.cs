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
    /// OpSConvert
    /// 
    /// Convert (value preserving) the width of Signed Value.  This is either a truncate or a sign extend.
    /// 
    ///  Results are computed per component. The operand&#8217;s type and Result Type must     have the same number of components. The widths of the components of the operand and the Result Type must be different.
    /// </summary>
    public sealed class OpSConvert : ConversionInstruction
    {
        public override bool IsConversion => true;
        public override OpCode OpCode => OpCode.SConvert;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID SignedValue;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(SignedValue) + ")";
        public override string ArgString => "SignedValue: " + StrOf(SignedValue);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.SConvert);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            SignedValue = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(SignedValue.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return SignedValue;
            }
        }
        #endregion
    }
}
