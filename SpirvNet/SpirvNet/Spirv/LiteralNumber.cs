using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpirvNet.Spirv
{
    /// <summary>
    /// A numeric value consuming one or more words. 
    /// When a numeric value is larger than one word, low-order words appear first.
    /// </summary>
    public struct LiteralNumber
    {
        public uint Value;
    }
}
