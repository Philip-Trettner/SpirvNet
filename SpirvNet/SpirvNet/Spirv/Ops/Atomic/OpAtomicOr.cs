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
    /// OpAtomicOr
    /// 
    /// Perform the following steps atomically with respect to any other atomic accesses within Scope to the same location: 
    /// 1) load through Pointer to get an Original Value,
    /// 2) get a New Value by the bitwise OR of Original Value and Value, and
    /// 3) store the New Value back through Pointer.
    /// 
    /// The instruction&#8217;s result is the Original Value.
    /// 
    /// Result Type, the type of Value, and the type pointed to by Pointer must all be same type.
    /// </summary>
    public sealed class OpAtomicOr : AtomicInstruction
    {
        public override bool IsAtomic => true;
        public override OpCode OpCode => OpCode.AtomicOr;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Pointer;
        public ExecutionScope Scope;
        public MemorySemantics Semantics;
        public ID Value;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Pointer) + ", " + StrOf(Scope) + ", " + StrOf(Semantics) + ", " + StrOf(Value) + ")";
        public override string ArgString => "Pointer: " + StrOf(Pointer) + ", " + "Scope: " + StrOf(Scope) + ", " + "Semantics: " + StrOf(Semantics) + ", " + "Value: " + StrOf(Value);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.AtomicOr);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Pointer = new ID(codes[i++]);
            Scope = (ExecutionScope)codes[i++];
            Semantics = (MemorySemantics)codes[i++];
            Value = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Pointer.Value);
            code.Add((uint)Scope);
            code.Add((uint)Semantics);
            code.Add(Value.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Pointer;
                yield return Value;
            }
        }
        #endregion
    }
}
