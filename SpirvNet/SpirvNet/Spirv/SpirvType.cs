using System;
using Mono.Cecil;
using SpirvNet.Spirv.Ops.TypeDeclaration;

namespace SpirvNet.Spirv
{
    /// <summary>
    /// A SPIR-V type
    /// </summary>
    public class SpirvType
    {
        /// <summary>
        /// ID of the type
        /// </summary>
        public ID TypeID { get; set; }

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
        /// Homogeneous element type (array or vector or matrix)
        /// </summary>
        public readonly SpirvType ElementType;

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

        /// <summary>
        /// Explicit ctor
        /// </summary>
        public SpirvType(ID typeID, SpirvTypeEnum typeEnum, uint bitWidth = 0, uint signedness = 0,
            uint elementCount = 0, SpirvType elementType = null)
        {
            TypeID = typeID;
            TypeEnum = typeEnum;
            BitWidth = bitWidth;
            Signedness = signedness;
            ElementCount = elementCount;
            ElementType = elementType;
        }

        /// <summary>
        /// From .NET ctor
        /// </summary>
        public SpirvType(TypeReference representedType, TypeBuilder builder, IDAllocator allocator)
        {
            RepresentedType = representedType;
            TypeID = allocator.CreateID();

            switch (representedType.FullName)
            {
                case "System.Void":
                    TypeEnum = SpirvTypeEnum.Void;
                    return;

                case "System.Boolean":
                    TypeEnum = SpirvTypeEnum.Boolean;
                    return;

                case "System.Int32":
                    TypeEnum = SpirvTypeEnum.Integer;
                    BitWidth = 32;
                    Signedness = 1;
                    return;
                case "System.UInt32":
                    TypeEnum = SpirvTypeEnum.Integer;
                    BitWidth = 32;
                    Signedness = 0;
                    return;
                case "System.Int64":
                    TypeEnum = SpirvTypeEnum.Integer;
                    BitWidth = 64;
                    Signedness = 1;
                    return;
                case "System.UInt64":
                    TypeEnum = SpirvTypeEnum.Integer;
                    BitWidth = 64;
                    Signedness = 0;
                    return;

                case "System.Single":
                    TypeEnum = SpirvTypeEnum.Floating;
                    BitWidth = 32;
                    return;
                case "System.Double":
                    TypeEnum = SpirvTypeEnum.Floating;
                    BitWidth = 64;
                    return;
            }

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
    }
}
