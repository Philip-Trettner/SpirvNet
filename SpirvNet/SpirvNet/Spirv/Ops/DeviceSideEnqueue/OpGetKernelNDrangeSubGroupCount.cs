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
    /// OpGetKernelNDrangeSubGroupCount
    /// 
    /// Returns the number of subgroups in each workgroup of the dispatch (except for the last in cases where the global size does not divide cleanly into work-groups) given the combination of the passed NDRange descriptor specified by ND Range and the function specified by Invoke.
    /// 
    /// ND Range must be a OpTypeStruct created by OpBuildNDRange.
    /// 
    /// Invoke must be a OpTypeFunction with the following signature:
    /// - Result Type must be OpTypeVoid.
    /// - The first parameter must be OpTypePointer to 8 bits OpTypeInt.
    /// - Optional list of parameters that must be OpTypePointer with WorkgroupLocal storage class.
    /// 
    /// Result Type must be a 32 bit OpTypeInt.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpGetKernelNDrangeSubGroupCount : DeviceSideEnqueueInstruction
    {
        public override bool IsDeviceSideEnqueue => true;
        public override OpCode OpCode => OpCode.GetKernelNDrangeSubGroupCount;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID NDRange;
        public ID Invoke;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(NDRange) + ", " + StrOf(Invoke) + ")";
        public override string ArgString => "NDRange: " + StrOf(NDRange) + ", " + "Invoke: " + StrOf(Invoke);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.GetKernelNDrangeSubGroupCount);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            NDRange = new ID(codes[i++]);
            Invoke = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(NDRange.Value);
            code.Add(Invoke.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return NDRange;
                yield return Invoke;
            }
        }
        #endregion
    }
}
