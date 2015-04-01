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
    /// OpReservedWritePipe
    /// 
    /// Write a packet from ptr into the reserved area specified by reserve_id and index of the pipe object specified by p. The reserved pipe entries are referred to by indices that go from 0 &#8230; num_packets - 1.Returns 0 if the operation is successfull and a negative value otherwise.
    /// 
    /// p must be a OpTypePipe with WriteOnly Access Qualifier.
    /// 
    /// reserve_id must be a OpTypeReserveId.
    /// 
    /// index must be a 32-bits OpTypeInt which is treated as unsigned value.
    /// 
    /// ptr must be a OpTypePointer with the same data type as p and a Generic storage class.
    /// 
    /// Result Type must be a 32-bits OpTypeInt.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpReservedWritePipe : PipeInstruction
    {
        public override bool IsPipe => true;
        public override OpCode OpCode => OpCode.ReservedWritePipe;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID P;
        public ID Reserve_id;
        public ID Index;
        public ID Ptr;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(P) + ", " + StrOf(Reserve_id) + ", " + StrOf(Index) + ", " + StrOf(Ptr) + ")";
        public override string ArgString => "P: " + StrOf(P) + ", " + "Reserve_id: " + StrOf(Reserve_id) + ", " + "Index: " + StrOf(Index) + ", " + "Ptr: " + StrOf(Ptr);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.ReservedWritePipe);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            P = new ID(codes[i++]);
            Reserve_id = new ID(codes[i++]);
            Index = new ID(codes[i++]);
            Ptr = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(P.Value);
            code.Add(Reserve_id.Value);
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
                yield return Reserve_id;
                yield return Index;
                yield return Ptr;
            }
        }
        #endregion
    }
}
