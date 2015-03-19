using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

namespace SpirvNet.Spirv
{
    /// <summary>
    /// Default class for unrecognized instructions
    /// </summary>
    public class UnrecognizedInstruction : Instruction
    {
        public UnrecognizedInstruction(OpCode opCode, uint wc) : base(opCode, wc)
        {
        }
    }
}
