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
    public sealed class OpSwitch : Instruction
    {
        public override bool IsFlowControl => true;
        public override OpCode OpCode => OpCode.Switch;

        public ID Selector;
        public ID Default;
        public Pair<LiteralNumber, ID>[] Target;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + Selector + ", " + Default + ", " + Target + ')';

        public override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Switch);
            var i = 1;
            Selector = new ID(codes[start + i++]);
            Default = new ID(codes[start + i++]);
            var length = (WordCount - i + 1) / 2;
            Target = new Pair<LiteralNumber, ID>[length];
            for (var k = 0; k < length; ++k)
                {
                    var f = new LiteralNumber(codes[start + i++]);
                    var s = new ID(codes[start + i++]);
                Target[k] = new Pair<LiteralNumber, ID>(f, s);
                }
        }

        public override void WriteCode(List<uint> code)
        {
            code.Add(Selector.Value);
            code.Add(Default.Value);
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
    }
}
