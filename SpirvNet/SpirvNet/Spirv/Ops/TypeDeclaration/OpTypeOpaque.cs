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
    /// OpTypeOpaque
    /// 
    /// Declare a named structure type with no body specified.
    /// 
    /// Result &lt;id&gt; is the &lt;id&gt; of the new opaque type.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpTypeOpaque : TypeDeclarationInstruction
    {
        public override bool IsTypeDeclaration => true;
        public override OpCode OpCode => OpCode.TypeOpaque;
        public override ID? ResultID => Result;

        public ID Result;
        public LiteralString TheNameOfTheOpaqueType;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Result) + ", " + StrOf(TheNameOfTheOpaqueType) + ")";
        public override string ArgString => "TheNameOfTheOpaqueType: " + StrOf(TheNameOfTheOpaqueType);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TypeOpaque);
            var i = start + 1;
            Result = new ID(codes[i++]);
            TheNameOfTheOpaqueType = LiteralString.FromCode(codes, ref i);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Result.Value);
            TheNameOfTheOpaqueType.WriteCode(code);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Result;
            }
        }
        #endregion
    }
}
