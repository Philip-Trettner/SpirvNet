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
    /// OpGroupBroadcast
    /// 
    /// Broadcast a value for workitem identified by the local id to all work-items in the group.
    /// 
    /// Value and Result Type must be a 32 or 64 bits wise OpTypeInt  or a 16, 32 or 64 OpTypeFloat floating-point scalar datatype.
    /// 
    /// LocalId must be an integer datatype. It can be a scalar, or a vector with 2 components or a vector with 3 components. LocalId must be the same for all work-items in the group.
    /// 
    /// Scope must be the Workgroup or Subgroup Execution Scope.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpGroupBroadcast : GroupInstruction
    {
        public override bool IsGroup => true;
        public override OpCode OpCode => OpCode.GroupBroadcast;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ExecutionScope Scope;
        public ID Value;
        public ID LocalId;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Scope) + ", " + StrOf(Value) + ", " + StrOf(LocalId) + ")";
        public override string ArgString => "Scope: " + StrOf(Scope) + ", " + "Value: " + StrOf(Value) + ", " + "LocalId: " + StrOf(LocalId);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.GroupBroadcast);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Scope = (ExecutionScope)codes[i++];
            Value = new ID(codes[i++]);
            LocalId = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
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
        #endregion
    }
}
