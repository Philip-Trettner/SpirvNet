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
    /// OpReleaseEvent
    /// 
    /// Decrements the reference count of the event object specified by event. The event object is deleted once the event reference count is zero, the specific command identified by this event has completed (or terminated) and there are no commands in any device command queue that require a wait for this event to complete.
    /// 
    /// event must be an event that was produced by OpEnqueueKernel, OpEnqueueMarker or OpCreateUserEvent.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpReleaseEvent : DeviceSideEnqueueInstruction
    {
        public override bool IsDeviceSideEnqueue => true;
        public override OpCode OpCode => OpCode.ReleaseEvent;

        public ID Event;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Event) + ")";
        public override string ArgString => "Event: " + StrOf(Event);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.ReleaseEvent);
            var i = start + 1;
            Event = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Event.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Event;
            }
        }
        #endregion
    }
}
