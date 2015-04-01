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
    /// OpCopyMemory
    /// 
    /// Copy from the memory pointed to by Source to the memory pointed to by Target. Both operands must be non-void pointers of the same type.  Matching storage class is not required. The amount of memory copied is the size of the type pointed to.
    /// 
    /// Memory Access must be a Memory Access literal.  See Memory Access for more detail.
    /// </summary>
    public sealed class OpCopyMemory : MemoryInstruction
    {
        public override bool IsMemory => true;
        public override OpCode OpCode => OpCode.CopyMemory;

        public ID Target;
        public ID Source;
        public LiteralNumber[] MemoryAccess = { };

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Target) + ", " + StrOf(Source) + ", " + StrOf(MemoryAccess) + ")";
        public override string ArgString => "Target: " + StrOf(Target) + ", " + "Source: " + StrOf(Source) + ", " + "MemoryAccess: " + StrOf(MemoryAccess);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.CopyMemory);
            var i = start + 1;
            Target = new ID(codes[i++]);
            Source = new ID(codes[i++]);
            var length = WordCount - (i - start);
            MemoryAccess = new LiteralNumber[length];
            for (var k = 0; k < length; ++k)
                MemoryAccess[k] = new LiteralNumber(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Target.Value);
            code.Add(Source.Value);
            if (MemoryAccess != null)
                foreach (var val in MemoryAccess)
                    code.Add(val.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Target;
                yield return Source;
            }
        }
        #endregion
    }
}
