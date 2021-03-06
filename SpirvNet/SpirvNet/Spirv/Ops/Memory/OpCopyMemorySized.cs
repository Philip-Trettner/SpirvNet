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
    /// OpCopyMemorySized
    /// 
    /// Copy from the memory pointed to by Source to the memory pointed to by Target. 
    /// 
    /// Size is the number of bytes to copy.
    /// 
    /// Memory Access must be a Memory Access literal.  See Memory Access for more detail.
    /// </summary>
    [DependsOn(LanguageCapability.Addr)]
    public sealed class OpCopyMemorySized : MemoryInstruction
    {
        public override bool IsMemory => true;
        public override OpCode OpCode => OpCode.CopyMemorySized;

        public ID Target;
        public ID Source;
        public ID Size;
        public LiteralNumber[] MemoryAccess = { };

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Target) + ", " + StrOf(Source) + ", " + StrOf(Size) + ", " + StrOf(MemoryAccess) + ")";
        public override string ArgString => "Target: " + StrOf(Target) + ", " + "Source: " + StrOf(Source) + ", " + "Size: " + StrOf(Size) + ", " + "MemoryAccess: " + StrOf(MemoryAccess);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.CopyMemorySized);
            var i = start + 1;
            Target = new ID(codes[i++]);
            Source = new ID(codes[i++]);
            Size = new ID(codes[i++]);
            var length = WordCount - (i - start);
            MemoryAccess = new LiteralNumber[length];
            for (var k = 0; k < length; ++k)
                MemoryAccess[k] = new LiteralNumber(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Target.Value);
            code.Add(Source.Value);
            code.Add(Size.Value);
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
                yield return Size;
            }
        }
        #endregion
    }
}
