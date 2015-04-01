using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Pipe
{
    /// <summary>
    /// OpCommitWritePipe
    /// 
    /// Indicates that all writes to num_packets associated with the reservation specified by reserve_id and the pipe object specified by p are completed.
    /// 
    /// p must be a OpTypePipe with WriteOnly Access Qualifier.
    /// 
    /// reserve_id must be a OpTypeReserveId.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpCommitWritePipe : PipeInstruction
    {
        public override bool IsPipe => true;
        public override OpCode OpCode => OpCode.CommitWritePipe;

        public ID P;
        public ID Reserve_id;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(P) + ", " + StrOf(Reserve_id) + ")";
        public override string ArgString => "P: " + StrOf(P) + ", " + "Reserve_id: " + StrOf(Reserve_id);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.CommitWritePipe);
            var i = start + 1;
            P = new ID(codes[i++]);
            Reserve_id = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(P.Value);
            code.Add(Reserve_id.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return P;
                yield return Reserve_id;
            }
        }
        #endregion
    }
}
