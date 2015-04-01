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
    /// OpTypeMatrix
    /// 
    /// Declare a new matrix type.
    /// 
    /// Column type is the type of each column in the matrix.  It must be vector type.
    /// 
    /// Column count is the number of columns in the new matrix type. It must be at least 2.
    /// 
    /// Result &lt;id&gt; is the &lt;id&gt; of the new matrix type.
    /// </summary>
    [DependsOn(LanguageCapability.Matrix)]
    public sealed class OpTypeMatrix : TypeDeclarationInstruction
    {
        public override bool IsTypeDeclaration => true;
        public override OpCode OpCode => OpCode.TypeMatrix;
        public override ID? ResultID => Result;

        public ID Result;
        public ID ColumnType;
        public LiteralNumber ColumnCount;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Result) + ", " + StrOf(ColumnType) + ", " + StrOf(ColumnCount) + ")";
        public override string ArgString => "ColumnType: " + StrOf(ColumnType) + ", " + "ColumnCount: " + StrOf(ColumnCount);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TypeMatrix);
            var i = start + 1;
            Result = new ID(codes[i++]);
            ColumnType = new ID(codes[i++]);
            ColumnCount = new LiteralNumber(codes[i++]);
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
        #endregion
    }
}
