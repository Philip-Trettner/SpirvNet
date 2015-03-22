using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpirvNet.Spirv
{
    /// <summary>
    /// SSA ID allocator
    /// </summary>
    public class IDAllocator
    {
        /// <summary>
        /// Next usable ID
        /// </summary>
        private uint nextID = 1;

        /// <summary>
        /// Creates a new ID
        /// </summary>
        public ID CreateID()
        {
            return new ID(nextID++);
        }
    }
}
