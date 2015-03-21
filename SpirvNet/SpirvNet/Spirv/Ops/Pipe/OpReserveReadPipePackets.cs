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
    public sealed class OpReserveReadPipePackets : Instruction
    {
        public override bool IsPipe => true;
        public override OpCode OpCode => OpCode.ReserveReadPipePackets;

        public ID ResultType;
        public ID Result;
        public ID P;
        public ID NumPackets;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + ResultType + ", " + Result + ", " + P + ", " + NumPackets + ')';

        public override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.ReserveReadPipePackets);
            var i = 1;
            ResultType = new ID(codes[start + i++]);
            Result = new ID(codes[start + i++]);
            P = new ID(codes[start + i++]);
            NumPackets = new ID(codes[start + i++]);
        }

        public override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(P.Value);
            code.Add(NumPackets.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return P;
                yield return NumPackets;
            }
        }
    }
}
