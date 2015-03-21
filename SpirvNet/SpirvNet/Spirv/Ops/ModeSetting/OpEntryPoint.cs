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
    public sealed class OpEntryPoint : Instruction
    {
        public override bool IsModeSetting => true;
        public override OpCode OpCode => OpCode.EntryPoint;

        public ExecutionModel ExecutionModel;
        public ID EntryPoint;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + ExecutionModel + ", " + EntryPoint + ')';

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.EntryPoint);
            var i = 1;
            ExecutionModel = (ExecutionModel)codes[start + i++];
            EntryPoint = new ID(codes[start + i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add((uint)ExecutionModel);
            code.Add(EntryPoint.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return EntryPoint;
            }
        }
    }
}
