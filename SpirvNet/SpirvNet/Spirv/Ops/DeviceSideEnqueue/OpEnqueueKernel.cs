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
    /// OpEnqueueKernel
    /// 
    /// Enqueue the the function specified by Invoke and the NDRange specified by ND Range for execution to the queue object specified by q. 
    /// 
    /// ND Range must be a OpTypeStruct created by OpBuildNDRange.
    /// 
    /// Num Events specifies the number of event objects in the wait list pointed Wait Events and must be 32 bit OpTypeInt treated as unsigned integer.
    /// 
    /// Wait Events specifies the list of wait event objects and must be a OpTypePointer to OpTypeDeviceEvent.
    /// 
    /// Ret Event is OpTypePointer to OpTypeDeviceEvent which gets implictly retained by this instruction.  must be a OpTypePointer to OpTypeDeviceEvent.
    /// 
    /// Invoke must be a OpTypeFunction with the following signature:
    /// - Result Type must be OpTypeVoid.
    /// - The first parameter must be OpTypePointer to 8 bits OpTypeInt.
    /// - Optional list of parameters that must be OpTypePointer with WorkgroupLocal storage class.
    /// 
    /// Param is the first parameter of the function specified by Invoke and must be OpTypePointer to 8 bit OpTypeInt.
    /// 
    /// Param Size is the size in bytes of the memory pointed by Param and must be a 32 bit OpTypeInt treated as unsigned int.
    /// 
    /// Param Align is the alignment of Param.
    /// 
    /// Local Size is an optional list of 32 bit OpTypeInt values which are treated as unsigned integers. Every Local Size specifies the size in bytes of the OpTypePointer with WorkgroupLocal of Invoke.  The number of Local Size operands must match the signature of Invoke OpTypeFunction
    /// 
    /// Result Type must be a 32 bit OpTypeInt. 
    /// 
    /// These are the possible return values:
    /// A successfull enqueue is indicated by the integer value 0
    /// A failed enqueue is indicated by the negative integer value -101
    /// 
    /// When running the clCompileProgram or clBuildProgram  with -g flag, the following errors may be returned instead of the negative value -101:
    /// - When q is an invalid queue object, the negative integer value -102 is returned.
    /// - When ND Range is an invalid descriptor or if the program was compiled with -cl-uniform-work-group-size and the local work size is specified in ndrange but the global work size specified in ND Range is not a multiple of the local work size, the negative integer value -160 is returned.
    /// - When Wait Events is null and Num Events &gt; 0, or if Wait Events is not null and Num Events is 0, or if event objects in Wait Events are not valid events, the negative integer value -57 is returned.
    /// - When the queue object q is full, the negative integer value -161 is returned.
    /// - When one of the operands Local Size is 0, the negative integer value -51 is returned.
    /// - When Ret Event is not a null object and an event could not be allocated, the negative integer value -100 is returned.
    /// - When there is a failure to queue Invoke in the queue q because of insufficient resources needed to execute the kernel, the negative integer value -5 is returned.
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
        public ID Q;
        public KernelEnqueueFlags Flags;
        public ID NDRange;
        public ID NumEvents;
        public ID WaitEvents;
        public ID RetEvent;
        public ID Invoke;
        public ID Param;
        public ID ParamSize;
        public ID ParamAlign;
        public ID[] Locals = { };

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Q) + ", " + StrOf(Flags) + ", " + StrOf(NDRange) + ", " + StrOf(NumEvents) + ", " + StrOf(WaitEvents) + ", " + StrOf(RetEvent) + ", " + StrOf(Invoke) + ", " + StrOf(Param) + ", " + StrOf(ParamSize) + ", " + StrOf(ParamAlign) + ", " + StrOf(Locals) + ")";
        public override string ArgString => "Q: " + StrOf(Q) + ", " + "Flags: " + StrOf(Flags) + ", " + "NDRange: " + StrOf(NDRange) + ", " + "NumEvents: " + StrOf(NumEvents) + ", " + "WaitEvents: " + StrOf(WaitEvents) + ", " + "RetEvent: " + StrOf(RetEvent) + ", " + "Invoke: " + StrOf(Invoke) + ", " + "Param: " + StrOf(Param) + ", " + "ParamSize: " + StrOf(ParamSize) + ", " + "ParamAlign: " + StrOf(ParamAlign) + ", " + "Locals: " + StrOf(Locals);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.EnqueueKernel);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Q = new ID(codes[i++]);
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
            Locals = new ID[length];
            for (var k = 0; k < length; ++k)
                Locals[k] = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Q.Value);
            code.Add((uint)Flags);
            code.Add(NDRange.Value);
            code.Add(NumEvents.Value);
            code.Add(WaitEvents.Value);
            code.Add(RetEvent.Value);
            code.Add(Invoke.Value);
            code.Add(Param.Value);
            code.Add(ParamSize.Value);
            code.Add(ParamAlign.Value);
            if (Locals != null)
                foreach (var val in Locals)
                    code.Add(val.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Q;
                yield return NDRange;
                yield return NumEvents;
                yield return WaitEvents;
                yield return RetEvent;
                yield return Invoke;
                yield return Param;
                yield return ParamSize;
                yield return ParamAlign;
                if (Locals != null)
                    foreach (var id in Locals)
                        yield return id;
            }
        }
        #endregion
    }
}
