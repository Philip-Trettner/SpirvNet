using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Debug
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    public sealed class OpSourceExtension : Instruction
    {
        public override bool IsDebug => true;
        public override OpCode OpCode => OpCode.SourceExtension;
        public LiteralString Extension;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + Extension + ')';
    }
}
