using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using SpirvNet.DotNet;
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
 
        public TypeBuilder(IDAllocator allocator)
        {
            this.allocator = allocator;
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
    }
}
