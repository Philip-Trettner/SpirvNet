using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Composite
{
    /// <summary>
    /// OpCompositeExtract
    /// 
    /// Extract a part of a composite object. 
    /// 
    /// Composite in the composite to extract from.
    /// 
    /// Indexes walk the type hierarchy, down to component granularity.  All indexes must be in bounds. 
    /// 
    /// Result Type must be the type of object selected by the last provided index.  The instruction result is the extracted object.
    /// </summary>
    public sealed class OpCompositeExtract : CompositeInstruction
    {
        public override bool IsComposite => true;
        public override OpCode OpCode => OpCode.CompositeExtract;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Composite;
        public LiteralNumber[] Indexes = { };

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Composite) + ", " + StrOf(Indexes) + ")";
        public override string ArgString => "Composite: " + StrOf(Composite) + ", " + "Indexes: " + StrOf(Indexes);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.CompositeExtract);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Composite = new ID(codes[i++]);
            var length = WordCount - (i - start);
            Indexes = new LiteralNumber[length];
            for (var k = 0; k < length; ++k)
                Indexes[k] = new LiteralNumber(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Composite.Value);
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
                yield return Composite;
            }
        }
        #endregion
    }
}
