using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

namespace SpirvNet.Spirv.Ops.Debug
{
    /// <summary>
    /// Add source-level location information. This has no semantic impact and can safely be removed from a module.
    /// Target is the Result ID to locate. It can be the Result ID of any other instruction; a variable, function, type,
    /// intermediate result, etc.
    /// File is the ID from an OpString instruction and is the source-level file name.
    /// Line is the source-level line number.
    /// Column is the source-level column number
    /// </summary>
    public sealed class OpLine : Instruction
    {
        public override OpCode OpCode => OpCode.Line;
        public ID Target;
        public ID File;
        public LiteralNumber Line;
        public LiteralNumber Column;
    }
}