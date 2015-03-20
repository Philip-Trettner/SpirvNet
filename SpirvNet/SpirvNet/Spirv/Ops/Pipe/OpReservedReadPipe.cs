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
    public sealed class OpReservedReadPipe : Instruction
    {
        public override bool IsPipe => true;
        public override OpCode OpCode => OpCode.ReservedReadPipe;
        public ID ResultType;
        public ID Result;
        public ID P;
        public ID ReserveId;
        public ID Index;
        public ID Ptr;
    }
}
