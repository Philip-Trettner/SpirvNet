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
    /// OpGetNumPipePackets
    /// 
    /// Returns the number of available entries in the pipe object specified by p. The number of available entries in a pipe is a dynamic value.  The value returned should be considered immediately stale.
    /// 
    /// p must be a OpTypePipe with ReadOnly or WriteOnly Access Qualifier.
    /// 
    /// Result Type must be a 32-bits OpTypeInt which should be treated as unsigned value.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpGetNumPipePackets : PipeInstruction
    {
        public override bool IsPipe => true;
        public override OpCode OpCode => OpCode.GetNumPipePackets;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID P;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(P) + ")";
        public override string ArgString => "P: " + StrOf(P);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.GetNumPipePackets);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            P = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(P.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return P;
            }
        }
        #endregion
    }
}
