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
    /// OpAccessChain
    /// 
    /// Create a pointer into a composite object that can be used with OpLoad and OpStore. 
    /// 
    /// Base must be a pointer type, pointing to the base of the object. 
    /// 
    /// Indexes walk the type hierarchy to the desired depth, potentially down to scalar granularity. The type of the pointer created will be to the type reached by walking the type hierarchy down to the last provided index.
    /// 
    /// The storage class of the pointer created will be the same as the storage class of the base operand.
    /// </summary>
    public sealed class OpAccessChain : MemoryInstruction
    {
        public override bool IsMemory => true;
        public override OpCode OpCode => OpCode.AccessChain;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Base;
        public ID[] Indexes = { };

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Base) + ", " + StrOf(Indexes) + ")";
        public override string ArgString => "Base: " + StrOf(Base) + ", " + "Indexes: " + StrOf(Indexes);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.AccessChain);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Base = new ID(codes[i++]);
            var length = WordCount - (i - start);
            Indexes = new ID[length];
            for (var k = 0; k < length; ++k)
                Indexes[k] = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Base.Value);
            if (Indexes != null)
                foreach (var val in Indexes)
                    code.Add(val.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Base;
                if (Indexes != null)
                    foreach (var id in Indexes)
                        yield return id;
            }
        }
        #endregion
    }
}
