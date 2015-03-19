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
        /// Opcode: The 16 high-order bits are the WordCount of the
        /// instruction.The 16 low-order bits are the opcode enumerant.
        /// </summary>
        public readonly uint InstructionCode;

        /// <summary>
        /// Returns the number of words in this instruction
        /// </summary>
        public readonly uint WordCount;

        /// <summary>
        /// OpCode
        /// </summary>
        public readonly OpCode OpCode;

        /// <summary>
        /// Optional instruction type ID (presence determined by opcode, valid if > 0)
        /// </summary>
        public uint InstructionTypeID = 0;
        /// <summary>
        /// Optional instruction result ID (presence determined by opcode, valid if > 0).
        /// </summary>
        public uint InstructionResultID = 0;

        /// <summary>
        /// List of operand codes
        /// </summary>
        public readonly List<uint> Operands = new List<uint>();
        

        /// <summary>
        /// Ctor
        /// </summary>
        protected Instruction(OpCode opCode, uint wc)
        {
            OpCode = opCode;
            WordCount = wc;
            InstructionCode = (uint)opCode + (wc << 16);
        }

        /// <summary>
        /// Adds the instruction bytecode to the given list
        /// </summary>
        public void Generate(List<uint> code)
        {
            var cc = code.Count;

            code.Add(InstructionCode);
            if (InstructionTypeID > 0) code.Add(InstructionTypeID);
            if (InstructionResultID > 0) code.Add(InstructionResultID);
            code.AddRange(Operands);

            Debug.Assert(WordCount == code.Count - cc);
        }
    }
}
