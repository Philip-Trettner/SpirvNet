using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.DeviceSideEnqueue
{
    /// <summary>
    /// OpGetKernelPreferredWorkGroupSizeMultiple
    /// 
    /// Returns the preferred multiple of work-group size for the function specified by Invoke. This is a performance hint. Specifying a work-group size that is not a multiple of the value returned by this query as the value of the local work size will not fail to enqueue Invoke for execution unless the work-group size specified is larger than the device maximum.
    /// 
    /// Invoke must be a OpTypeFunction with the following signature:
    /// - Result Type must be OpTypeVoid.
    /// - The first parameter must be OpTypePointer to 8 bits OpTypeInt.
    /// - Optional list of parameters that must be OpTypePointer with WorkgroupLocal storage class.
    /// 
    /// Result Type must be a 32 bit OpTypeInt.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpGetKernelPreferredWorkGroupSizeMultiple : DeviceSideEnqueueInstruction
    {
        public override bool IsDeviceSideEnqueue => true;
        public override OpCode OpCode => OpCode.GetKernelPreferredWorkGroupSizeMultiple;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Invoke;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Invoke) + ")";
        public override string ArgString => "Invoke: " + StrOf(Invoke);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.GetKernelPreferredWorkGroupSizeMultiple);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Invoke = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Invoke.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Invoke;
            }
        }
        #endregion
    }
}
