using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv;
using SpirvNet.Validation;

namespace SpirvNet.Interpreter
{
    /// <summary>
    /// An execution machine for SPIR-V modules
    /// </summary>
    public class Machine
    {
        /// <summary>
        /// Currently loaded (validated) module
        /// </summary>
        public readonly ValidatedModule Module;

        /// <summary>
        /// Array of objects (and current values)
        /// Index = SSA location
        /// </summary>
        private readonly object[] values;

        public Machine(ValidatedModule module)
        {
            Module = module;

            values = new object[Module.OriginalModule.Bound];
        }
    }
}
