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
    /// OpGroupCommitWritePipe
    /// 
    /// A group level indication that all writes to num_packets associated with the reservation specified by reserve_id to the pipe object specified by p are completed.
    /// 
    /// Scope must be the Workgroup or Subgroup Execution Scope.
    /// 
    /// p must be a OpTypePipe with WriteOnly Access Qualifier.
    /// 
    /// reserve_id must be a OpTypeReserveId.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpGroupCommitWritePipe : PipeInstruction
    {
        public override bool IsPipe => true;
        public override OpCode OpCode => OpCode.GroupCommitWritePipe;

        public ExecutionScope Scope;
        public ID P;
        public ID Reserve_id;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Scope) + ", " + StrOf(P) + ", " + StrOf(Reserve_id) + ")";
        public override string ArgString => "Scope: " + StrOf(Scope) + ", " + "P: " + StrOf(P) + ", " + "Reserve_id: " + StrOf(Reserve_id);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.GroupCommitWritePipe);
            var i = start + 1;
            Scope = (ExecutionScope)codes[i++];
            P = new ID(codes[i++]);
            Reserve_id = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add((uint)Scope);
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
