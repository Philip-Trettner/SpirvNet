using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Atomic
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    public sealed class OpAtomicStore : Instruction
    {
        public override bool IsAtomic => true;
        public override OpCode OpCode => OpCode.AtomicStore;

        public ID Pointer;
        public ExecutionScope Scope;
        public MemorySemantics Semantics;
        public ID Value;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Pointer) + ", " + StrOf(Scope) + ", " + StrOf(Semantics) + ", " + StrOf(Value) + ")";

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.AtomicStore);
            var i = 1;
            Pointer = new ID(codes[start + i++]);
            Scope = (ExecutionScope)codes[start + i++];
            Semantics = (MemorySemantics)codes[start + i++];
            Value = new ID(codes[start + i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Pointer.Value);
            code.Add((uint)Scope);
            code.Add((uint)Semantics);
            code.Add(Value.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Pointer;
                yield return Value;
            }
        }
        #endregion
    }
}
