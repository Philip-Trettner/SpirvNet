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
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpEnqueueKernel : Instruction
    {
        public override bool IsDeviceSideEnqueue => true;
        public override OpCode OpCode => OpCode.EnqueueKernel;

        public ID ResultType;
        public ID Result;
        public ID q;
        public KernelEnqueueFlags Flags;
        public ID NDRange;
        public ID NumEvents;
        public ID WaitEvents;
        public ID RetEvent;
        public ID Invoke;
        public ID Param;
        public ID ParamSize;
        public ID ParamAlign;
        public ID[] LocalSize;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + ResultType + ", " + Result + ", " + q + ", " + Flags + ", " + NDRange + ", " + NumEvents + ", " + WaitEvents + ", " + RetEvent + ", " + Invoke + ", " + Param + ", " + ParamSize + ", " + ParamAlign + ", " + LocalSize + ')';

        public override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.EnqueueKernel);
            var i = 1;
            ResultType = new ID(codes[start + i++]);
            Result = new ID(codes[start + i++]);
            q = new ID(codes[start + i++]);
            Flags = (KernelEnqueueFlags)codes[start + i++];
            NDRange = new ID(codes[start + i++]);
            NumEvents = new ID(codes[start + i++]);
            WaitEvents = new ID(codes[start + i++]);
            RetEvent = new ID(codes[start + i++]);
            Invoke = new ID(codes[start + i++]);
            Param = new ID(codes[start + i++]);
            ParamSize = new ID(codes[start + i++]);
            ParamAlign = new ID(codes[start + i++]);
            var length = WordCount - i + 1;
            LocalSize = new ID[length];
            for (var k = 0; k < length; ++k)
                LocalSize[k] = new ID(codes[start + i++]);
        }

        public override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(q.Value);
            code.Add((uint)Flags);
            code.Add(NDRange.Value);
            code.Add(NumEvents.Value);
            code.Add(WaitEvents.Value);
            code.Add(RetEvent.Value);
            code.Add(Invoke.Value);
            code.Add(Param.Value);
            code.Add(ParamSize.Value);
            code.Add(ParamAlign.Value);
            foreach (var val in LocalSize)
                code.Add(val.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return q;
                yield return NDRange;
                yield return NumEvents;
                yield return WaitEvents;
                yield return RetEvent;
                yield return Invoke;
                yield return Param;
                yield return ParamSize;
                yield return ParamAlign;
                if (LocalSize != null)
                    foreach (var id in LocalSize)
                        yield return id;
            }
        }
    }
}
