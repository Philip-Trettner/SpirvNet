using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Function
{
    /// <summary>
    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf
    /// </summary>
    public sealed class OpFunctionCall : Instruction
    {
        public override bool IsFunction => true;
        public override OpCode OpCode => OpCode.FunctionCall;

        public ID ResultType;
        public ID Result;
        public ID Function;
        public ID[] Arguments;

        public override string ToString() => '(' + OpCode + '(' + (int)OpCode + ")" + ", " + ResultType + ", " + Result + ", " + Function + ", " + Arguments + ')';

        public override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.FunctionCall);
            var i = 1;
            ResultType = new ID(codes[start + i++]);
            Result = new ID(codes[start + i++]);
            Function = new ID(codes[start + i++]);
            var length = WordCount - i + 1;
            Arguments = new ID[length];
            for (var k = 0; k < length; ++k)
                Arguments[k] = new ID(codes[start + i++]);
        }

        public override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Function.Value);
            foreach (var val in Arguments)
                code.Add(val.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Function;
                if (Arguments != null)
                    foreach (var id in Arguments)
                        yield return id;
            }
        }
    }
}
