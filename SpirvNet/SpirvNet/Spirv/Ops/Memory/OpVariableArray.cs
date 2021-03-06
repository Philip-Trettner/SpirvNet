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
    /// OpVariableArray
    /// 
    /// Allocate N objects sequentially in memory, resulting in a pointer to the first such object.
    /// 
    /// Storage Class is the kind of memory holding the object.
    /// 
    /// N is the number of objects to allocate.
    /// 
    /// Result Type is a type from OpTypePointer whose type pointed to is the type of one of the N objects allocated in memory.
    /// 
    /// Note: This is not the same thing as allocating a single object that is an array.
    /// </summary>
    [DependsOn(LanguageCapability.Addr)]
    public sealed class OpVariableArray : MemoryInstruction
    {
        public override bool IsMemory => true;
        public override OpCode OpCode => OpCode.VariableArray;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public StorageClass StorageClass;
        public ID N;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(StorageClass) + ", " + StrOf(N) + ")";
        public override string ArgString => "StorageClass: " + StrOf(StorageClass) + ", " + "N: " + StrOf(N);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.VariableArray);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            StorageClass = (StorageClass)codes[i++];
            N = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add((uint)StorageClass);
            code.Add(N.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return N;
            }
        }
        #endregion
    }
}
