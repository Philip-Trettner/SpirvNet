using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Group
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpAsyncGroupCopy : Instruction
    {
        public override bool IsGroup => true;
        public override OpCode OpCode => OpCode.AsyncGroupCopy;
        public ID ResultType;
        public ID Result;
        public ExecutionScope Scope;
        public ID Destination;
        public ID Source;
        public ID NumElements;
        public ID Stride;
        public ID Event;
    }
}
