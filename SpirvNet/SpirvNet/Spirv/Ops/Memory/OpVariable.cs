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
    /// OpVariable
    /// 
    /// Allocate an object in memory, resulting in a pointer to it, which can be used with OpLoad and OpStore.
    /// 
    /// Storage Class is the kind of memory holding the object.
    /// 
    /// Initializer is optional.  If Initializer is present, it will be the initial value of the variable&#8217;s memory content. Initializer must be an &lt;id&gt; from a constant instruction. Initializer must have the same type as the type pointed to by Result Type.
    /// 
    /// Result Type is a type from OpTypePointer, where the type pointed to is the type of object in memory.
    /// </summary>
    public sealed class OpVariable : MemoryInstruction
    {
        public override bool IsMemory => true;
        public override OpCode OpCode => OpCode.Variable;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public StorageClass StorageClass;
        public ID? Initializer;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(StorageClass) + ", " + StrOf(Initializer) + ")";
        public override string ArgString => "StorageClass: " + StrOf(StorageClass) + ", " + "Initializer: " + StrOf(Initializer);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Variable);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            StorageClass = (StorageClass)codes[i++];
            if (i - start < WordCount)
                Initializer = new ID(codes[i++]);
            else
                Initializer = null;
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add((uint)StorageClass);
            if (Initializer.HasValue)
                code.Add(Initializer.Value.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                if (Initializer.HasValue)
                    yield return Initializer.Value;
            }
        }
        #endregion
    }
}
