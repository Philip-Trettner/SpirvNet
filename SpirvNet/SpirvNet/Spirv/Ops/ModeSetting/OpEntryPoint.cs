using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.ModeSetting
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    public sealed class OpEntryPoint : Instruction
    {
        public override bool IsModeSetting => true;
        public override OpCode OpCode => OpCode.EntryPoint;
        public ExecutionModel ExecutionModel;
        public ID EntryPoint;
    }
}
