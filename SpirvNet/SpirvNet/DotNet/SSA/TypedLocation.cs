using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv;

namespace SpirvNet.DotNet.SSA
{
    /// <summary>
    /// A SSA location with a type
    /// </summary>
    class TypedLocation
    {
        /// <summary>
        /// Hosted type
        /// </summary>
        public readonly SpirvType Type;

        /// <summary>
        /// SSA ID
        /// </summary>
        public ID ID { get; set; }

        public TypedLocation(SpirvType type, IDAllocator allocator)
        {
            Type = type;
            ID = allocator.CreateID();
        }
        
        public TypedLocation(ID id, Type type, TypeBuilder builder)
        {
            ID = id;
            Type = builder.Create(type);
        }
    }
}
