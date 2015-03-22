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
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
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
