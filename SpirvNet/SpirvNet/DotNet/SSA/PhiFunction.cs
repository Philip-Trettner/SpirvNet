using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpirvNet.DotNet.SSA
{
    /// <summary>
    /// Phi function merging a set of locations into another one
    /// </summary>
    public class PhiFunction
    {
        /// <summary>
        /// Target location
        /// </summary>
        public readonly TypedLocation Target;

        /// <summary>
        /// Source locations
        /// </summary>
        public readonly TypedLocation[] Sources;

        public PhiFunction(TypedLocation target, params TypedLocation[] sources)
        {
            Target = target;
            Sources = sources;
        }
    }
}
