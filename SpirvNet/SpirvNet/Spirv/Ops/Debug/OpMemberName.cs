using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

namespace SpirvNet.Spirv.Ops.Debug
{
    /// <summary>
    /// Name a member of a structure type. This has no semantic impact and can safely be removed from a module.
    /// Type is the ID from an OpTypeStruct instruction.
    /// Member is the number of the member to name in the structure.The first member is member 0, the next is member 1, . . .
    /// Name is the string to name the member with
    /// </summary>
    public sealed class OpMemberName : Instruction
    {
        public override OpCode OpCode => OpCode.MemberName;
        public ID Type;
        public LiteralNumber Member;
        public LiteralString Name;
    }
}
