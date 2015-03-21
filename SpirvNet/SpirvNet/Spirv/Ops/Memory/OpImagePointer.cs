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

        public override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.ImagePointer);
            var i = 1;
            ResultType = new ID(codes[start + i++]);
            Result = new ID(codes[start + i++]);
            Image = new ID(codes[start + i++]);
            Coordinate = new ID(codes[start + i++]);
            Sample = new ID(codes[start + i++]);
        }

        public override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Image.Value);
            code.Add(Coordinate.Value);
            code.Add(Sample.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Image;
                yield return Coordinate;
                yield return Sample;
            }
        }
    }
}
