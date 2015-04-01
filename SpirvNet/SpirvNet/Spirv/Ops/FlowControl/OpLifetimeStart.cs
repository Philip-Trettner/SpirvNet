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
    /// OpLifetimeStart
    /// 
    /// Declare that the content of the object pointed to was not defined before this instruction. If Operand 1 has a non-void type, Operand 2 must be 0, otherwise Operand 2 is the amount of memory whose lifetime is starting.
    /// </summary>
    public sealed class OpLifetimeStart : FlowControlInstruction
    {
        public override bool IsFlowControl => true;
        public override OpCode OpCode => OpCode.LifetimeStart;

        public ID Object;
        public LiteralNumber Number;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Object) + ", " + StrOf(Number) + ")";
        public override string ArgString => "Object: " + StrOf(Object) + ", " + "Number: " + StrOf(Number);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.LifetimeStart);
            var i = start + 1;
            Object = new ID(codes[i++]);
            Number = new LiteralNumber(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Object.Value);
            code.Add(Number.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Object;
            }
        }
        #endregion
    }
}
