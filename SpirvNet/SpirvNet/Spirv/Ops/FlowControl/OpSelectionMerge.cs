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
    /// OpSelectionMerge
    /// 
    /// Declare and control a structured control-flow selection construct, used with OpBranchConditional or OpSwitch.
    /// 
    /// Label is the label of the merge block for this structured selection construct.
    /// 
    /// See Structured Control Flow for more detail.
    /// </summary>
    public sealed class OpSelectionMerge : FlowControlInstruction
    {
        public override bool IsFlowControl => true;
        public override OpCode OpCode => OpCode.SelectionMerge;

        public ID Label;
        public SelectionControl SelectionControl;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Label) + ", " + StrOf(SelectionControl) + ")";
        public override string ArgString => "Label: " + StrOf(Label) + ", " + "SelectionControl: " + StrOf(SelectionControl);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.SelectionMerge);
            var i = start + 1;
            Label = new ID(codes[i++]);
            SelectionControl = (SelectionControl)codes[i++];
        }

        protected override void WriteCode(List<uint> code)
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
        #endregion
    }
}
