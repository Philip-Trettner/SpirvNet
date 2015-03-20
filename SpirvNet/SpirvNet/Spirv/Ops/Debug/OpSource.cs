using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

namespace SpirvNet.Spirv.Ops.Debug
{
    /// <summary>
    /// Document what source language this module was translated from. This has no
    /// semantic impact and can safely be removed from a module.
    /// Version is the version of the source language
    /// </summary>
    public sealed class OpSource : Instruction
    {
        public override OpCode OpCode => OpCode.Source;
        public SourceLanguage Language;
        public LiteralNumber Version;
    }
}
