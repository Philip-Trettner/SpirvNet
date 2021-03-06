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
    /// OpGenericPtrMemSemantics
    /// 
    /// Returns a valid Memory Semantics value for ptr. ptr must point to Generic. 
    /// 
    /// Result Type must be a 32-bits wide OpTypeInt value.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpGenericPtrMemSemantics : MemoryInstruction
    {
        public override bool IsMemory => true;
        public override OpCode OpCode => OpCode.GenericPtrMemSemantics;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Ptr;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Ptr) + ")";
        public override string ArgString => "Ptr: " + StrOf(Ptr);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.GenericPtrMemSemantics);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Ptr = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Ptr.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Ptr;
            }
        }
        #endregion
    }
}
