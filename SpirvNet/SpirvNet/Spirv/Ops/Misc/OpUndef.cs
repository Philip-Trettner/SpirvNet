using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

namespace SpirvNet.Spirv.Ops.Misc
{
    /// <summary>
    /// Make an intermediate object with no initialization.
    /// Result Type is the type of object to make
    /// </summary>
    public sealed class OpUndef : Instruction
    {
        public override OpCode OpCode => OpCode.Undef;
        public ID ResultType;
        public ID Result;
    }
}
