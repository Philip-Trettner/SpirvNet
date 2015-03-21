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
    public sealed class OpAtomicCompareExchangeWeak : Instruction
    {
        public override bool IsAtomic => true;
        public override OpCode OpCode => OpCode.AtomicCompareExchangeWeak;

        public ID ResultType;
        public ID Result;
        public ID Pointer;
        public ExecutionScope Scope;
        public MemorySemantics Semantics;
        public ID Value;
        public ID Comparator;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + ResultType + ", " + Result + ", " + Pointer + ", " + Scope + ", " + Semantics + ", " + Value + ", " + Comparator + ')';

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.AtomicCompareExchangeWeak);
            var i = 1;
            ResultType = new ID(codes[start + i++]);
            Result = new ID(codes[start + i++]);
            Pointer = new ID(codes[start + i++]);
            Scope = (ExecutionScope)codes[start + i++];
            Semantics = (MemorySemantics)codes[start + i++];
            Value = new ID(codes[start + i++]);
            Comparator = new ID(codes[start + i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Pointer.Value);
            code.Add((uint)Scope);
            code.Add((uint)Semantics);
            code.Add(Value.Value);
            code.Add(Comparator.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Pointer;
                yield return Value;
                yield return Comparator;
            }
        }
    }
}
