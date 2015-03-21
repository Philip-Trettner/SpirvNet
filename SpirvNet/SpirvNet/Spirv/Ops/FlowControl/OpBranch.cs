using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.FlowControl
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    public sealed class OpBranch : Instruction
    {
        public override bool IsFlowControl => true;
        public override OpCode OpCode => OpCode.Branch;

        public ID TargetLabel;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + TargetLabel + ')';

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Branch);
            var i = 1;
            TargetLabel = new ID(codes[start + i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(TargetLabel.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return TargetLabel;
            }
        }
    }
}
