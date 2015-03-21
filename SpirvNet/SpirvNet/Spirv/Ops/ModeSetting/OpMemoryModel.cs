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
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    public sealed class OpMemoryModel : Instruction
    {
        public override bool IsModeSetting => true;
        public override OpCode OpCode => OpCode.MemoryModel;

        public AddressingModel AddressingModel;
        public MemoryModel MemoryModel;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(AddressingModel) + ", " + StrOf(MemoryModel) + ")";

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.MemoryModel);
            var i = 1;
            AddressingModel = (AddressingModel)codes[start + i++];
            MemoryModel = (MemoryModel)codes[start + i++];
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
