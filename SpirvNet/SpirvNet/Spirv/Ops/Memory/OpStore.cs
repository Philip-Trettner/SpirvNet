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
    /// OpStore
    /// 
    /// Store through a pointer.
    /// 
    /// Pointer is the pointer to store through.  It must have a type of OpTypePointer whose operand is the same as the type of Object.
    /// 
    /// Object is the object to store.
    /// 
    /// Memory Access must be a Memory Access literal.  See Memory Access for more detail.
    /// </summary>
    public sealed class OpStore : MemoryInstruction
    {
        public override bool IsMemory => true;
        public override OpCode OpCode => OpCode.Store;

        public ID Pointer;
        public ID Object;
        public LiteralNumber[] MemoryAccess = { };

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Pointer) + ", " + StrOf(Object) + ", " + StrOf(MemoryAccess) + ")";
        public override string ArgString => "Pointer: " + StrOf(Pointer) + ", " + "Object: " + StrOf(Object) + ", " + "MemoryAccess: " + StrOf(MemoryAccess);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Store);
            var i = start + 1;
            Pointer = new ID(codes[i++]);
            Object = new ID(codes[i++]);
            var length = WordCount - (i - start);
            MemoryAccess = new LiteralNumber[length];
            for (var k = 0; k < length; ++k)
                MemoryAccess[k] = new LiteralNumber(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Pointer.Value);
            code.Add(Object.Value);
            if (MemoryAccess != null)
                foreach (var val in MemoryAccess)
                    code.Add(val.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Pointer;
                yield return Object;
            }
        }
        #endregion
    }
}
