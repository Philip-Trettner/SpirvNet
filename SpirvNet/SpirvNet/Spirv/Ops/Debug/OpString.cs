using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Debug
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    public sealed class OpString : Instruction
    {
        public override bool IsDebug => true;
        public override OpCode OpCode => OpCode.String;

        public ID Result;
        public LiteralString Name;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + Result + ", " + Name + ')';

        public override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.String);
            var i = 1;
            Result = new ID(codes[start + i++]);
            Name = LiteralString.FromCode(codes, ref i);
        }

        public override void WriteCode(List<uint> code)
        {
            code.Add(Result.Value);
            Name.WriteCode(code);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Result;
            }
        }
    }
}
