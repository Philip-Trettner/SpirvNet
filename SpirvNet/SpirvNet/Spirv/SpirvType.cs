using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Mono.Cecil;
using SpirvNet.Helper;
using SpirvNet.Spirv.Ops.TypeDeclaration;

namespace SpirvNet.Spirv
{
    /// <summary>
    /// A SPIR-V type
    /// </summary>
    public sealed class SpirvType
    {
        /// <summary>
        /// ID of the type
        /// </summary>
        public ID TypeID { get; set; }

        /// <summary>
        /// Debug Name
        /// </summary>
        public string DebugName { get; set; }
        /// <summary>
        /// Original type name
        /// </summary>
        public string OriginalName { get; set; }

        /// <summary>
        /// Represented type (can be null)
        /// </summary>
        public readonly TypeReference RepresentedType;

        /// <summary>
        /// Type enum
        /// </summary>
        public readonly SpirvTypeEnum TypeEnum;

        /// <summary>
        /// Numerical bit width (true iff IsNumerical)
        /// </summary>
        public readonly uint BitWidth;

        /// <summary>
        /// Signedness of integer type
        /// </summary>
        public readonly uint Signedness;

        /// <summary>
        /// Number of vector, matrix, array elements
        /// </summary>
        public readonly uint ElementCount;

        /// <summary>
        /// Homogeneous element type (array or vector or matrix or pointer)
        /// </summary>
        public readonly SpirvType ElementType;

        /// <summary>
        /// Return type (valid for functions)
        /// </summary>
        public readonly SpirvType ReturnType;
        /// <summary>
        /// Parameter types (valid for function)
        /// </summary>
        public readonly SpirvType[] ParameterTypes;

        /// <summary>
        /// Struct members
        /// </summary>
        public readonly StructMember[] Members;
        /// <summary>
        /// Member index
        /// </summary>
        public uint MemberIndex(string name) => (uint)Members.First(m => m.Name == name).Index;


        public bool IsVoid => TypeEnum == SpirvTypeEnum.Void;

        public bool IsSigned => Signedness != 0;

        public bool IsBoolean => TypeEnum == SpirvTypeEnum.Boolean;
        public bool IsThis => TypeEnum == SpirvTypeEnum.SpecialThis;
        public bool IsInteger => TypeEnum == SpirvTypeEnum.Integer;
        public bool IsFloating => TypeEnum == SpirvTypeEnum.Floating;
        public bool IsNumerical => IsInteger || IsFloating;

        public bool IsVector => TypeEnum == SpirvTypeEnum.Vector;
        public bool IsMatrix => TypeEnum == SpirvTypeEnum.Matrix;
        public bool IsArray => TypeEnum == SpirvTypeEnum.Array;
        public bool IsStructure => TypeEnum == SpirvTypeEnum.Structure;

        public bool IsAggregate => IsStructure || IsArray;
        public bool IsComposite => IsAggregate || IsMatrix || IsVector;

        public bool IsFunction => TypeEnum == SpirvTypeEnum.Function;

        /// <summary>
        /// Explicit ctor
        /// </summary>
        public SpirvType(ID typeID, SpirvTypeEnum typeEnum, uint bitWidth = 0, uint signedness = 0,
            uint elementCount = 0, SpirvType elementType = null, SpirvType returnType = null, SpirvType[] parameterTypes = null, StructMember[] structMembers = null)
        {
            TypeID = typeID;
            TypeEnum = typeEnum;
            BitWidth = bitWidth;
            Signedness = signedness;
            ElementCount = elementCount;
            ElementType = elementType;
            ReturnType = returnType;
            ParameterTypes = parameterTypes;
            Members = structMembers;

            DebugName = ToString();
        }

