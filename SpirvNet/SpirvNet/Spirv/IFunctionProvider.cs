using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using SpirvNet.DotNet.SSA;

namespace SpirvNet.Spirv
{
    /// <summary>
    /// Interface for a method definition -> function ID,Type provider
    /// </summary>
    public interface IFunctionProvider
    {
        /// <summary>
        /// Returns function ID and function type for a given method
        /// (Method is also added to module)
        /// </summary>
        KeyValuePair<ID, SpirvType> Resolve(MethodDefinition def);
    }
}
