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
    public sealed class OpTextureQueryLevels : Instruction
    {
        public override bool IsTexture => true;
        public override OpCode OpCode => OpCode.TextureQueryLevels;
        public ID ResultType;
        public ID Result;
        public ID Sampler;
    }
}