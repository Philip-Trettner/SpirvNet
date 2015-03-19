using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

namespace SpirvNet.Spirv.Ops
{
    public class OpNop : Instruction
    {
        public OpNop() : base(OpCode.Nop, 0)
        {
        }
    }
}
