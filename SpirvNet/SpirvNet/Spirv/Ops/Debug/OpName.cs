using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

namespace SpirvNet.Spirv.Ops.Debug
{
    /// <summary>
    /// Name a Result ID. This has no semantic impact and can safely be removed from a module.
    /// Target is the Result ID to name. It can be the Result ID of any other instruction; a variable, function, type,
    /// intermediate result, etc.
    /// Name is the string to name ID with
    /// </summary>
    public sealed class OpName : Instruction
    {
        public override OpCode OpCode => OpCode.Name;
        public ID Target;
        public LiteralString Name;
    }
}
