using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Group
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpAsyncGroupCopy : GroupInstruction
    {
        public override bool IsGroup => true;
        public override OpCode OpCode => OpCode.AsyncGroupCopy;

        public ID ResultType;
        public ID Result;
        public ExecutionScope Scope;
        public ID Destination;
        public ID Source;
        public ID NumElements;
        public ID Stride;
        public ID Event;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Scope) + ", " + StrOf(Destination) + ", " + StrOf(Source) + ", " + StrOf(NumElements) + ", " + StrOf(Stride) + ", " + StrOf(Event) + ")";

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.AsyncGroupCopy);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Scope = (ExecutionScope)codes[i++];
            Destination = new ID(codes[i++]);
            Source = new ID(codes[i++]);
            NumElements = new ID(codes[i++]);
            Stride = new ID(codes[i++]);
            Event = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add((uint)Scope);
            code.Add(Destination.Value);
            code.Add(Source.Value);
            code.Add(NumElements.Value);
            code.Add(Stride.Value);
            code.Add(Event.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Destination;
                yield return Source;
                yield return NumElements;
                yield return Stride;
                yield return Event;
            }
        }
        #endregion
    }
}
