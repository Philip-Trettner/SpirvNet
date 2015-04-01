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
    /// OpAtomicLoad
    /// 
    /// Atomically load through Pointer using the given Semantics. All subparts of the value that is loaded will be read atomically with respect to all other atomic accesses to it within Scope.
    /// 
    /// Result Type must be the same type as the type pointed to by Pointer.
    /// </summary>
    public sealed class OpAtomicLoad : AtomicInstruction
    {
        public override bool IsAtomic => true;
        public override OpCode OpCode => OpCode.AtomicLoad;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Pointer;
        public ExecutionScope Scope;
        public MemorySemantics Semantics;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Pointer) + ", " + StrOf(Scope) + ", " + StrOf(Semantics) + ")";
        public override string ArgString => "Pointer: " + StrOf(Pointer) + ", " + "Scope: " + StrOf(Scope) + ", " + "Semantics: " + StrOf(Semantics);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.AtomicLoad);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Pointer = new ID(codes[i++]);
            Scope = (ExecutionScope)codes[i++];
            Semantics = (MemorySemantics)codes[i++];
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Pointer.Value);
            code.Add((uint)Scope);
            code.Add((uint)Semantics);
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
