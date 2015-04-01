using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Derivative
{
    /// <summary>
    /// OpDPdxCoarse
    /// 
    /// Result is the partial derivative of P with respect to the window x coordinate. Will use local differencing based on the value of P for the current fragment&#8217;s neighbors, and will possibly, but not necessarily, include the value of P for the current fragment. That is, over a given area, the implementation can compute x derivatives in fewer unique locations than would be allowed for OpDPdxFine.
    /// 
    /// P is the value to take the derivative of.
    /// 
    /// Result Type must be the same as the type of P. This type must be a floating-point scalar or floating-point vector.
    /// </summary>
    [DependsOn(LanguageCapability.Shader)]
    public sealed class OpDPdxCoarse : DerivativeInstruction
    {
        public override bool IsDerivative => true;
        public override OpCode OpCode => OpCode.DPdxCoarse;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID P;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(P) + ")";
        public override string ArgString => "P: " + StrOf(P);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.DPdxCoarse);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            P = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(P.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return P;
            }
        }
        #endregion
    }
}
