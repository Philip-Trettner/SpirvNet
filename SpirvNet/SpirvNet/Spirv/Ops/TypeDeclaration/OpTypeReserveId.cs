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
    /// OpTypeReserveId
    /// 
    /// Declare an OpenCL reservation id object.
    /// 
    /// Result &lt;id&gt; is the &lt;id&gt; of the new reservation type.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpTypeReserveId : TypeDeclarationInstruction
    {
        public override bool IsTypeDeclaration => true;
        public override OpCode OpCode => OpCode.TypeReserveId;
        public override ID? ResultID => Result;

        public ID Result;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Result) + ")";
        public override string ArgString => "";

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TypeReserveId);
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
