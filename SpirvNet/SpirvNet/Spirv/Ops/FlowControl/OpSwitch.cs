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
    /// OpSwitch
    /// 
    /// Multi-way branch to one of the operand label &lt;id&gt;.
    /// 
    /// Selector must be a scalar integer type.  It will be compared for equality to the Target literals.
    /// 
    /// Default must be the &lt;id&gt; of a label.  If Selector does not equal any of the Target literals, control flow will branch to the Default label &lt;id&gt;.
    /// 
    /// Target must be alternating scalar-integer literals and the &lt;id&gt; of a label.  If Selector equals one of the literals, control flow will branch to the following label &lt;id&gt;. It is invalid for any two Target literals to be equal to each other. If Target is not present, control flow will branch to the Default label &lt;id&gt;.
    /// 
    /// This instruction must be the last instruction in a block.
    /// </summary>
    public sealed class OpSwitch : FlowControlInstruction
    {
        public override bool IsFlowControl => true;
        public override OpCode OpCode => OpCode.Switch;

        public ID Selector;
        public ID Default;
        public Pair<LiteralNumber, ID>[] Target = { };

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Selector) + ", " + StrOf(Default) + ", " + StrOf(Target) + ")";
        public override string ArgString => "Selector: " + StrOf(Selector) + ", " + "Default: " + StrOf(Default) + ", " + "Target: " + StrOf(Target);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Switch);
            var i = start + 1;
            Selector = new ID(codes[i++]);
            Default = new ID(codes[i++]);
            var length = (WordCount - (i - start)) / 2;
            Target = new Pair<LiteralNumber, ID>[length];
            for (var k = 0; k < length; ++k)
            {
                var f = new LiteralNumber(codes[i++]);
                var s = new ID(codes[i++]);
                Target[k] = new Pair<LiteralNumber, ID>(f, s);
            }
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Selector.Value);
            code.Add(Default.Value);
            if (Target != null)
                foreach (var val in Target)
                {
                    code.Add(val.First.Value);
                    code.Add(val.Second.Value);
                }
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Selector;
                yield return Default;
                if (Target != null)
                    foreach (var p in Target)
                        yield return p.Second;
            }
        }
        #endregion
    }
}
