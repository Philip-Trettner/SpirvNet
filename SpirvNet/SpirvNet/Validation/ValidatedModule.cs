using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv;

namespace SpirvNet.Validation
{
    /// <summary>
    /// A validated and analysed version of a module
    /// </summary>
    public class ValidatedModule
    {
        /// <summary>
        /// Reference to original module
        /// </summary>
        public readonly Module OriginalModule;
        
        private ValidatedModule(Module originalModule)
        {
            OriginalModule = originalModule;
        }

        /// <summary>
        /// Analyses and validates a module
        /// </summary>
        private void Analyse()
        {
            // TODO
        }

        /// <summary>
        /// Creates a validated version of the module (or throws a ValidationException)
        /// </summary>
        public static ValidatedModule Validate(Module module)
        {
            var vmod = new ValidatedModule(module);
            vmod.Analyse();
            return vmod;
        }
    }
}
