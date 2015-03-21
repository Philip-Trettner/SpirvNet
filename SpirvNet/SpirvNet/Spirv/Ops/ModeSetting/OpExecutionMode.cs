using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.ModeSetting
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    public sealed class OpExecutionMode : Instruction
    {
        public override bool IsModeSetting => true;
        public override OpCode OpCode => OpCode.ExecutionMode;

        public ID EntryPoint;
        public ExecutionMode ExecutionMode;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + EntryPoint + ", " + ExecutionMode + ')';

        public override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.ExecutionMode);
            var i = 1;
            EntryPoint = new ID(codes[start + i++]);
            ExecutionMode = (ExecutionMode)codes[start + i++];
        }

        public override void WriteCode(List<uint> code)
        {
            code.Add(EntryPoint.Value);
            code.Add((uint)ExecutionMode);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return EntryPoint;
            }
        }
    }
}