        /// <summary>
        /// Returns true iff the given object is an instance of this type
        /// </summary>
        public bool IsInstance(object obj)
        {
            switch (TypeEnum)
            {
                case SpirvTypeEnum.Void:
                    return false;

                case SpirvTypeEnum.Boolean:
                    return obj is Boolean;

                case SpirvTypeEnum.Integer:
                    if (IsSigned && BitWidth == 32)
                        return obj is Int32;
                    if (IsSigned && BitWidth == 64)
                        return obj is Int64;
                    if (!IsSigned && BitWidth == 32)
                        return obj is UInt32;
                    if (!IsSigned && BitWidth == 64)
                        return obj is UInt64;
                    throw new NotSupportedException("Integer with non-32/64 width.");

                case SpirvTypeEnum.Floating:
                    if (BitWidth == 32)
                        return obj is Single;
                    if (BitWidth == 64)
                        return obj is Double;
                    throw new NotSupportedException("FP with non-32/64 width.");

                default:
                    throw new NotImplementedException("Type detection for " + this + " is not implemented yet.");
            }
        }

        /// <summary>
        /// From .NET ctor
        /// </summary>
        public SpirvType(TypeReference representedType, TypeBuilder builder, IDAllocator allocator)
        {
            RepresentedType = representedType;
            TypeID = allocator.CreateID();

            var found = true;
            switch (representedType.FullName)
            {
                case "System.Void":
                    TypeEnum = SpirvTypeEnum.Void;
                    break;

                case "System.Boolean":
                    TypeEnum = SpirvTypeEnum.Boolean;
                    break;

                case "System.Int32":
                    TypeEnum = SpirvTypeEnum.Integer;
                    BitWidth = 32;
                    Signedness = 1;
                    break;
                case "System.UInt32":
                    TypeEnum = SpirvTypeEnum.Integer;
                    BitWidth = 32;
                    Signedness = 0;
                    break;
                case "System.Int64":
                    TypeEnum = SpirvTypeEnum.Integer;
                    BitWidth = 64;
                    Signedness = 1;
                    break;
                case "System.UInt64":
                    TypeEnum = SpirvTypeEnum.Integer;
                    BitWidth = 64;
                    Signedness = 0;
                    break;

                case "System.Single":
                    TypeEnum = SpirvTypeEnum.Floating;
                    BitWidth = 32;
                    break;
                case "System.Double":
                    TypeEnum = SpirvTypeEnum.Floating;
                    BitWidth = 64;
                    break;

                default:
                    found = false;
                    break;
            }

            if (!found)
                switch (representedType.MetadataType)
                {
                    case MetadataType.Void:
                        TypeEnum = SpirvTypeEnum.Void;
                        break;

                    case MetadataType.Boolean:
                        TypeEnum = SpirvTypeEnum.Boolean;
                        break;

                    case MetadataType.Int32:
                        TypeEnum = SpirvTypeEnum.Integer;
                        BitWidth = 32;
                        Signedness = 1;
                        break;
                    case MetadataType.UInt32:
                        TypeEnum = SpirvTypeEnum.Integer;
                        BitWidth = 32;
                        Signedness = 0;
                        break;
                    case MetadataType.Int64:
                        TypeEnum = SpirvTypeEnum.Integer;
                        BitWidth = 64;
                        Signedness = 1;
                        break;
                    case MetadataType.UInt64:
                        TypeEnum = SpirvTypeEnum.Integer;
                        BitWidth = 64;
                        Signedness = 0;
                        break;

                    case MetadataType.Single:
                        TypeEnum = SpirvTypeEnum.Floating;
                        BitWidth = 32;
                        break;
                    case MetadataType.Double:
                        TypeEnum = SpirvTypeEnum.Floating;
                        BitWidth = 64;
                        break;

                    case MetadataType.Array:
                        TypeEnum = SpirvTypeEnum.Array;
                        ElementType = builder.Create(representedType.GetElementType());
                        // TODO: Fixed size vs. dynamic size
                        break;

                    case MetadataType.ValueType:
                        TypeEnum = SpirvTypeEnum.Structure;
                        Members = ((TypeDefinition)representedType).Fields.Select((f, i) => new StructMember(i, f.Name, builder.Create(f.FieldType))).ToArray();
                        break;

                    case MetadataType.Class:
                        throw new NotSupportedException("Reference Type not supported: " + representedType);

                    default:
                        throw new NotSupportedException("Type not supported: " + representedType);
                }

            DebugName = ToString();
            OriginalName = RepresentedType.FullName;
        }

