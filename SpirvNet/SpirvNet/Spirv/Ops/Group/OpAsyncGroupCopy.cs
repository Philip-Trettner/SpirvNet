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
    /// OpAsyncGroupCopy
    /// 
    /// Perform an asynchronous group copy of Num Elements elements from Source to Destination. The asynchronous copy is performed by all work-items in a group.
    /// 
    /// Returns an event object that can be used by OpWaitGroupEvents to wait for the copy to finish.
    /// 
    /// Event must be OpTypeEvent.
    /// 
    /// Event can be used to associate the copy with a previous copy allowing an event to be shared by multiple copies. Otherwise Event should be a OpConstantNullObject.
    /// 
    /// If Event argument is not OpConstantNullObject, the event object supplied in event argument will be returned.
    /// 
    /// Scope must be the Workgroup or Subgroup Execution Scope.
    /// 
    /// Destination and Source should both be pointers to the same integer or floating point scalar or vector data type.
    /// 
    /// Destination and Source pointer storage class can be either WorkgroupLocal or WorkgroupGlobal.
    /// 
    /// When Destination pointer storage class is WorkgroupLocal, the Source pointer storage class must be WorkgroupGlobal. In this case Stride defines the stride in elements when reading from Source pointer.
    /// 
    /// When Destination pointer storage class is WorkgroupGlobal, the Source pointer storage class must be WorkgroupLocal. In this case Stride defines the stride in elements when writing each element to Destination pointer.
    /// 
    /// Stride and NumElemens must be a 32 bit OpTypeInt when the Addressing Model is Physical32 and 64 bit OpTypeInt when the Addressing Model is Physical64.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpAsyncGroupCopy : GroupInstruction
    {
        public override bool IsGroup => true;
        public override OpCode OpCode => OpCode.AsyncGroupCopy;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

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
        public override string ArgString => "Scope: " + StrOf(Scope) + ", " + "Destination: " + StrOf(Destination) + ", " + "Source: " + StrOf(Source) + ", " + "NumElements: " + StrOf(NumElements) + ", " + "Stride: " + StrOf(Stride) + ", " + "Event: " + StrOf(Event);

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
