using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Extension
{
    /// <summary>
    /// OpExtInst
    /// 
    /// Execute an instruction in an imported set of extended instructions.
    /// 
    /// Set is the result of an OpExtInstImport instruction.
    /// 
    /// Instruction is the enumerant of the instruction to execute within the extended instruction Set.
    /// 
    /// Operand 1, &#8230; are the operands to the extended instruction.
    /// </summary>
    public sealed class OpExtInst : ExtensionInstruction
    {
        public override bool IsExtension => true;
        public override OpCode OpCode => OpCode.ExtInst;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Set;
        public LiteralNumber Instruction;
        public ID[] Operands = { };

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Set) + ", " + StrOf(Instruction) + ", " + StrOf(Operands) + ")";
        public override string ArgString => "Set: " + StrOf(Set) + ", " + "Instruction: " + StrOf(Instruction) + ", " + "Operands: " + StrOf(Operands);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.ExtInst);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Set = new ID(codes[i++]);
            Instruction = new LiteralNumber(codes[i++]);
            var length = WordCount - (i - start);
            Operands = new ID[length];
            for (var k = 0; k < length; ++k)
                Operands[k] = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Set.Value);
            code.Add(Instruction.Value);
            if (Operands != null)
                foreach (var val in Operands)
                    code.Add(val.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Set;
                if (Operands != null)
                    foreach (var id in Operands)
                        yield return id;
            }
        }
        #endregion
    }
}
