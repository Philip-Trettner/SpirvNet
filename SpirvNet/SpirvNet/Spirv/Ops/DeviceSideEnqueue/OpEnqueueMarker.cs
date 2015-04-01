using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.DeviceSideEnqueue
{
    /// <summary>
    /// OpEnqueueMarker
    /// 
    /// Enqueue a marker command to to the queue object specified by q. The marker command waits for a list of events to complete, or if the list is empty it waits for all previously enqueued commands in q to complete before the marker completes.
    /// 
    /// Num Events specifies the number of event objects in the wait list pointed Wait Events and must be 32 bit OpTypeInt treated as unsigned integer.
    /// 
    /// Wait Events specifies the list of wait event objects and must be a OpTypePointer to OpTypeDeviceEvent.
    /// 
    /// Ret Event is OpTypePointer to OpTypeDeviceEvent which gets implictly retained by this instruction.  must be a OpTypePointer to OpTypeDeviceEvent. If Ret Event is set to null this instruction becomes a no-op.
    /// 
    /// Result Type must be a 32 bit OpTypeInt.
    /// 
    /// These are the possible return values:
    /// A successfull enqueue is indicated by the integer value 0
    /// A failed enqueue is indicated by the negative integer value -101
    /// 
    /// When running the clCompileProgram or clBuildProgram  with -g flag, the following errors may be returned instead of the negative integer value -101:
    /// - When q is an invalid queue object, the negative integer value -102 is returned.
    /// - When Wait Events is null and Num Events &gt; 0, or if Wait Events is not null and Num Events is 0, or if event objects in Wait Events are not valid events, the negative integer value -57 is returned.
    /// - When the queue object q is full, the negative integer value -161 is returned.
    /// - When Ret Event is not a null object and an event could not be allocated, the negative integer value -100 is returned.
    /// - When there is a failure to queue Invoke in the queue q because of insufficient resources needed to execute the kernel, the negative integer value -5 is returned.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpEnqueueMarker : DeviceSideEnqueueInstruction
    {
        public override bool IsDeviceSideEnqueue => true;
        public override OpCode OpCode => OpCode.EnqueueMarker;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Q;
        public ID NumEvents;
        public ID WaitEvents;
        public ID RetEvent;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Q) + ", " + StrOf(NumEvents) + ", " + StrOf(WaitEvents) + ", " + StrOf(RetEvent) + ")";
        public override string ArgString => "Q: " + StrOf(Q) + ", " + "NumEvents: " + StrOf(NumEvents) + ", " + "WaitEvents: " + StrOf(WaitEvents) + ", " + "RetEvent: " + StrOf(RetEvent);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.EnqueueMarker);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Q = new ID(codes[i++]);
            NumEvents = new ID(codes[i++]);
            WaitEvents = new ID(codes[i++]);
            RetEvent = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Q.Value);
            code.Add(NumEvents.Value);
            code.Add(WaitEvents.Value);
            code.Add(RetEvent.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Q;
                yield return NumEvents;
                yield return WaitEvents;
                yield return RetEvent;
            }
        }
        #endregion
    }
}
