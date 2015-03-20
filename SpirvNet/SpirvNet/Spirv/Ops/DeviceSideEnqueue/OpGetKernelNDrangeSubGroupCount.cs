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
    public sealed class OpGetKernelNDrangeSubGroupCount : Instruction
    {
        public override bool IsDeviceSideEnqueue => true;
        public override OpCode OpCode => OpCode.GetKernelNDrangeSubGroupCount;
        public ID ResultType;
        public ID Result;
        public ID NDRange;
        public ID Invoke;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + ResultType + ", " + Result + ", " + NDRange + ", " + Invoke + ')';
    }
}
