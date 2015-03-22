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
    public sealed class OpEnqueueKernel : DeviceSideEnqueueInstruction
    {
        public override bool IsDeviceSideEnqueue => true;
        public override OpCode OpCode => OpCode.EnqueueKernel;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

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
        public ID[] LocalSize = { };

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(q) + ", " + StrOf(Flags) + ", " + StrOf(NDRange) + ", " + StrOf(NumEvents) + ", " + StrOf(WaitEvents) + ", " + StrOf(RetEvent) + ", " + StrOf(Invoke) + ", " + StrOf(Param) + ", " + StrOf(ParamSize) + ", " + StrOf(ParamAlign) + ", " + StrOf(LocalSize) + ")";
        public override string ArgString => "q: " + StrOf(q) + ", " + "Flags: " + StrOf(Flags) + ", " + "NDRange: " + StrOf(NDRange) + ", " + "NumEvents: " + StrOf(NumEvents) + ", " + "WaitEvents: " + StrOf(WaitEvents) + ", " + "RetEvent: " + StrOf(RetEvent) + ", " + "Invoke: " + StrOf(Invoke) + ", " + "Param: " + StrOf(Param) + ", " + "ParamSize: " + StrOf(ParamSize) + ", " + "ParamAlign: " + StrOf(ParamAlign) + ", " + "LocalSize: " + StrOf(LocalSize);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.EnqueueKernel);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            q = new ID(codes[i++]);
            Flags = (KernelEnqueueFlags)codes[i++];
            NDRange = new ID(codes[i++]);
            NumEvents = new ID(codes[i++]);
            WaitEvents = new ID(codes[i++]);
            RetEvent = new ID(codes[i++]);
            Invoke = new ID(codes[i++]);
            Param = new ID(codes[i++]);
            ParamSize = new ID(codes[i++]);
            ParamAlign = new ID(codes[i++]);
            var length = WordCount - (i - start);
            LocalSize = new ID[length];
            for (var k = 0; k < length; ++k)
                LocalSize[k] = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
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
            if (LocalSize != null)
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
        #endregion
    }
}
