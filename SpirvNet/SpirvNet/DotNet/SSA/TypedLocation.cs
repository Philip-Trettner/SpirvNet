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
    public class TypedLocation
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

        private TypedLocation(ID id, SpirvType type)
        {
            ID = id;
            Type = type;
        }

        /// <summary>
        /// Returns a special invalid location for "this" parameter
        /// </summary>
        public static TypedLocation SpecialThis => new TypedLocation(Spirv.ID.Invalid, new SpirvType(Spirv.ID.Invalid, SpirvTypeEnum.SpecialThis));

        public override string ToString() => string.Format("({0}, {1})", ID, Type);
    }
}
