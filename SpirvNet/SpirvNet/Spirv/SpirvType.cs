using System;
using Mono.Cecil;
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


        public bool IsVoid => TypeEnum == SpirvTypeEnum.Void;

        public bool IsSigned => Signedness != 0;

        public bool IsBoolean => TypeEnum == SpirvTypeEnum.Boolean;
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
            uint elementCount = 0, SpirvType elementType = null, SpirvType returnType = null, SpirvType[] parameterTypes = null)
        {
            TypeID = typeID;
            TypeEnum = typeEnum;
            BitWidth = bitWidth;
            Signedness = signedness;
            ElementCount = elementCount;
            ElementType = elementType;
            ReturnType = returnType;
            ParameterTypes = parameterTypes;

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
                        throw new NotImplementedException("Structs, vectors, matrices not implemented");

                    case MetadataType.Class:
                        throw new NotSupportedException("Reference Type not supported: " + representedType);

                    default:
                        throw new NotSupportedException("Type not supported: " + representedType);
                }

            DebugName = ToString();
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
                case SpirvTypeEnum.Structure: return "struct{TODO}";
                case SpirvTypeEnum.Function: return "function{TODO}";
                default:
                    throw new ArgumentOutOfRangeException();
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
                case SpirvTypeEnum.Structure: throw new NotImplementedException();
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
