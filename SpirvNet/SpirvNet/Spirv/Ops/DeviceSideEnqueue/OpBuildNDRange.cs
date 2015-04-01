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
    /// OpBuildNDRange
    /// 
    /// Given the global work size specified by GlobalWorkSize, local work size specified by LocalWorkSize and global work offset specified by GlobalWorkOffset, builds a 1D, 2D or 3D ND-range descriptor structure.
    /// 
    /// GlobalWorkSize, LocalWorkSize and GlobalWorkOffset must be a scalar or an array with 2 or 3 components. Where the type of each element in the array is 32 bit OpTypeInt when the Addressing Model is Physical32 or 64 bit OpTypeInt when the Addressing Model is Physical64.
    /// 
    /// Result Type is the descriptor and must be a OpTypeStruct with the following ordered list of members, starting from the first to last:
    /// - 32 bit OpTypeInt that specifies the number of dimensions used to specify the global work-items and work-items in the work-group. 
    /// - OpTypeArray with 3 elements, where each element is 32 bit OpTypeInt when the Addressing Model is Physical32 and 64 bit OpTypeInt when the Addressing Model is Physical64. This member is an array of per-dimension unsigned values that describe the offset used to calculate the global ID of a work-item.
    /// - OpTypeArray with 3 elements, where each element is 32 bit OpTypeInt when the Addressing Model is Physical32 and 64 bit OpTypeInt when the Addressing Model is Physical64. This member is an array of per-dimension unsigned values that describe the number of global work-items in the dimensions that will execute the kernel function.
    /// - OpTypeArray with 3 elements, where each element is 32 bit OpTypeInt when the Addressing Model is Physical32 and 64 bit OpTypeInt when the Addressing Model is Physical64. This member is an array of an array of per-dimension unsigned values that describe the number of work-items that make up a work-group.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpBuildNDRange : DeviceSideEnqueueInstruction
    {
        public override bool IsDeviceSideEnqueue => true;
        public override OpCode OpCode => OpCode.BuildNDRange;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID GlobalWorkSize;
        public ID LocalWorkSize;
        public ID GlobalWorkOffset;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(GlobalWorkSize) + ", " + StrOf(LocalWorkSize) + ", " + StrOf(GlobalWorkOffset) + ")";
        public override string ArgString => "GlobalWorkSize: " + StrOf(GlobalWorkSize) + ", " + "LocalWorkSize: " + StrOf(LocalWorkSize) + ", " + "GlobalWorkOffset: " + StrOf(GlobalWorkOffset);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.BuildNDRange);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            GlobalWorkSize = new ID(codes[i++]);
            LocalWorkSize = new ID(codes[i++]);
            GlobalWorkOffset = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(GlobalWorkSize.Value);
            code.Add(LocalWorkSize.Value);
            code.Add(GlobalWorkOffset.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return GlobalWorkSize;
                yield return LocalWorkSize;
                yield return GlobalWorkOffset;
            }
        }
        #endregion
    }
}
