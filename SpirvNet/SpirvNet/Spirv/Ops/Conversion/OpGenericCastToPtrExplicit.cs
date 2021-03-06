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
    /// OpGenericCastToPtrExplicit
    /// 
    /// Attempts to explicitly convert Source pointer to storage storage-class pointer value. Source pointer must point to Generic. If the cast cast fails, the instruction returns an OpConstantNullPointer in storage Storage Class. 
    /// 
    /// Result Type must be a pointer type pointing to storage Storage Class. storage can be one of the following literal values: WorkgroupLocal, WorkgroupGlobal or Private.
    /// 
    /// Result Type and Source pointer must point to the same type.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpGenericCastToPtrExplicit : ConversionInstruction
    {
        public override bool IsConversion => true;
        public override OpCode OpCode => OpCode.GenericCastToPtrExplicit;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID SourcePointer;
        public StorageClass Storage;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(SourcePointer) + ", " + StrOf(Storage) + ")";
        public override string ArgString => "SourcePointer: " + StrOf(SourcePointer) + ", " + "Storage: " + StrOf(Storage);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.GenericCastToPtrExplicit);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            SourcePointer = new ID(codes[i++]);
            Storage = (StorageClass)codes[i++];
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(SourcePointer.Value);
            code.Add((uint)Storage);
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
