using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Memory
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    public sealed class OpStore : Instruction
    {
        public override bool IsMemory => true;
        public override OpCode OpCode => OpCode.Store;

        public ID Pointer;
        public ID Object;
        public MemoryAccess[] MemoryAccess = new MemoryAccess[] { };

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + Pointer + ", " + Object + ", " + MemoryAccess + ')';

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Store);
            var i = 1;
            Pointer = new ID(codes[start + i++]);
            Object = new ID(codes[start + i++]);
            var length = WordCount - i + 1;
            MemoryAccess = new MemoryAccess[length];
            for (var k = 0; k < length; ++k)
                MemoryAccess[k] = (MemoryAccess)codes[start + i++];
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Pointer.Value);
            code.Add(Object.Value);
            foreach (var val in MemoryAccess)
                code.Add((uint)val);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Pointer;
                yield return Object;
            }
        }
    }
}
