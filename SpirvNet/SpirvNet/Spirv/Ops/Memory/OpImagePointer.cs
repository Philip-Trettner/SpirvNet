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
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    public sealed class OpImagePointer : Instruction
    {
        public override bool IsMemory => true;
        public override OpCode OpCode => OpCode.ImagePointer;
        public ID ResultType;
        public ID Result;
        public ID Image;
        public ID Coordinate;
        public ID Sample;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + ResultType + ", " + Result + ", " + Image + ", " + Coordinate + ", " + Sample + ')';
    }
}
