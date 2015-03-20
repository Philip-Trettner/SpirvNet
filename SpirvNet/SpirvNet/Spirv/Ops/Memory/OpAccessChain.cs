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
    public sealed class OpAccessChain : Instruction
    {
        public override bool IsMemory => true;
        public override OpCode OpCode => OpCode.AccessChain;
        public ID ResultType;
        public ID Result;
        public ID Base;
        public ID[] Indexes;
    }
}
