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
    /// OpTypeVector
    /// 
    /// Declare a new vector type.
    /// 
    /// Component type is the type of each component in the resulting type.
    /// 
    /// Component count is the number of compononents in the resulting type.  It must be at least 2.
    /// 
    /// Result &lt;id&gt; is the &lt;id&gt; of the new vector type.
    /// </summary>
    public sealed class OpTypeVector : TypeDeclarationInstruction
    {
        public override bool IsTypeDeclaration => true;
        public override OpCode OpCode => OpCode.TypeVector;
        public override ID? ResultID => Result;

        public ID Result;
        public ID ComponentType;
        public LiteralNumber ComponentCount;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Result) + ", " + StrOf(ComponentType) + ", " + StrOf(ComponentCount) + ")";
        public override string ArgString => "ComponentType: " + StrOf(ComponentType) + ", " + "ComponentCount: " + StrOf(ComponentCount);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TypeVector);
            var i = start + 1;
            Result = new ID(codes[i++]);
            ComponentType = new ID(codes[i++]);
            ComponentCount = new LiteralNumber(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Result.Value);
            code.Add(ComponentType.Value);
            code.Add(ComponentCount.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Result;
                yield return ComponentType;
            }
        }
        #endregion
    }
}
