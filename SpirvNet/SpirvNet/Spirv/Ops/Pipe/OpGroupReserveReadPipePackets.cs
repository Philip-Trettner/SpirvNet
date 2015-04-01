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
    /// OpGroupReserveReadPipePackets
    /// 
    /// Reserve num_packets entries for reading from the pipe object specified by p at group level. Returns a valid reservation ID if the reservation is successful.
    /// 
    /// The reserved pipe entries are referred to by indices that go from 0 &#8230; num_packets - 1.
    /// 
    /// Scope must be the Workgroup or Subgroup Execution Scope.
    /// 
    /// p must be a OpTypePipe with ReadOnly Access Qualifier.
    /// 
    /// num_packets must be a 32-bits OpTypeInt which is treated as unsigned value.
    /// 
    /// Result Type must be a OpTypeReserveId.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpGroupReserveReadPipePackets : PipeInstruction
    {
        public override bool IsPipe => true;
        public override OpCode OpCode => OpCode.GroupReserveReadPipePackets;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ExecutionScope Scope;
        public ID P;
        public ID Num_packets;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Scope) + ", " + StrOf(P) + ", " + StrOf(Num_packets) + ")";
        public override string ArgString => "Scope: " + StrOf(Scope) + ", " + "P: " + StrOf(P) + ", " + "Num_packets: " + StrOf(Num_packets);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.GroupReserveReadPipePackets);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Scope = (ExecutionScope)codes[i++];
            P = new ID(codes[i++]);
            Num_packets = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add((uint)Scope);
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
