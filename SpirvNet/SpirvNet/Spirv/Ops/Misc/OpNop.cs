using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

namespace SpirvNet.Spirv.Ops.Misc
{
    /// <summary>
    /// Use is invalid
    /// </summary>
    public sealed class OpNop : Instruction
    {
        public override OpCode OpCode => OpCode.Nop;
    }
}
