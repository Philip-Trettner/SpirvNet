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
    public sealed class OpBuildNDRange : Instruction
    {
        public override bool IsDeviceSideEnqueue => true;
        public override OpCode OpCode => OpCode.BuildNDRange;

        public ID ResultType;
        public ID Result;
        public ID GlobalWorkSize;
        public ID LocalWorkSize;
        public ID GlobalWorkOffset;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + ResultType + ", " + Result + ", " + GlobalWorkSize + ", " + LocalWorkSize + ", " + GlobalWorkOffset + ')';

        public override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.BuildNDRange);
            var i = 1;
            ResultType = new ID(codes[start + i++]);
            Result = new ID(codes[start + i++]);
            GlobalWorkSize = new ID(codes[start + i++]);
            LocalWorkSize = new ID(codes[start + i++]);
            GlobalWorkOffset = new ID(codes[start + i++]);
        }

        public override void WriteCode(List<uint> code)
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
    }
}