        public override string ToString()
        {
            switch (TypeEnum)
            {
                case SpirvTypeEnum.Void: return "void";
                case SpirvTypeEnum.Boolean: return "bool";
                case SpirvTypeEnum.Integer: return (Signedness == 0 ? "u" : "") + "int" + BitWidth;
                case SpirvTypeEnum.Floating: return "float" + BitWidth;
                case SpirvTypeEnum.Vector: return "vec" + ElementCount + "(" + ElementType + ")";
                case SpirvTypeEnum.Matrix: return "mat" + ElementCount + "(" + ElementType + ")";
                case SpirvTypeEnum.Array: return "array" + ElementCount + "(" + ElementType + ")";
                case SpirvTypeEnum.Structure: return string.Format("struct({0})", Members.Aggregated(", "));
                case SpirvTypeEnum.Function: return string.Format("function({0})->{1}", ParameterTypes.Aggregated(", "), ReturnType);
                case SpirvTypeEnum.SpecialThis: return "this";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Prefix for glsl vec/mat types
        /// </summary>
        public string GlslPrefix
        {
            get
            {
                switch (TypeEnum)
                {
                    case SpirvTypeEnum.Boolean: return "b";
                    case SpirvTypeEnum.Integer: return "i";
                    case SpirvTypeEnum.Floating: return BitWidth == 32 ? "" : "d";
                    case SpirvTypeEnum.Vector:
                        return ElementType.GlslPrefix;
                    default:
                        throw new NotSupportedException("Unsupported base type " + this);
                }
            }
        }

        /// <summary>
        /// GLSL type name
        /// </summary>
        public string GlslType
        {
            get
            {
                if (IsInteger && BitWidth == 64)
                    throw new NotSupportedException("GLSL does not support 64bit integer.");

                switch (TypeEnum)
                {
                    case SpirvTypeEnum.Void: return "void";
                    case SpirvTypeEnum.Boolean: return "bool";
                    case SpirvTypeEnum.Integer: return (Signedness == 0 ? "u" : "") + "int";
                    case SpirvTypeEnum.Floating: return BitWidth == 32 ? "float" : "double";
                    case SpirvTypeEnum.Vector: return ElementType.GlslPrefix + "vec" + ElementCount;
                    case SpirvTypeEnum.Matrix: return ElementType.GlslPrefix + "mat" + ElementCount + (ElementCount == ElementType.ElementCount ? "" : "x" + ElementType.ElementCount);
                    case SpirvTypeEnum.Array: return ElementType.GlslType + "[" + ElementCount + "]";
                    case SpirvTypeEnum.Structure: return "struct" + TypeID.Value;
                    default:
                        throw new NotSupportedException("Type not supported");
                }
            }
        }

        public TypeDeclarationInstruction CreateOp()
        {
            switch (TypeEnum)
            {
                case SpirvTypeEnum.Void: return new OpTypeVoid { Result = TypeID };
                case SpirvTypeEnum.Boolean: return new OpTypeBool { Result = TypeID };
                case SpirvTypeEnum.Integer: return new OpTypeInt { Result = TypeID, Signedness = { Value = Signedness }, Width = { Value = BitWidth } };
                case SpirvTypeEnum.Floating: return new OpTypeFloat { Result = TypeID, Width = { Value = BitWidth } };
                case SpirvTypeEnum.Vector: return new OpTypeVector { Result = TypeID, ComponentType = ElementType.TypeID, ComponentCount = { Value = ElementCount } };
                case SpirvTypeEnum.Matrix: return new OpTypeMatrix { Result = TypeID, ColumnType = ElementType.TypeID, ColumnCount = { Value = ElementCount } };
                case SpirvTypeEnum.Array: throw new NotImplementedException("Array length is by-ID");
                case SpirvTypeEnum.Structure: return new OpTypeStruct { Result = TypeID, MemberTypes = Members.Select(m => m.Type.TypeID).ToArray() };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// True iff two types are compatible
        /// </summary>
        public static bool Compatible(SpirvType t1, SpirvType t2)
        {
            return t1.ToString() == t2.ToString();
        }
    }
}
