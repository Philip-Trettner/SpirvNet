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

        public ID Result;
        public ID ReturnType;
        public ID[] FunctionTypes = { };

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Result) + ", " + StrOf(ReturnType) + ", " + StrOf(FunctionTypes) + ")";

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TypeFunction);
            var i = start + 1;
            Result = new ID(codes[i++]);
            ReturnType = new ID(codes[i++]);
            var length = WordCount - (i - start);
            FunctionTypes = new ID[length];
            for (var k = 0; k < length; ++k)
                FunctionTypes[k] = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Result.Value);
            code.Add(ReturnType.Value);
            if (FunctionTypes != null)
                foreach (var val in FunctionTypes)
                    code.Add(val.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Result;
                yield return ReturnType;
                if (FunctionTypes != null)
                    foreach (var id in FunctionTypes)
                        yield return id;
            }
        }
        #endregion
    }
}
