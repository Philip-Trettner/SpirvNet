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
    public sealed class OpTypeFunction : TypeDeclarationInstruction
    {
        public override bool IsTypeDeclaration => true;
        public override OpCode OpCode => OpCode.TypeFunction;
        public override ID? ResultID => Result;

        public ID Result;
        public ID ReturnType;
        public ID[] ParameterTypes = { };

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Result) + ", " + StrOf(ReturnType) + ", " + StrOf(ParameterTypes) + ")";
        public override string ArgString => "ReturnType: " + StrOf(ReturnType) + ", " + "ParameterTypes: " + StrOf(ParameterTypes);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TypeFunction);
            var i = start + 1;
            Result = new ID(codes[i++]);
            ReturnType = new ID(codes[i++]);
            var length = WordCount - (i - start);
            ParameterTypes = new ID[length];
            for (var k = 0; k < length; ++k)
                ParameterTypes[k] = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Result.Value);
            code.Add(ReturnType.Value);
            if (ParameterTypes != null)
                foreach (var val in ParameterTypes)
                    code.Add(val.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Result;
                yield return ReturnType;
                if (ParameterTypes != null)
                    foreach (var id in ParameterTypes)
                        yield return id;
            }
        }
        #endregion
    }
}
