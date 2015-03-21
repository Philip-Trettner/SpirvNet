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
    public sealed class OpGroupBroadcast : Instruction
    {
        public override bool IsGroup => true;
        public override OpCode OpCode => OpCode.GroupBroadcast;

        public ID ResultType;
        public ID Result;
        public ExecutionScope Scope;
        public ID Value;
        public ID LocalId;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + ResultType + ", " + Result + ", " + Scope + ", " + Value + ", " + LocalId + ')';

        public override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.GroupBroadcast);
            var i = 1;
            ResultType = new ID(codes[start + i++]);
            Result = new ID(codes[start + i++]);
            Scope = (ExecutionScope)codes[start + i++];
            Value = new ID(codes[start + i++]);
            LocalId = new ID(codes[start + i++]);
        }

        public override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add((uint)Scope);
            code.Add(Value.Value);
            code.Add(LocalId.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Value;
                yield return LocalId;
            }
        }
    }
}
