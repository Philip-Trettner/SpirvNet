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
    public sealed class OpUConvert : Instruction
    {
        public override bool IsConversion => true;
        public override OpCode OpCode => OpCode.UConvert;

        public ID ResultType;
        public ID Result;
        public ID UnsignedValue;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + ResultType + ", " + Result + ", " + UnsignedValue + ')';

        public override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.UConvert);
            var i = 1;
            ResultType = new ID(codes[start + i++]);
            Result = new ID(codes[start + i++]);
            UnsignedValue = new ID(codes[start + i++]);
        }

        public override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(UnsignedValue.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return UnsignedValue;
            }
        }
    }
}
