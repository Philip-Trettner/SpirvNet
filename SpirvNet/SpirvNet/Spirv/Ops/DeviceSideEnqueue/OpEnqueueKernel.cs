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
    public sealed class OpEnqueueKernel : Instruction
    {
        public override bool IsDeviceSideEnqueue => true;
        public override OpCode OpCode => OpCode.EnqueueKernel;
        public ID ResultType;
        public ID Result;
        public ID q;
        public KernelEnqueueFlags Flags;
        public ID NDRange;
        public ID NumEvents;
        public ID WaitEvents;
        public ID RetEvent;
        public ID Invoke;
        public ID Param;
        public ID ParamSize;
        public ID ParamAlign;
        public ID[] LocalSize;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + ResultType + ", " + Result + ", " + q + ", " + Flags + ", " + NDRange + ", " + NumEvents + ", " + WaitEvents + ", " + RetEvent + ", " + Invoke + ", " + Param + ", " + ParamSize + ", " + ParamAlign + ", " + LocalSize + ')';
    }
}
