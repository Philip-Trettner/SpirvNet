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
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    public sealed class OpTypeStruct : Instruction
    {
        public override bool IsTypeDeclaration => true;
        public override OpCode OpCode => OpCode.TypeStruct;

        public ID Result;
        public ID[] MemberTypes = { };

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Result) + ", " + StrOf(MemberTypes) + ")";

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TypeStruct);
            var i = start + 1;
            Result = new ID(codes[i++]);
            var length = WordCount - (i - start);
            MemberTypes = new ID[length];
            for (var k = 0; k < length; ++k)
                MemberTypes[k] = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Result.Value);
            if (MemberTypes != null)
                foreach (var val in MemberTypes)
                    code.Add(val.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Result;
                if (MemberTypes != null)
                    foreach (var id in MemberTypes)
                        yield return id;
            }
        }
        #endregion
    }
}
