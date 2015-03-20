using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

namespace SpirvNet.Spirv.Ops.Debug
{
    /// <summary>
    /// Document an extension to the source language. This has no semantic impact and can safely be removed from a module.
    /// Extension is a string describing a source-language extension.
    /// Its form is dependent on the how the source language describes extensions
    /// </summary>
    public sealed class OpSourceExtension : Instruction
    {
        public override OpCode OpCode => OpCode.SourceExtension;
        public LiteralString Extension;
    }
}
