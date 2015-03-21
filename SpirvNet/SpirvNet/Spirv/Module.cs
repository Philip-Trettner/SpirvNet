﻿using System;
using System.Collections.Generic;
using System.IO;
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
        public uint VersionNumber { get; private set; } = 99;

        /// <summary>
        /// Generator’s magic number. It is associated with the tool that
        /// generated the module.Its value does not effect any semantics, and
        /// is allowed to be 0. Using a non-0 value is encouraged, and can be
        /// registered with Khronos.
        /// </summary>
        public uint Generator { get; set; } = 42;

        /// <summary>
        /// Bound; where all IDs in this module are guaranteed to satisfy 0 &lt; bound &lt; Bound
        /// (Only valid after bytecode generation)
        /// </summary>
        public uint Bound { get; private set; } = 0;

        /// <summary>
        /// 0 (Reserved for instruction schema, if needed.)
        /// </summary>
        public uint InstructionSchema { get; private set; } = 0;

        /// <summary>
        /// All instructions of this module
        /// </summary>
        public readonly List<Instruction> Instructions = new List<Instruction>();

        /// <summary>
        /// Generates the bytecode for this module
        /// </summary>
        public List<uint> GenerateBytecode()
        {
            var maxID = 1u;
            foreach (var instruction in Instructions)
                foreach (var id in instruction.AllIDs)
                    if (id.Value > maxID)
                        maxID = id.Value;
            Bound = maxID + 1;

            var code = new List<uint>
            {
                MagicNumber,
                VersionNumber,
                Generator,
                Bound,
                InstructionSchema
            };
            foreach (var instruction in Instructions)
                instruction.Generate(code);
            return code;
        }

        /// <summary>
        /// Writes this module to a stream
        /// </summary>
        public void WriteToStream(Stream s)
        {
            var code = GenerateBytecode();
            foreach (var c in code)
            {
                var buffer = BitConverter.GetBytes(c);
                s.Write(buffer, 0, buffer.Length);
            }
        }

        /// <summary>
        /// Creates a module from file by name
        /// </summary>
        public static Module FromFile(string filename) => FromStream(new FileStream(filename, FileMode.Open));

        /// <summary>
        /// Creates a module from stream
        /// </summary>
        public static Module FromStream(Stream stream)
        {
            // read bytes
            var bytes = new List<byte>();
            var buffer = new byte[1024];
            do
            {
                var rc = stream.Read(buffer, 0, buffer.Length);

                if (rc <= 0)
                    break;

                for (var i = 0; i < rc; ++i)
                    bytes.Add(buffer[i]);

            } while (true);

            if (bytes.Count < 5 * sizeof(uint))
                throw new FormatException("Less than 5 words can");

            if (bytes.Count % sizeof(uint) != 0)
                throw new FormatException("bytecount not divisible by 4 (" + bytes.Count + ")");

            // convert to uint
            var bytearray = bytes.ToArray();
            var code = new uint[bytes.Count / sizeof(uint)];
            for (var i = 0; i < code.Length; ++i)
                code[i] = BitConverter.ToUInt32(bytearray, i * 4);

            return FromCode(code);
        }

        /// <summary>
        /// Creates a module from words
        /// </summary>
        public static Module FromCode(uint[] code)
        {
            // verify
            if (code[0] != MagicNumber)
                throw new FormatException("Magic number mismatch: " + code[0].ToString("X") + " vs " + MagicNumber.ToString("X"));

            // create module header
            var mod = new Module
            {
                VersionNumber = code[1],
                Generator = code[2],
                Bound = code[3],
                InstructionSchema = code[4]
            };

            // read instructions
            var ptr = 5;
            while (ptr < code.Length)
            {
                var instr = Instruction.Read(code, ref ptr);
                mod.Instructions.Add(instr);
            }

            return mod;
        }

        /// <summary>
        /// Returns a deep-copy of all modules
        /// </summary>
        public Module Clone() => FromCode(GenerateBytecode().ToArray());
    }
}
