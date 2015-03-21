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
    public sealed class OpCopyMemory : Instruction
    {
        public override bool IsMemory => true;
        public override OpCode OpCode => OpCode.CopyMemory;

        public ID Target;
        public ID Source;
        public MemoryAccess[] MemoryAccess = new MemoryAccess[] { };

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + Target + ", " + Source + ", " + MemoryAccess + ')';

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.CopyMemory);
            var i = 1;
            Target = new ID(codes[start + i++]);
            Source = new ID(codes[start + i++]);
            var length = WordCount - i + 1;
            MemoryAccess = new MemoryAccess[length];
            for (var k = 0; k < length; ++k)
                MemoryAccess[k] = (MemoryAccess)codes[start + i++];
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Target.Value);
            code.Add(Source.Value);
            foreach (var val in MemoryAccess)
                code.Add((uint)val);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Target;
                yield return Source;
            }
        }
    }
}
