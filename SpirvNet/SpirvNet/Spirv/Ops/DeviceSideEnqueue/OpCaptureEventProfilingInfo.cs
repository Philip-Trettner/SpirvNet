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
    /// OpCaptureEventProfilingInfo
    /// 
    /// Captures the profiling information specified by info for the command associated with the event specified by event in the memory pointed by value.The profiling information will be available in value once the command identified by event has completed.
    /// 
    /// event must be a OpTypeDeviceEvent that was produced by OpEnqueueKernel or OpEnqueueMarker. 
    /// 
    /// When info is CmdExecTime value must be a OpTypePointer with WorkgroupGlobal storage class, to two 64-bit OpTypeInt values. The first 64-bit value describes the elapsed time CL_PROFILING_COMMAND_END - CL_PROFLING_COMMAND_START for the command identified by event in nanoseconds. The second 64-bit value describes the elapsed time CL_PROFILING_COMMAND_COMPLETE - CL_PROFILING_COMAMND_START for the command identified by event in nanoseconds.
    /// 
    /// Note: The behavior of of this instruction is undefined when called multiple times for the same event.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpCaptureEventProfilingInfo : DeviceSideEnqueueInstruction
    {
        public override bool IsDeviceSideEnqueue => true;
        public override OpCode OpCode => OpCode.CaptureEventProfilingInfo;

        public ID Event;
        public KernelProfilingInfo Info;
        public ID Value;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Event) + ", " + StrOf(Info) + ", " + StrOf(Value) + ")";
        public override string ArgString => "Event: " + StrOf(Event) + ", " + "Info: " + StrOf(Info) + ", " + "Value: " + StrOf(Value);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.CaptureEventProfilingInfo);
            var i = start + 1;
            Event = new ID(codes[i++]);
            Info = (KernelProfilingInfo)codes[i++];
            Value = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Event.Value);
            code.Add((uint)Info);
            code.Add(Value.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Event;
                yield return Value;
            }
        }
        #endregion
    }
}
