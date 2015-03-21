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
    public sealed class OpVariable : Instruction
    {
        public override bool IsMemory => true;
        public override OpCode OpCode => OpCode.Variable;

        public ID ResultType;
        public ID Result;
        public StorageClass StorageClass;
        public ID? Initializer;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + ResultType + ", " + Result + ", " + StorageClass + ", " + Initializer + ')';

        public override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Variable);
            var i = 1;
            ResultType = new ID(codes[start + i++]);
            Result = new ID(codes[start + i++]);
            StorageClass = (StorageClass)codes[start + i++];
            if (i < WordCount)
                Initializer = new ID(codes[start + i++]);
        }

        public override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add((uint)StorageClass);
            if (Initializer.HasValue)
                code.Add(Initializer.Value.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                if (Initializer.HasValue)
                    yield return Initializer.Value;
            }
        }
    }
}
