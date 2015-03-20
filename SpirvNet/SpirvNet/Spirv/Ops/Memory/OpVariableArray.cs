using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Memory
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    [DependsOn(LanguageCapability.Addr)]
    public sealed class OpVariableArray : Instruction
    {
        public override bool IsMemory => true;
        public override OpCode OpCode => OpCode.VariableArray;
        public ID ResultType;
        public ID Result;
        public StorageClass StorageClass;
        public ID N;
    }
}
