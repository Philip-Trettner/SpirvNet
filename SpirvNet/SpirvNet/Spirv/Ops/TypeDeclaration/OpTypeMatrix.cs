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
    public sealed class OpTypeMatrix : Instruction
    {
        public override bool IsTypeDeclaration => true;
        public override OpCode OpCode => OpCode.TypeMatrix;

        public ID Result;
        public ID ColumnType;
        public LiteralNumber ColumnCount;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + Result + ", " + ColumnType + ", " + ColumnCount + ')';

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TypeMatrix);
            var i = 1;
            Result = new ID(codes[start + i++]);
            ColumnType = new ID(codes[start + i++]);
            ColumnCount = new LiteralNumber(codes[start + i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Result.Value);
            code.Add(ColumnType.Value);
            code.Add(ColumnCount.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Result;
                yield return ColumnType;
            }
        }
    }
}
