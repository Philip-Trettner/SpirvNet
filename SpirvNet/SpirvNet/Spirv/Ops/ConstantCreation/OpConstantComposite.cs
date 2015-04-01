using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.ConstantCreation
{
    /// <summary>
    /// OpConstantComposite
    /// 
    /// Declare a new composite constant.
    /// 
    /// Constituents will become members of a structure, or elements of an array, or components of a vector, or columns of a matrix. There must be exactly one Constituent for each top-level member/element/component/column of the result. The Constituents must appear in the order needed by the definition of the type of the result. The Constituents must be the &lt;id&gt; of other constant declarations.
    /// 
    /// Result Type must be a composite type, whose top-level members/elements/components/columns have the same type as the types of the operands.
    /// </summary>
    public sealed class OpConstantComposite : ConstantCreationInstruction
    {
        public override bool IsConstantCreation => true;
        public override OpCode OpCode => OpCode.ConstantComposite;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID[] Constituents = { };

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Constituents) + ")";
        public override string ArgString => "Constituents: " + StrOf(Constituents);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.ConstantComposite);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            var length = WordCount - (i - start);
            Constituents = new ID[length];
            for (var k = 0; k < length; ++k)
                Constituents[k] = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            if (Constituents != null)
                foreach (var val in Constituents)
                    code.Add(val.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                if (Constituents != null)
                    foreach (var id in Constituents)
                        yield return id;
            }
        }
        #endregion
    }
}
