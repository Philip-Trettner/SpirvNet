using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SpirvNet.Spirv
{
    /// <summary>
    /// A SSA-style ID
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ID
    {
        /// <summary>
        /// Numerical value
        /// </summary>
        public readonly uint Value;

        public ID(uint id)
        {
            Value = id;
        }
    }
}
