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
    /// OpTypeBool
    /// 
    /// Declare the Boolean type.  Values of this type can only be either true or false. There is no physical size or bit pattern defined for these values.  If they are stored (in conjuction with OpVariable), they can only be used with logical addressing operations, not physical, and only with non-externally visible shader storage classes: WorkgroupLocal, WorkgroupGlobal, PrivateGlobal, and Function.
    /// 
    /// Result &lt;id&gt; is the &lt;id&gt; of the new Boolean type.
    /// </summary>
    public sealed class OpTypeBool : TypeDeclarationInstruction
    {
        public override bool IsTypeDeclaration => true;
        public override OpCode OpCode => OpCode.TypeBool;
        public override ID? ResultID => Result;

        public ID Result;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Result) + ")";
        public override string ArgString => "";

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TypeBool);
            var i = start + 1;
            Result = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Result.Value);
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
