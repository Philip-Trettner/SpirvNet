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
    public sealed class OpSelectionMerge : Instruction
    {
        public override bool IsFlowControl => true;
        public override OpCode OpCode => OpCode.SelectionMerge;

        public ID Label;
        public SelectionControl SelectionControl;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + Label + ", " + SelectionControl + ')';

        public override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.SelectionMerge);
            var i = 1;
            Label = new ID(codes[start + i++]);
            SelectionControl = (SelectionControl)codes[start + i++];
        }

        public override void WriteCode(List<uint> code)
        {
            code.Add(Label.Value);
            code.Add((uint)SelectionControl);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Label;
            }
        }
    }
}
