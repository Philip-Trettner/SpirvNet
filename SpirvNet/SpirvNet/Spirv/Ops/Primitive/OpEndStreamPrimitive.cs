using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Primitive
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    [DependsOn(LanguageCapability.Geom)]
    public sealed class OpEndStreamPrimitive : Instruction
    {
        public override bool IsPrimitive => true;
        public override OpCode OpCode => OpCode.EndStreamPrimitive;

        public ID Stream;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + Stream + ')';

        public override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.EndStreamPrimitive);
            var i = 1;
            Stream = new ID(codes[start + i++]);
        }

        public override void WriteCode(List<uint> code)
        {
            code.Add(Stream.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Stream;
            }
        }
    }
}
