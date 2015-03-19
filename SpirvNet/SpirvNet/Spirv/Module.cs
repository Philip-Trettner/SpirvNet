using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpirvNet.Spirv
{
    /// <summary>
    /// A SPIR-V module
    /// </summary>
    public class Module
    {
        /// <summary>
        /// SPIR-V Magic Number
        /// </summary>
        public const uint MagicNumber = 0x07230203;

        /// <summary>
        /// Version number. The first public version will be 100 (use 99 for pre-release).
        /// </summary>
        public const uint VersionNumber = 99;

        /// <summary>
        /// Generator’s magic number. It is associated with the tool that
        /// generated the module.Its value does not effect any semantics, and
        /// is allowed to be 0. Using a non-0 value is encouraged, and can be
        /// registered with Khronos.
        /// </summary>
        public const uint Generator = 42;

        /// <summary>
        /// Bound; where all IDs in this module are guaranteed to satisfy 0 &lt; bound &lt; Bound
        /// </summary>
        public uint Bound = 0;

        /// <summary>
        /// 0 (Reserved for instruction schema, if needed.)
        /// </summary>
        public uint InstructionSchema = 0;

        /// <summary>
        /// All instructions of this module
        /// </summary>
        public readonly List<Instruction> Instructions = new List<Instruction>();

        /// <summary>
        /// Generates the bytecode for this module
        /// </summary>
        public List<uint> GenerateBytecode()
        {
            var code = new List<uint>
            {
                MagicNumber,
                VersionNumber,
                Bound,
                InstructionSchema
            };
            foreach (var instruction in Instructions)
                instruction.Generate(code);
            return code;
        }
    }
}
