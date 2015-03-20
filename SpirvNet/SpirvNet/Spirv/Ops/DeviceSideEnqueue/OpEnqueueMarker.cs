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
    public sealed class OpEnqueueMarker : Instruction
    {
        public override bool IsDeviceSideEnqueue => true;
        public override OpCode OpCode => OpCode.EnqueueMarker;
        public ID ResultType;
        public ID Result;
        public ID q;
        public ID NumEvents;
        public ID WaitEvents;
        public ID RetEvent;
    }
}