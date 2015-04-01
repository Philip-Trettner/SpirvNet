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
    /// OpGroupSMax
    /// 
    /// A signed integer maximum group operation specified for all values of X specified by work-items in the group.
    /// 
    /// X and Result Type must be a 32 or 64 bits wide OpTypeInt data type.
    /// 
    /// Scope must be the Workgroup or Subgroup Execution Scope.
    /// 
    /// The identity I is INT_MIN when X is 32 bits wide and LONG_MIN when X is 64 bits wide.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpGroupSMax : GroupInstruction
    {
        public override bool IsGroup => true;
        public override OpCode OpCode => OpCode.GroupSMax;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ExecutionScope Scope;
        public GroupOperation Operation;
        public ID X;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Scope) + ", " + StrOf(Operation) + ", " + StrOf(X) + ")";
        public override string ArgString => "Scope: " + StrOf(Scope) + ", " + "Operation: " + StrOf(Operation) + ", " + "X: " + StrOf(X);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.GroupSMax);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Scope = (ExecutionScope)codes[i++];
            Operation = (GroupOperation)codes[i++];
            X = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add((uint)Scope);
            code.Add((uint)Operation);
            code.Add(X.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return X;
            }
        }
        #endregion
    }
}
