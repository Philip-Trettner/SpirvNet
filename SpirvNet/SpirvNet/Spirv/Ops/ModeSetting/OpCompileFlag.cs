using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.ModeSetting
{
    /// <summary>
    /// OpCompileFlag
    /// 
    /// Add a compilation Flag.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpCompileFlag : ModeSettingInstruction
    {
        public override bool IsModeSetting => true;
        public override OpCode OpCode => OpCode.CompileFlag;

        public LiteralString Flag;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Flag) + ")";
        public override string ArgString => "Flag: " + StrOf(Flag);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.CompileFlag);
            var i = start + 1;
            Flag = LiteralString.FromCode(codes, ref i);
        }

        protected override void WriteCode(List<uint> code)
        {
            Flag.WriteCode(code);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield break;
            }
        }
        #endregion
    }
}
