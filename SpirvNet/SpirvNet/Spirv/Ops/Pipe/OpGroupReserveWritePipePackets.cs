using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Pipe
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpGroupReserveWritePipePackets : Instruction
    {
        public override bool IsPipe => true;
        public override OpCode OpCode => OpCode.GroupReserveWritePipePackets;
        public ID ResultType;
        public ID Result;
        public ExecutionScope Scope;
        public ID P;
        public ID NumPackets;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + ResultType + ", " + Result + ", " + Scope + ", " + P + ", " + NumPackets + ')';
    }
}
