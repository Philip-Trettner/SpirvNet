using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.TypeDeclaration
{
    /// <summary>
    /// OpTypeArray
    /// 
    /// Declare a new array type: a dynamically-indexable ordered aggregate of elements all having the same type.
    /// 
    /// Element Type is the type of each element in the array.
    /// 
    /// Length is the number of elements in the array.  It must be at least 1. Length must come from a constant instruction of an Integer-type scalar whose value is at least 1.
    /// 
    /// Result &lt;id&gt; is the &lt;id&gt; of the new array type.
    /// </summary>
    public sealed class OpTypeArray : TypeDeclarationInstruction
    {
        public override bool IsTypeDeclaration => true;
        public override OpCode OpCode => OpCode.TypeArray;
        public override ID? ResultID => Result;

        public ID Result;
        public ID ElementType;
        public ID Length;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Result) + ", " + StrOf(ElementType) + ", " + StrOf(Length) + ")";
        public override string ArgString => "ElementType: " + StrOf(ElementType) + ", " + "Length: " + StrOf(Length);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TypeArray);
            var i = start + 1;
            Result = new ID(codes[i++]);
            ElementType = new ID(codes[i++]);
            Length = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Result.Value);
            code.Add(ElementType.Value);
            code.Add(Length.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Result;
                yield return ElementType;
                yield return Length;
            }
        }
        #endregion
    }
}
