using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv;
using SpirvNet.Validation;

namespace SpirvNet.GLSL
{
    /// <summary>
    /// Statement base class
    /// </summary>
    class Statement
    {
        /// <summary>
        /// GLSL shader code lines
        /// </summary>
        public virtual IEnumerable<string> CodeLines { get { yield break; } }
        
        /// <summary>
        /// Variable name for a given ID
        /// </summary>
        public string VarName(ID id) => "v" + id.Value;
    }
}
