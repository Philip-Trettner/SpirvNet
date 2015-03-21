﻿using System;
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

        public OpUnknown(OpCode opCode)
        {
            OpCode = opCode;
        }

        public override void Generate(List<uint> code)
        {
            WordCount = (uint)(1 + Args.Count);
            code.Add(InstructionCode);
            code.AddRange(Args);
        }

        public override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode);
            for (var i = 1; i < WordCount; ++i)
                Args.Add(codes[start + i]);
        }

        public override void WriteCode(List<uint> code)
        {
            code.AddRange(Args);
        }
    }
}
