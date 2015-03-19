using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv;
using SpirvNet.Spirv.Enums;

namespace SpirvNet
{
    public static class Extensions
    {
        /// <summary>
        /// Returns true iff code is a removable (debug) instruction
        /// </summary>
        public static bool IsDebug(this OpCode code)
        {
            switch (code)
            {
                case OpCode.Source:
                case OpCode.SourceExtension:
                case OpCode.Name:
                case OpCode.MemberName:
                case OpCode.String:
                case OpCode.Line:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns true iff code is a branch instruction
        /// </summary>
        public static bool IsBranch(this OpCode code)
        {
            switch (code)
            {
                case OpCode.Branch:
                case OpCode.BranchConditional:
                case OpCode.Switch:
                case OpCode.Kill:
                case OpCode.Return:
                case OpCode.ReturnValue:
                case OpCode.Unreachable:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns true iff code is a type decl
        /// </summary>
        public static bool IsTypeDeclaration(this OpCode code)
        {
            switch (code)
            {
                case OpCode.TypeArray:
                case OpCode.TypeBool:
                case OpCode.TypeDeviceEvent:
                case OpCode.TypeEvent:
                case OpCode.TypeFilter:
                case OpCode.TypeFloat:
                case OpCode.TypeFunction:
                case OpCode.TypeInt:
                case OpCode.TypeMatrix:
                case OpCode.TypeOpaque:
                case OpCode.TypePipe:
                case OpCode.TypePointer:
                case OpCode.TypeQueue:
                case OpCode.TypeReserveId:
                case OpCode.TypeRuntimeArray:
                case OpCode.TypeSampler:
                case OpCode.TypeStruct:
                case OpCode.TypeVector:
                case OpCode.TypeVoid:
                case OpCode.Constant:
                case OpCode.ConstantComposite:
                case OpCode.ConstantFalse:
                case OpCode.ConstantNullObject:
                case OpCode.ConstantNullPointer:
                case OpCode.ConstantSampler:
                case OpCode.ConstantTrue:
                case OpCode.Variable:
                    return true;
                default:
                    return false;
            }
        }
        
        /// <summary>
        /// Returns true iff opcode is a decoration
        /// </summary>
        public static bool IsAnnotation(this OpCode code)
        {
            switch (code)
            {
                case OpCode.Decorate:
                case OpCode.GroupDecorate:
                case OpCode.MemberDecorate:
                case OpCode.DecorationGroup:
                case OpCode.GroupMemberDecorate:
                    return true;
                default:
                    return false;
            }
        }
    }
}
