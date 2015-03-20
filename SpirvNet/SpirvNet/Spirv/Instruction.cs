using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

namespace SpirvNet.Spirv
{
    /// <summary>
    /// A SPIR-V Instruction
    /// </summary>
    public abstract class Instruction
    {
        /// <summary>
        /// Cached information for instruction layout
        /// </summary>
        private class LayoutInfo
        {
            public uint WordCount;
        }

        /// <summary>
        /// Mapping of cached layouts
        /// </summary>
        private static readonly Dictionary<Type, LayoutInfo> CachedLayouts = new Dictionary<Type, LayoutInfo>(); 

        /// <summary>
        /// Opcode: The 16 high-order bits are the WordCount of the
        /// instruction.The 16 low-order bits are the opcode enumerant.
        /// </summary>
        public uint InstructionCode => (uint)OpCode + (WordCount << 16);

        /// <summary>
        /// Returns the number of words in this instruction
        /// Only valid after first Generate call
        /// </summary>
        public uint WordCount { get; protected set; } = 0;

        /// <summary>
        /// OpCode
        /// </summary>
        public abstract OpCode OpCode { get; }
        
        /// <summary>
        /// Adds the instruction bytecode to the given list
        /// Code can be null (only updates word count)
        /// </summary>
        public virtual void Generate(List<uint> code)
        {
            // get type info
            var t = GetType();

            LayoutInfo info;
            if (!CachedLayouts.TryGetValue(t, out info))
            {
                // generate info
                info = new LayoutInfo();

                CachedLayouts.Add(t, info);
            }

            WordCount = info.WordCount;

            // generate bytecode
            if (code == null)
                return;
            var cc = code.Count;

            code.Add(InstructionCode);
            // TODO

            Debug.Assert(WordCount == code.Count - cc);
        }
    }
}
