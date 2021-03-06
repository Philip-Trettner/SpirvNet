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
    /// OpReserveReadPipePackets
    /// 
    /// Reserve num_packets entries for reading from the pipe object specified by p. Returns a valid reservation ID if the reservation is successful.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpReserveReadPipePackets : PipeInstruction
    {
        public override bool IsPipe => true;
        public override OpCode OpCode => OpCode.ReserveReadPipePackets;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID P;
        public ID Num_packets;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(P) + ", " + StrOf(Num_packets) + ")";
        public override string ArgString => "P: " + StrOf(P) + ", " + "Num_packets: " + StrOf(Num_packets);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.ReserveReadPipePackets);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            P = new ID(codes[i++]);
            Num_packets = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(P.Value);
            code.Add(Num_packets.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return P;
                yield return Num_packets;
            }
        }
        #endregion
    }
}
