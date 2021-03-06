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
    /// OpRetainEvent
    /// 
    /// Increments the reference count of the event object specified by event.
    /// 
    /// event must be an event that was produced by OpEnqueueKernel, OpEnqueueMarker or OpCreateUserEvent.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpRetainEvent : DeviceSideEnqueueInstruction
    {
        public override bool IsDeviceSideEnqueue => true;
        public override OpCode OpCode => OpCode.RetainEvent;

        public ID Event;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Event) + ")";
        public override string ArgString => "Event: " + StrOf(Event);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.RetainEvent);
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
