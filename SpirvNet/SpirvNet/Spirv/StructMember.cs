using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpirvNet.Spirv
{
    /// <summary>
    /// A member of a SPIR-V struct
    /// </summary>
    public class StructMember
    {
        /// <summary>
        /// Member index
        /// </summary>
        public readonly int Index;
        /// <summary>
        /// Member name
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// Member type
        /// </summary>
        public readonly SpirvType Type;

        public StructMember(int index, string name, SpirvType type)
        {
            Index = index;
            Name = name;
            Type = type;
        }

        public override string ToString() => string.Format("{0}:{1}:{2}", Index, Name, Type);
    }
}
