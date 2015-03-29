using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Mono.Cecil;
using SpirvNet.DotNet;
using SpirvNet.Spirv.Ops.ConstantCreation;
using SpirvNet.Spirv.Ops.TypeDeclaration;

namespace SpirvNet.Spirv
{
    /// <summary>
    /// A builder for types
    /// </summary>
    public class TypeBuilder
    {
        /// <summary>
        /// ID allocator
        /// </summary>
        private readonly IDAllocator allocator;

        /// <summary>
        /// Mapping from .NET type to SPIR-V type
        /// </summary>
        private readonly Dictionary<string, SpirvType> cilToSpirv = new Dictionary<string, SpirvType>();

        /// <summary>
        /// Type to type ref mapping
        /// </summary>
        private readonly Dictionary<Type, TypeReference> typeToRef = new Dictionary<Type, TypeReference>();

        /// <summary>
        /// List of constants
        /// </summary>
        public IEnumerable<ConstantCreationInstruction> Constants => cachedConstants.Values;

        private readonly Dictionary<string, ConstantCreationInstruction> cachedConstants = new Dictionary<string, ConstantCreationInstruction>();

        public TypeBuilder(IDAllocator allocator)
        {
            this.allocator = allocator;
        }


        public ID ConstantTrue() => ConstantBool(true);
        public ID ConstantFalse() => ConstantBool(false);
        public ID ConstantInt32(int val) => Constant(val.GetType(), val.ToString(), LiteralNumber.ArrayFor(val));
        public ID ConstantUInt32(uint val) => Constant(val.GetType(), val.ToString(), LiteralNumber.ArrayFor(val));
        public ID ConstantInt64(long val) => Constant(val.GetType(), val.ToString(), LiteralNumber.ArrayFor(val));
        public ID ConstantUInt64(ulong val) => Constant(val.GetType(), val.ToString(), LiteralNumber.ArrayFor(val));
        public ID ConstantFloat32(float val) => Constant(val.GetType(), val.ToString(CultureInfo.InvariantCulture), LiteralNumber.ArrayFor(val));
        public ID ConstantFloat64(double val) => Constant(val.GetType(), val.ToString(CultureInfo.InvariantCulture), LiteralNumber.ArrayFor(val));

        public ID ConstantBool(bool b)
        {
            if (!cachedConstants.ContainsKey(b.ToString()))
                cachedConstants.Add(b.ToString(), b ?
                   (ConstantCreationInstruction)new OpConstantTrue { Result = allocator.CreateID(), ResultType = Create(typeof(bool)).TypeID }
                 : (ConstantCreationInstruction)new OpConstantFalse { Result = allocator.CreateID(), ResultType = Create(typeof(bool)).TypeID });

            return cachedConstants[b.ToString()].ResultID.Value;
        }

        /// <summary>
        /// One-value for a given numerical type
        /// </summary>
        public ID ConstantUnit(SpirvType type)
        {
            switch (type.TypeEnum)
            {
                case SpirvTypeEnum.Boolean:
                    return ConstantTrue();

                case SpirvTypeEnum.Integer:
                    if (type.IsSigned && type.BitWidth == 32)
                        return ConstantInt32(1);

                    if (type.IsSigned && type.BitWidth == 64)
                        return ConstantInt64(1);

                    if (!type.IsSigned && type.BitWidth == 32)
                        return ConstantUInt32(1);

                    if (!type.IsSigned && type.BitWidth == 64)
                        return ConstantUInt64(1);

                    throw new NotSupportedException();

                case SpirvTypeEnum.Floating:
                    if (type.BitWidth == 32)
                        return ConstantFloat32(1);

                    if (type.BitWidth == 64)
                        return ConstantFloat64(1);

                    throw new NotSupportedException();

                default:
                    throw new NotSupportedException();
            }
        }
        /// <summary>
        /// Zero-value for a given numerical type
        /// </summary>
        public ID ConstantZero(SpirvType type)
        {
            switch (type.TypeEnum)
            {
                case SpirvTypeEnum.Boolean:
                    return ConstantFalse();

                case SpirvTypeEnum.Integer:
                    if (type.IsSigned && type.BitWidth == 32)
                        return ConstantInt32(0);

                    if (type.IsSigned && type.BitWidth == 64)
                        return ConstantInt64(0);

                    if (!type.IsSigned && type.BitWidth == 32)
                        return ConstantUInt32(0);

                    if (!type.IsSigned && type.BitWidth == 64)
                        return ConstantUInt64(0);

                    throw new NotSupportedException();

                case SpirvTypeEnum.Floating:
                    if (type.BitWidth == 32)
                        return ConstantFloat32(0);

                    if (type.BitWidth == 64)
                        return ConstantFloat64(0);

                    throw new NotSupportedException();

                case SpirvTypeEnum.Structure:
                    return Constant(type, "NULL", t => new OpConstantComposite { Result = allocator.CreateID(), ResultType = t.TypeID, Constituents = type.Members.Select(m => ConstantZero(m.Type)).ToArray() });

                default:
                    throw new NotSupportedException();
            }
        }

        private ID Constant(Type type, string s, LiteralNumber[] nrs) => Constant(Create(type), s, t => new OpConstant { Result = allocator.CreateID(), ResultType = t.TypeID, Value = nrs });

        private ID Constant(SpirvType type, string s, Func<SpirvType, ConstantCreationInstruction> op)
        {
            var name = type + ": " + s;
            if (!cachedConstants.ContainsKey(name))
                cachedConstants.Add(name, op(type));
            return cachedConstants[name].ResultID.Value;
        }

        /// <summary>
        /// Creates (or returns cached) SPIR-V type
        /// </summary>
        public SpirvType Create(TypeReference type)
        {
            if (cilToSpirv.ContainsKey(type.FullName))
                return cilToSpirv[type.FullName];

            var spirvType = new SpirvType(type, this, allocator);
            cilToSpirv.Add(type.FullName, spirvType);
            return spirvType;
        }
        /// <summary>
        /// Creates (or returns cached) SPIR-V type
        /// </summary>
        public SpirvType Create(Type type)
        {
            if (typeToRef.ContainsKey(type))
                return Create(typeToRef[type]);

            typeToRef.Add(type, CecilLoader.TypeReferenceFor(type));
            return Create(typeToRef[type]);
        }

        /// <summary>
        /// Creates all type operations
        /// </summary>
        public IEnumerable<TypeDeclarationInstruction> CreateTypeOps()
        {
            foreach (var type in cilToSpirv.Values)
                yield return type.CreateOp();
        }

        /// <summary>
        /// Creates a mapping from type to name
        /// </summary>
        public IEnumerable<KeyValuePair<ID, string>> CreateTypeNames()
        {
            foreach (var spirvType in cilToSpirv.Values)
            {
                if (!string.IsNullOrEmpty(spirvType.DebugName))
                    yield return new KeyValuePair<ID, string>(spirvType.TypeID, spirvType.DebugName);
                if (!string.IsNullOrEmpty(spirvType.OriginalName))
                    yield return new KeyValuePair<ID, string>(spirvType.TypeID, spirvType.OriginalName);
            }
        }

        /// <summary>
        /// Creates a mapping from constant to name
        /// </summary>
        public Dictionary<ID, string> CreateConstantNames()
        {
            var dic = new Dictionary<ID, string>();
            foreach (var cc in cachedConstants)
                dic.Add(cc.Value.ResultID ?? ID.Invalid, "const " + cc.Key);
            return dic;
        }
    }
}
