using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

namespace SpirvNet.Spirv.Ops.Debug
{
    /// <summary>
    /// Name a string for use with other debug instructions (see OpLine). This has no semantic impact and
    /// can safely be removed from a module.
    /// String is the literal string being assigned a Result<id>.It has no result type and no storage
    /// </summary>
    public sealed class OpString : Instruction
    {
        public override OpCode OpCode => OpCode.String;
        public ID Result;
        public LiteralString String;
    }
}