using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Composite
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    public sealed class OpCompositeInsert : Instruction
    {
        public override bool IsComposite => true;
        public override OpCode OpCode => OpCode.CompositeInsert;

        public ID ResultType;
        public ID Result;
        public ID Object;
        public ID Composite;
        public ID[] Indexes;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + ResultType + ", " + Result + ", " + Object + ", " + Composite + ", " + Indexes + ')';

        public override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.CompositeInsert);
            var i = 1;
            ResultType = new ID(codes[start + i++]);
            Result = new ID(codes[start + i++]);
            Object = new ID(codes[start + i++]);
            Composite = new ID(codes[start + i++]);
            var length = WordCount - i + 1;
            Indexes = new ID[length];
            for (var k = 0; k < length; ++k)
                Indexes[k] = new ID(codes[start + i++]);
        }

        public override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Object.Value);
            code.Add(Composite.Value);
            foreach (var val in Indexes)
                code.Add(val.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Object;
                yield return Composite;
                if (Indexes != null)
                    foreach (var id in Indexes)
                        yield return id;
            }
        }
    }
}
