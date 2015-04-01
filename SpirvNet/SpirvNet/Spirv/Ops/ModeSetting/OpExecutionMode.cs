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
    /// OpExecutionMode
    /// 
    /// Declare an execution mode for an entry point.
    /// 
    /// Entry Point must be the Entry Point &lt;id&gt; operand of an OpEntryPoint instruction.
    /// 
    /// Mode is the execution mode. See Execution Mode.
    /// </summary>
    public sealed class OpExecutionMode : ModeSettingInstruction
    {
        public override bool IsModeSetting => true;
        public override OpCode OpCode => OpCode.ExecutionMode;

        public ID EntryPoint;
        public ExecutionMode Mode;
        public LiteralNumber[] ExtraOperands = { };

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(EntryPoint) + ", " + StrOf(Mode) + ", " + StrOf(ExtraOperands) + ")";
        public override string ArgString => "EntryPoint: " + StrOf(EntryPoint) + ", " + "Mode: " + StrOf(Mode) + ", " + "ExtraOperands: " + StrOf(ExtraOperands);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.ExecutionMode);
            var i = start + 1;
            EntryPoint = new ID(codes[i++]);
            Mode = (ExecutionMode)codes[i++];
            var length = WordCount - (i - start);
            ExtraOperands = new LiteralNumber[length];
            for (var k = 0; k < length; ++k)
                ExtraOperands[k] = new LiteralNumber(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(EntryPoint.Value);
            code.Add((uint)Mode);
            if (ExtraOperands != null)
                foreach (var val in ExtraOperands)
                    code.Add(val.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return EntryPoint;
            }
        }
        #endregion
    }
}
