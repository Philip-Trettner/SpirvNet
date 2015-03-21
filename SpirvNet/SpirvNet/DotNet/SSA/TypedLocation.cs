using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public readonly uint ID;

        public TypedLocation(SpirvType type, uint id)
        {
            Type = type;
            ID = id;
        }
    }
}
