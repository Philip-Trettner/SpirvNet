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

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + ResultType + ", " + Result + ", " + SourcePointer + ", " + StorageClass + ')';
    }
}
