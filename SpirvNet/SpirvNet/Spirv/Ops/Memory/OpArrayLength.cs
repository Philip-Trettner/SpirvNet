using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Memory
{
    /// <summary>
    /// OpArrayLength
    /// 
    /// Result is the array length of a run-time array.
    /// 
    /// Structure must be an object of type OpTypeStruct that contains a member that is a run-time array.
    /// 
    /// Array member is a member number of Structure that must have a type from OpTypeRuntimeArray.
    /// </summary>
    [DependsOn(LanguageCapability.Shader)]
    public sealed class OpArrayLength : MemoryInstruction
    {
        public override bool IsMemory => true;
        public override OpCode OpCode => OpCode.ArrayLength;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Structure;
        public LiteralNumber ArrayMember;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Structure) + ", " + StrOf(ArrayMember) + ")";
        public override string ArgString => "Structure: " + StrOf(Structure) + ", " + "ArrayMember: " + StrOf(ArrayMember);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.ArrayLength);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Structure = new ID(codes[i++]);
            ArrayMember = new LiteralNumber(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Structure.Value);
            code.Add(ArrayMember.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Structure;
            }
        }
        #endregion
    }
}
