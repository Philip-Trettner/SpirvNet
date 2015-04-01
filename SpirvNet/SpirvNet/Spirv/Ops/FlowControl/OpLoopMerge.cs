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
    /// OpLoopMerge
    /// 
    /// Declare and control a structured control-flow loop construct.
    /// 
    /// Label is the label of the merge block for this structured loop construct.
    /// 
    /// See Structured Control Flow for more detail.
    /// </summary>
    public sealed class OpLoopMerge : FlowControlInstruction
    {
        public override bool IsFlowControl => true;
        public override OpCode OpCode => OpCode.LoopMerge;

        public ID Label;
        public LoopControl LoopControl;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Label) + ", " + StrOf(LoopControl) + ")";
        public override string ArgString => "Label: " + StrOf(Label) + ", " + "LoopControl: " + StrOf(LoopControl);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.LoopMerge);
            var i = start + 1;
            Label = new ID(codes[i++]);
            LoopControl = (LoopControl)codes[i++];
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Label.Value);
            code.Add((uint)LoopControl);
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
