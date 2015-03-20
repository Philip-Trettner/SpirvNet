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
        // Classificiation
        public virtual bool IsFunction => false;
        public virtual bool IsGroup => false;
        public virtual bool IsMemory => false;
        public virtual bool IsMisc => false;
        public virtual bool IsModeSetting => false;
        public virtual bool IsPipe => false;
        public virtual bool IsPrimitive => false;
        public virtual bool IsRelationalLogical => false;
        public virtual bool IsTexture => false;
        public virtual bool IsTypeDeclaration => false;
        public virtual bool IsAnnotation => false;
        public virtual bool IsArithmetic => false;
        public virtual bool IsAtomic => false;
        public virtual bool IsBarrier => false;
        public virtual bool IsComposite => false;
        public virtual bool IsConstantCreation => false;
        public virtual bool IsConversion => false;
        public virtual bool IsDebug => false;
        public virtual bool IsDerivative => false;
        public virtual bool IsDeviceSideEnqueue => false;
        public virtual bool IsExtension => false;
        public virtual bool IsFlowControl => false;

        /// <summary>
        /// Cached information for instruction layout
        /// </summary>
        private class LayoutInfo
        {
            /// <summary>
            /// Code from object -> add to code
            /// </summary>
            public readonly List<Action<object, List<uint>>> Fields = new List<Action<object, List<uint>>>();

            /// <summary>
            /// Code from object -> ID
            /// </summary>
            public readonly List<Func<object, ID>> IDs = new List<Func<object, ID>>();
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
        /// </summary>
        public virtual void Generate(List<uint> code)
        {
            // get type info
            var info = GenerateAndCacheInfo();

            // generate bytecode
            var cc = code.Count;

            code.Add(0); // dummy
            foreach (var field in info.Fields)
                field(this, code);

            WordCount = (uint)(code.Count - cc);
            code[cc] = InstructionCode; // real val
        }

        /// <summary>
        /// Generates and caches layout info on demand
        /// </summary>
        private LayoutInfo GenerateAndCacheInfo()
        {
            var t = GetType();

            LayoutInfo info;
            if (!CachedLayouts.TryGetValue(t, out info))
            {
                // generate info
                info = new LayoutInfo();
                foreach (var tfield in t.GetFields())
                {
                    var field = tfield; // extra var for closure capture

                    if (field.FieldType == typeof (ID))
                    {
                        info.IDs.Add(o => (ID) field.GetValue(o));
                        info.Fields.Add((o, c) => c.Add(((ID) field.GetValue(o)).Value));
                    }

                    else if (field.FieldType == typeof (LiteralNumber))
                        info.Fields.Add((o, c) => c.Add(((LiteralNumber) field.GetValue(o)).Value));

                    else if (field.FieldType == typeof (LiteralString))
                        info.Fields.Add((o, c) => ((LiteralString) field.GetValue(o)).Generate(c));

                    else
                        throw new NotSupportedException("Unsupported field type for " + field);
                }

                CachedLayouts.Add(t, info);
            }
            return info;
        }
    }
}
