using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Conversion
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpGenericCastToPtrExplicit : Instruction
    {
        public override bool IsConversion => true;
        public override OpCode OpCode => OpCode.GenericCastToPtrExplicit;

        public ID ResultType;
        public ID Result;
        public ID SourcePointer;
        public StorageClass StorageClass;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(SourcePointer) + ", " + StrOf(StorageClass) + ")";

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.GenericCastToPtrExplicit);
            var i = 1;
            ResultType = new ID(codes[start + i++]);
            Result = new ID(codes[start + i++]);
            SourcePointer = new ID(codes[start + i++]);
            StorageClass = (StorageClass)codes[start + i++];
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(SourcePointer.Value);
            code.Add((uint)StorageClass);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return SourcePointer;
            }
        }
        #endregion
    }
}
