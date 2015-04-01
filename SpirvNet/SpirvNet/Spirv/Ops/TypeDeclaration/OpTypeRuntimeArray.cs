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
    /// OpTypeRuntimeArray
    /// 
    /// Declare a new run-time array type.  Its length is not known at compile time.
    /// 
    /// Element type is the type of each element in the array. See OpArrayLength for getting the Length of an array of this type.
    /// 
    /// Objects of this type can only be created with OpVariable using the Uniform Storage Class.
    /// 
    /// Result &lt;id&gt; is the &lt;id&gt; of the new run-time array type.
    /// </summary>
    [DependsOn(LanguageCapability.Shader)]
    public sealed class OpTypeRuntimeArray : TypeDeclarationInstruction
    {
        public override bool IsTypeDeclaration => true;
        public override OpCode OpCode => OpCode.TypeRuntimeArray;
        public override ID? ResultID => Result;

        public ID Result;
        public ID ElementType;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Result) + ", " + StrOf(ElementType) + ")";
        public override string ArgString => "ElementType: " + StrOf(ElementType);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TypeRuntimeArray);
            var i = start + 1;
            Result = new ID(codes[i++]);
            ElementType = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Result.Value);
            code.Add(ElementType.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Result;
                yield return ElementType;
            }
        }
        #endregion
    }
}
