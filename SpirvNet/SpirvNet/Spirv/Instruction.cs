﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
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
            /// <summary>
            /// Word count
            /// </summary>
            public uint WordCount;

            /// <summary>
            /// Code from object -> add to code
            /// </summary>
            public List<Action<object, List<uint>>> Fields = new List<Action<object, List<uint>>>();
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
                info = new LayoutInfo { WordCount = 1 };
                foreach (var tfield in t.GetFields())
                {
                    var field = tfield; // extra var for closure capture

                    if (field.FieldType == typeof(ID))
                    {
                        info.WordCount += 1u;
                        info.Fields.Add((o, c) => c.Add(((ID)field.GetValue(o)).Value));
                    }
                    else if (field.FieldType == typeof(LiteralNumber))
                    {
                        info.WordCount += 1u;
                        info.Fields.Add((o, c) => c.Add(((LiteralNumber)field.GetValue(o)).Value));
                    }
                    else
                    {
                        throw new NotSupportedException("Unsupported field type for " + field);
                    }
                }

                CachedLayouts.Add(t, info);
            }

            WordCount = info.WordCount;

            // generate bytecode
            if (code == null)
                return;
            var cc = code.Count;

            code.Add(InstructionCode);
            foreach (var field in info.Fields)
                field(this, code);

            Debug.Assert(WordCount == code.Count - cc);
        }
    }
}
