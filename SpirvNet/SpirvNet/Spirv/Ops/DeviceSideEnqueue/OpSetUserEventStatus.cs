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
    /// OpSetUserEventStatus
    /// 
    /// Sets the execution status of a user event specified by event.status can be either 0 (CL_COMPLETE) to indicate that this kernel and all its child kernels finished execution successfully, or a negative integer value indicating an error.
    /// 
    /// event must be a OpTypeDeviceEvent that was produced by OpCreateUserEvent.
    /// 
    /// status must be a 32-bit OpTypeInt treated as a signed integer.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpSetUserEventStatus : DeviceSideEnqueueInstruction
    {
        public override bool IsDeviceSideEnqueue => true;
        public override OpCode OpCode => OpCode.SetUserEventStatus;

        public ID Event;
        public ID Status;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Event) + ", " + StrOf(Status) + ")";
        public override string ArgString => "Event: " + StrOf(Event) + ", " + "Status: " + StrOf(Status);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.SetUserEventStatus);
            var i = start + 1;
            Event = new ID(codes[i++]);
            Status = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Event.Value);
            code.Add(Status.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Event;
                yield return Status;
            }
        }
        #endregion
    }
}
