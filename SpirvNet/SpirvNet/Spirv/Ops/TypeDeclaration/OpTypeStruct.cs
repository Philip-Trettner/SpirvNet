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
    /// OpTypeStruct
    /// 
    /// Declare a new structure type: an aggregate of heteregeneous members.
    /// 
    /// Member N type is the type of member N of the structure. The first member is member 0, the next is member 1, &#8230;
    /// 
    /// Result &lt;id&gt; is the &lt;id&gt; of the new structure type.
    /// </summary>
    public sealed class OpTypeStruct : TypeDeclarationInstruction
    {
        public override bool IsTypeDeclaration => true;
        public override OpCode OpCode => OpCode.TypeStruct;
        public override ID? ResultID => Result;

        public ID Result;
        public ID[] Members = { };

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Result) + ", " + StrOf(Members) + ")";
        public override string ArgString => "Members: " + StrOf(Members);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TypeStruct);
            var i = start + 1;
            Result = new ID(codes[i++]);
            var length = WordCount - (i - start);
            Members = new ID[length];
            for (var k = 0; k < length; ++k)
                Members[k] = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Result.Value);
            if (Members != null)
                foreach (var val in Members)
                    code.Add(val.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Result;
                if (Members != null)
                    foreach (var id in Members)
                        yield return id;
            }
        }
        #endregion
    }
}
