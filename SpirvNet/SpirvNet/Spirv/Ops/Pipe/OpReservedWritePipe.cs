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
    public sealed class OpReservedWritePipe : Instruction
    {
        public override bool IsPipe => true;
        public override OpCode OpCode => OpCode.ReservedWritePipe;

        public ID ResultType;
        public ID Result;
        public ID P;
        public ID ReserveId;
        public ID Index;
        public ID Ptr;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + ResultType + ", " + Result + ", " + P + ", " + ReserveId + ", " + Index + ", " + Ptr + ')';

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.ReservedWritePipe);
            var i = 1;
            ResultType = new ID(codes[start + i++]);
            Result = new ID(codes[start + i++]);
            P = new ID(codes[start + i++]);
            ReserveId = new ID(codes[start + i++]);
            Index = new ID(codes[start + i++]);
            Ptr = new ID(codes[start + i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(P.Value);
            code.Add(ReserveId.Value);
            code.Add(Index.Value);
            code.Add(Ptr.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return P;
                yield return ReserveId;
                yield return Index;
                yield return Ptr;
            }
        }
    }
}
