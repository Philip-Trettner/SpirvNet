using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;

namespace SpirvNet.DotNet.SSA
{
    /// <summary>
    /// A builder for types
    /// </summary>
    class TypeBuilder
    {
        /// <summary>
        /// Mapping from .NET type to SPIR-V type
        /// </summary>
        private readonly Dictionary<TypeReference, SpirvType> cilToSpirv = new Dictionary<TypeReference, SpirvType>();

        /// <summary>
        /// Creates (or returns cached) SPIR-V type
        /// </summary>
        public SpirvType Create(TypeReference type)
        {
            if (cilToSpirv.ContainsKey(type))
                return cilToSpirv[type];

            // TODO create type

            var spirvType = new SpirvType(type);
            cilToSpirv.Add(type, spirvType);
            return spirvType;
        }
    }
}
