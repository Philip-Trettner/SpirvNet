using SpirvNet.Validation;

namespace SpirvNet.GLSL
{
    /// <summary>
    /// Class for generating shaders from SPIR-V modules
    /// </summary>
    public class ShaderGenerator
    {
        /// <summary>
        /// The underlying module
        /// </summary>
        public readonly ValidatedModule Module;

        public ShaderGenerator(ValidatedModule module)
        {
            Module = module;
        }
    }
}
