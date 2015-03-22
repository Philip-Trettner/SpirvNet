using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

namespace SpirvNet.Spirv.Ops
{
    /// <summary>
    /// Default class for unrecognized instructions
    /// </summary>
    public sealed class OpUnknown : Instruction
    {
        /// <summary>
        /// List of arguments (could be result or operand)
        /// </summary>
        public readonly List<uint> Args = new List<uint>();

        /// <summary>
        /// Explicit op code
        /// </summary>
        public override OpCode OpCode { get; }

        public override string ArgString => Args.Count == 0 ? "\"\"" : Args.Select(u => u.ToString("X8")).Aggregate((s1, s2) => s1 + ", " + s2);

        public OpUnknown(OpCode opCode)
        {
            OpCode = opCode;
        }

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode);
            for (var i = 1; i < WordCount; ++i)
                Args.Add(codes[start + i]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.AddRange(Args);
        }
    }
}
