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
    /// OpMemoryModel
    /// 
    /// Set addressing model and memory model for the entire module.
    /// 
    /// Addressing Model selects the module&#8217;s addressing model, see Addressing Model.
    /// 
    /// Memory Model selects the module&#8217;s memory model, see Memory Model.
    /// </summary>
    public sealed class OpMemoryModel : ModeSettingInstruction
    {
        public override bool IsModeSetting => true;
        public override OpCode OpCode => OpCode.MemoryModel;

        public AddressingModel AddressingModel;
        public MemoryModel MemoryModel;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(AddressingModel) + ", " + StrOf(MemoryModel) + ")";
        public override string ArgString => "AddressingModel: " + StrOf(AddressingModel) + ", " + "MemoryModel: " + StrOf(MemoryModel);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.MemoryModel);
            var i = start + 1;
            AddressingModel = (AddressingModel)codes[i++];
            MemoryModel = (MemoryModel)codes[i++];
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add((uint)AddressingModel);
            code.Add((uint)MemoryModel);
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
