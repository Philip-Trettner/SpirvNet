using System;
using System.Collections.Generic;
using SpirvNet.Spirv.Enums;

namespace SpirvNet.Helper
{
    public static class Extensions
    {
        /// <summary>
        /// Converts .NET Fullname to Cecil fullname
        /// (Nested classes are A/B instead of A+B)
        /// </summary>
        public static string CecilFullType(this string s)
        {
            return string.IsNullOrEmpty(s) ? "" : s.Replace("+", "/");
        }

        /// <summary>
        /// Returns a random element of a given array
        /// </summary>
        public static object RandomElement(this Array a, Random r)
        {
            if (a == null || a.Length == 0)
                return null;
            return a.GetValue(r.Next(a.Length));
        }

        /// <summary>
        /// Executes selector for each int up to (excluding) i
        /// </summary>
        public static IEnumerable<T> ForUpTo<T>(this int i, Func<int, T> sel)
        {
            for (var k = 0; k < i; ++k)
                yield return sel(k);
        }

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
