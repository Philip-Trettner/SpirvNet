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
    /// OpBitcast
    /// 
    /// Bit-pattern preserving type conversion for Numerical-type or pointer-type vectors and scalars.
    /// 
    /// Operand is the bit pattern whose type will change.
    /// 
    /// Result Type must be different than the type of Operand.  Both Result Type and the type of Operand must be Numerical-types or pointer types. The components of Operand and Result Type must be same bit width. 
    /// 
    ///  Results are computed per component. The operand&#8217;s type and Result Type must     have the same number of components.
    /// </summary>
    public sealed class OpBitcast : ConversionInstruction
    {
        public override bool IsConversion => true;
        public override OpCode OpCode => OpCode.Bitcast;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Operand;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Operand) + ")";
        public override string ArgString => "Operand: " + StrOf(Operand);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Bitcast);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Operand = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Operand.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Operand;
            }
        }
        #endregion
    }
}
