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
    /// OpLoad
    /// 
    /// Load through a pointer.
    /// 
    /// Pointer is the pointer to load through.  It must have a type of OpTypePointer whose operand is the same as Result Type.
    /// 
    /// Memory Access must be a Memory Access literal.  See Memory Access for more detail.
    /// </summary>
    public sealed class OpLoad : MemoryInstruction
    {
        public override bool IsMemory => true;
        public override OpCode OpCode => OpCode.Load;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Pointer;
        public LiteralNumber[] MemoryAccess = { };

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Pointer) + ", " + StrOf(MemoryAccess) + ")";
        public override string ArgString => "Pointer: " + StrOf(Pointer) + ", " + "MemoryAccess: " + StrOf(MemoryAccess);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Load);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Pointer = new ID(codes[i++]);
            var length = WordCount - (i - start);
            MemoryAccess = new LiteralNumber[length];
            for (var k = 0; k < length; ++k)
                MemoryAccess[k] = new LiteralNumber(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Pointer.Value);
            if (MemoryAccess != null)
                foreach (var val in MemoryAccess)
                    code.Add(val.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Pointer;
            }
        }
        #endregion
    }
}
