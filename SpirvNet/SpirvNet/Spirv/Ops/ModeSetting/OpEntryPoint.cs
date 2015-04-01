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
    /// OpEntryPoint
    /// 
    /// Declare an entry point and its execution model.
    /// 
    /// Execution Model is the execution model for the entry point and its static call tree.  See Execution Model.
    /// 
    /// Entry Point must the Result &lt;id&gt; of an OpFunction instruction.
    /// </summary>
    public sealed class OpEntryPoint : ModeSettingInstruction
    {
        public override bool IsModeSetting => true;
        public override OpCode OpCode => OpCode.EntryPoint;

        public ExecutionModel ExecutionModel;
        public ID EntryPoint;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ExecutionModel) + ", " + StrOf(EntryPoint) + ")";
        public override string ArgString => "ExecutionModel: " + StrOf(ExecutionModel) + ", " + "EntryPoint: " + StrOf(EntryPoint);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.EntryPoint);
            var i = start + 1;
            ExecutionModel = (ExecutionModel)codes[i++];
            EntryPoint = new ID(codes[i++]);
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
        #endregion
    }
}
