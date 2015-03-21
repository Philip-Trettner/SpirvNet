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
    public sealed class OpAsyncGroupCopy : Instruction
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

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + ResultType + ", " + Result + ", " + Scope + ", " + Destination + ", " + Source + ", " + NumElements + ", " + Stride + ", " + Event + ')';

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.AsyncGroupCopy);
            var i = 1;
            ResultType = new ID(codes[start + i++]);
            Result = new ID(codes[start + i++]);
            Scope = (ExecutionScope)codes[start + i++];
            Destination = new ID(codes[start + i++]);
            Source = new ID(codes[start + i++]);
            NumElements = new ID(codes[start + i++]);
            Stride = new ID(codes[start + i++]);
            Event = new ID(codes[start + i++]);
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
    }
}
