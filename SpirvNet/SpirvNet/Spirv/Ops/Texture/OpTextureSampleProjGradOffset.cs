using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Texture
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    [DependsOn(LanguageCapability.Shader)]
    public sealed class OpTextureSampleProjGradOffset : Instruction
    {
        public override bool IsTexture => true;
        public override OpCode OpCode => OpCode.TextureSampleProjGradOffset;
        public ID ResultType;
        public ID Result;
        public ID Sampler;
        public ID Coordinate;
        public ID dx;
        public ID dy;
        public ID Offset;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + ResultType + ", " + Result + ", " + Sampler + ", " + Coordinate + ", " + dx + ", " + dy + ", " + Offset + ')';
    }
}
