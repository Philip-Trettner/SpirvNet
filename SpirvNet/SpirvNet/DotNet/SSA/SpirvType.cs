using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;

namespace SpirvNet.DotNet.SSA
{
    /// <summary>
    /// A SPIR-V type
    /// </summary>
    class SpirvType
    {
        /// <summary>
        /// Represented type
        /// </summary>
        public readonly TypeReference RepresentedType;

        // TODO: SPIRV decl

        public SpirvType(TypeReference representedType)
        {
            RepresentedType = representedType;
        }
    }
}
