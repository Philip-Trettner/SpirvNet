using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Extension
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    public sealed class OpExtension : Instruction
    {
        public override bool IsExtension => true;
        public override OpCode OpCode => OpCode.Extension;

        public LiteralString Name;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + Name + ')';

        public override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Extension);
            var i = 1;
            Name = LiteralString.FromCode(codes, ref i);
        }

        public override void WriteCode(List<uint> code)
        {
            Name.WriteCode(code);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield break;
            }
        }
    }
}
