using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

namespace SpirvNet.Spirv.Ops.Annotation
{
    /// <summary>
    /// TODO
    /// </summary>
    public sealed class OpDecorationGroup : Instruction
    {
        public override OpCode OpCode => OpCode.DecorationGroup;
    }
}
