using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;
using SpirvNet.Spirv.Ops;

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
            /// Ctor info
            /// </summary>
            public ConstructorInfo Ctor;
        }

        /// <summary>
        /// Mapping of cached layouts
        /// </summary>
        private static Dictionary<Type, LayoutInfo> cachedLayouts;
        /// <summary>
        /// Mapping of cached ops
        /// </summary>
        private static Dictionary<OpCode, LayoutInfo> cachedOps;

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
            // generate bytecode
            var cc = code.Count;

            code.Add(0); // dummy
            WriteCode(code);

            WordCount = (uint)(code.Count - cc);
            code[cc] = InstructionCode; // real val
        }

        /// <summary>
        /// Calculates the highest used ID
        /// </summary>
        public virtual IEnumerable<ID> AllIDs => new ID[] { };

        /// <summary>
        /// Generates and caches layout info on demand
        /// </summary>
        private static void GenerateAndCacheInfo()
        {
            if (cachedLayouts != null) return;

            // generate info
            cachedLayouts = new Dictionary<Type, LayoutInfo>();
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
                if (type.IsSubclassOf(typeof(Instruction)) && !type.IsAbstract)
                {
                    // generate info
                    var info = new LayoutInfo();

                    // default ctor
                    var ctor = type.GetConstructor(new Type[] { });
                    if (ctor != null)
                    {
                        info.Ctor = ctor;
                        var obj = ctor.Invoke(null) as Instruction;
                        if (obj == null)
                            throw new InvalidOperationException("Strange ctor");
                        cachedOps.Add(obj.OpCode, info);
                    }

                    cachedLayouts.Add(type, info);
                }
        }

        /// <summary>
        /// Fills this instruction from codes
        /// </summary>
        public abstract void FromCode(uint[] codes, int start);
        /// <summary>
        /// Adds code to the given array
        /// </summary>
        public abstract void WriteCode(List<uint> code);

        /// <summary>
        /// Reads an instruction from a buffer
        /// </summary>
        public static Instruction Read(uint[] codes, ref int ptr)
        {
            GenerateAndCacheInfo();

            var icode = codes[ptr];
            var opcode = (OpCode)(icode & 0x0000FFFF);
            var wc = icode >> 16;
            if (ptr + wc >= codes.Length)
                throw new FormatException("End of codes");

            if (cachedOps.ContainsKey(opcode))
            {
                var info = cachedOps[opcode];

                var op = info.Ctor.Invoke(null) as Instruction;
                if (op == null)
                    throw new FormatException("Malfunctioning ctor of " + opcode);

                op.WordCount = wc;
                op.FromCode(codes, ptr);

                ptr += (int)wc;
                return op;
            }
            else // OpUnknown
            {
                var op = new OpUnknown(opcode) { WordCount = wc };
                op.FromCode(codes, ptr);
                ptr += (int)wc;
                return op;
            }
        }
    }
}
