using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Validation;

namespace SpirvNet.GLSL
{
    /// <summary>
    /// Builder for a shader function
    /// </summary>
    class ShaderFuncBuilder
    {
        /// <summary>
        /// The function in questions
        /// </summary>
        public readonly ValidatedFunction Function;

        public ShaderFuncBuilder(ValidatedFunction function)
        {
            Function = function;
        }
    }
}
