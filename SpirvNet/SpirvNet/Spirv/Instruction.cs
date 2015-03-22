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
        /// If non-null, this the result ID of the op
        /// </summary>Var
        public virtual ID? ResultID => null;
        /// <summary>
        /// If non-null, this the result type ID of the op
        /// </summary>
        public virtual ID? ResultTypeID => null;

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
        public void Generate(List<uint> code)
        {
            // generate bytecode
            var cc = code.Count;

            code.Add(0); // dummy
            WriteCode(code);

            WordCount = (uint)(code.Count - cc);
            code[cc] = InstructionCode; // real val
        }
        /// <summary>
        /// Returns the word array for this op
        /// </summary>
        public uint[] Generate()
        {
            var code = new List<uint>();
            Generate(code);
            return code.ToArray();
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
            cachedOps = new Dictionary<OpCode, LayoutInfo>();
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
        /// Generates a dummy instruction for every registered op code
        /// </summary>
        public static Dictionary<OpCode, Instruction> GenerateDummyInstructions()
        {
            GenerateAndCacheInfo();

            var result = new Dictionary<OpCode, Instruction>();

            foreach (var op in cachedOps)
            {
                var info = op.Value;
                var i = info.Ctor.Invoke(null) as Instruction;
                if (i != null)
                    result.Add(op.Key, i);
            }

            return result;
        }

        /// <summary>
        /// Fills this instruction from codes
        /// </summary>
        protected abstract void FromCode(uint[] codes, int start);
        /// <summary>
        /// Adds code to the given array
        /// </summary>
        protected abstract void WriteCode(List<uint> code);

        /// <summary>
        /// Reads an instruction from a buffer
        /// </summary>
        public static Instruction Read(uint[] codes)
        {
            var ptr = 0;
            return Read(codes, ref ptr);
        }
        /// <summary>
        /// Reads an instruction from a buffer
        /// </summary>
        public static Instruction Read(uint[] codes, ref int ptr)
        {
            GenerateAndCacheInfo();

            var icode = codes[ptr];
            var opcode = (OpCode)(icode & 0x0000FFFF);
            var wc = icode >> 16;
            if (wc <= 0)
                throw new FormatException("WordCount may not be zero");
            if (ptr + wc > codes.Length)
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

        protected static string StrOf(ID id) => id.ToString();
        protected static string StrOf(LiteralNumber nr) => nr.ToString();
        protected static string StrOf(LiteralString str) => str.ToString();
        protected static string StrOf(ID? id) => string.Format("<{0}>", id?.ToString() ?? "null");
        protected static string StrOf(ID[] ids) => string.Format("[{0}]", ids == null ? "null" : ids.Length == 0 ? " " : ids.Select(i => i.ToString()).Aggregate((s1, s2) => s1 + ", " + s2));
        protected static string StrOf(LiteralNumber[] nrs) => string.Format("[{0}]", nrs == null ? "null" : nrs.Length == 0 ? " " : nrs.Select(i => i.ToString()).Aggregate((s1, s2) => s1 + ", " + s2));
        protected static string StrOf<T>(T e) where T : struct => e.ToString();
        protected static string StrOf<T>(T[] es) => string.Format("[{0}]", es == null ? "null" : es.Length == 0 ? " " : es.Select(i => i.ToString()).Aggregate((s1, s2) => s1 + ", " + s2));
        protected static string StrOf<U, V>(Pair<U, V>[] p) => string.Format("[{0}]", p == null ? "null" : p.Length == 0 ? " " : p.Select(i => i.ToString()).Aggregate((s1, s2) => s1 + ", " + s2));

        /// <summary>
        /// Debug function to fill fiels with random values
        /// </summary>
        internal void FillWithRandomValues(Random random)
        {
            foreach (var field in GetType().GetFields())
            {
                var ftype = field.FieldType;

                if (ftype == typeof(ID))
                    field.SetValue(this, new ID((uint)random.Next()));

                else if (ftype == typeof(ID?))
                    field.SetValue(this, random.NextDouble() < 0.5 ? new ID((uint)random.Next()) : (ID?)null);

                else if (ftype == typeof(ID[]))
                    field.SetValue(this, random.Next(0, 4).ForUpTo(i => new ID((uint)random.Next())).ToArray());

                else if (ftype == typeof(LiteralNumber))
                    field.SetValue(this, new LiteralNumber((uint)random.Next()));

                else if (ftype == typeof(LiteralNumber[]))
                    field.SetValue(this, random.Next(0, 4).ForUpTo(i => new LiteralNumber((uint)random.Next())).ToArray());

                else if (ftype == typeof(LiteralString))
                    field.SetValue(this, new LiteralString { Value = (random.Next(0, 3) * random.Next(0, 7)).ForUpTo(i => random.Next(0, 10).ToString()).Aggregate("", (s1, s2) => s1 + s2) });

                else if (ftype.IsEnum)
                    field.SetValue(this, Enum.GetValues(ftype).RandomElement(random));

                else if (ftype.IsArray && ftype.GetElementType().IsEnum)
                {
                    var arraySize = random.Next(0, 4);
                    var array = ftype.GetConstructor(new[] { typeof(int) })?.Invoke(new object[] { arraySize }) as Array;
                    if (array == null)
                        throw new InvalidOperationException("array null");
                    for (var i = 0; i < arraySize; ++i)
                        array.SetValue(Enum.GetValues(ftype.GetElementType()).RandomElement(random), i);
                    field.SetValue(this, array);
                }

                else if (ftype == typeof(Pair<LiteralNumber, ID>[]))
                    field.SetValue(this, random.Next(0, 4).ForUpTo(i => new Pair<LiteralNumber, ID>(new LiteralNumber((uint)random.Next()), new ID((uint)random.Next()))).ToArray());

                else
                    throw new NotSupportedException("Unsupported field type: " + ftype + " of " + field);
            }
        }
    }
}
