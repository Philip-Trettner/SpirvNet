using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpirvNet.Spirv
{
    /// <summary>
    /// An object of type struct
    /// </summary>
    public class SpirvStruct
    {
        /// <summary>
        /// Type
        /// </summary>
        public readonly SpirvType Type;

        /// <summary>
        /// Member objects
        /// </summary>
        public readonly object[] Members;

        public SpirvStruct(SpirvType type, params object[] members)
        {
            Type = type;
            Members = members;
        }
    }
}
