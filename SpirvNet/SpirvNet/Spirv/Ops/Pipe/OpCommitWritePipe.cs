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
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpCommitWritePipe : PipeInstruction
    {
        public override bool IsPipe => true;
        public override OpCode OpCode => OpCode.CommitWritePipe;

        public ID P;
        public ID ReserveId;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(P) + ", " + StrOf(ReserveId) + ")";
        public override string ArgString => "P: " + StrOf(P) + ", " + "ReserveId: " + StrOf(ReserveId);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.CommitWritePipe);
            var i = start + 1;
            P = new ID(codes[i++]);
            ReserveId = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(P.Value);
            code.Add(ReserveId.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return P;
                yield return ReserveId;
            }
        }
        #endregion
    }
}
