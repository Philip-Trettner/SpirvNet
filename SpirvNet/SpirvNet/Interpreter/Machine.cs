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

            if (module.Bound > 4194304)
                throw new NotSupportedException("Universal minimum is 4,194,304 locations. Probably a good limit for debug emulation.");

            values = new object[Module.Bound];

            // initialize constants
            foreach (var location in module.Locations)
                if (location.IsConstant)
                    values[location.ID] = location.Constant;
        }

        /// <summary>
        /// Executes a function of this module
        /// </summary>
        public object Execute(ValidatedFunction function, params object[] args)
        {
            if (function.Module != Module)
                throw new NotSupportedException("Foreign functions not allowed");

            var iptr = 0;

            throw new NotImplementedException("TODO");
        }
    }
}
