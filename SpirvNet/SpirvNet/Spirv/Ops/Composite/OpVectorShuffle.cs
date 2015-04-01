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
    /// OpVectorShuffle
    /// 
    /// Select arbitrary components from two vectors to make a new vector.
    /// 
    /// Vector 1 and Vector 2 are logically concatenated, forming a single vector with Vector 1&#8217;s components appearing before Vector 2&#8217;s. The components of this logical vector are logically numbered with a single consecutive set of numbers from 0 to one less than the total number of components. These two vectors must be of the same component type, but do not have to have the same number of components.
    /// 
    /// Components are these logical numbers (see above), selecting which of the logically numbered components form the result. They can select the components in any order and can repeat components. The first component of the result is selected by the first Component operand,  the second component of the result is selected by the second Component operand, etc.
    /// 
    /// Result Type must be a vector of the same component type as the Vector operands' component type.  The number of components in Result Type must be the same as the number of Component operands.
    /// 
    /// Note: A vector &#8220;swizzle&#8221; can be done by using the vector for both Vector operands, or using an OpUndef for one of the Vector operands.
    /// </summary>
    public sealed class OpVectorShuffle : CompositeInstruction
    {
        public override bool IsComposite => true;
        public override OpCode OpCode => OpCode.VectorShuffle;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Vector1;
        public ID Vector2;
        public LiteralNumber[] Components = { };

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Vector1) + ", " + StrOf(Vector2) + ", " + StrOf(Components) + ")";
        public override string ArgString => "Vector1: " + StrOf(Vector1) + ", " + "Vector2: " + StrOf(Vector2) + ", " + "Components: " + StrOf(Components);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.VectorShuffle);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Vector1 = new ID(codes[i++]);
            Vector2 = new ID(codes[i++]);
            var length = WordCount - (i - start);
            Components = new LiteralNumber[length];
            for (var k = 0; k < length; ++k)
                Components[k] = new LiteralNumber(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Vector1.Value);
            code.Add(Vector2.Value);
            if (Components != null)
                foreach (var val in Components)
                    code.Add(val.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Vector1;
                yield return Vector2;
            }
        }
        #endregion
    }
}
