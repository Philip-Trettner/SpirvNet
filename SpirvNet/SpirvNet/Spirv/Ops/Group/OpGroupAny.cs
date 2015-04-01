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
    /// OpGroupAny
    /// 
    /// Evaluates a predicate for all work-items in the group,and returns true if predicate evaluates to true for any work-item in the group, otherwise returns false.
    /// 
    /// Both the Predicate and the Result Type must be of OpTypeBool.
    /// 
    /// Scope must be the Workgroup or Subgroup Execution Scope.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpGroupAny : GroupInstruction
    {
        public override bool IsGroup => true;
        public override OpCode OpCode => OpCode.GroupAny;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ExecutionScope Scope;
        public ID Predicate;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Scope) + ", " + StrOf(Predicate) + ")";
        public override string ArgString => "Scope: " + StrOf(Scope) + ", " + "Predicate: " + StrOf(Predicate);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.GroupAny);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Scope = (ExecutionScope)codes[i++];
            Predicate = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add((uint)Scope);
            code.Add(Predicate.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Predicate;
            }
        }
        #endregion
    }
}
