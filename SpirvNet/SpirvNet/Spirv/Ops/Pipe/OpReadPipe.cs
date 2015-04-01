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
    /// OpReadPipe
    /// 
    /// Read a packet from the pipe object specified by p into ptr. Returns 0 if the operation is successfull and a negative value if the pipe is empty.
    /// 
    /// p must be a OpTypePipe with ReadOnly Access Qualifier.
    /// 
    /// ptr must be a OpTypePointer with the same data type as p and a Generic storage class.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpReadPipe : PipeInstruction
    {
        public override bool IsPipe => true;
        public override OpCode OpCode => OpCode.ReadPipe;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID P;
        public ID Ptr;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(P) + ", " + StrOf(Ptr) + ")";
        public override string ArgString => "P: " + StrOf(P) + ", " + "Ptr: " + StrOf(Ptr);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.ReadPipe);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            P = new ID(codes[i++]);
            Ptr = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(P.Value);
            code.Add(Ptr.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return P;
                yield return Ptr;
            }
        }
        #endregion
    }
}
