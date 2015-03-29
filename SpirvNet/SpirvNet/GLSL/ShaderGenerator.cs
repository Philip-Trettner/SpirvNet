using System.Collections.Generic;
using System.Linq;
using SpirvNet.Helper;
using SpirvNet.Validation;

namespace SpirvNet.GLSL
{
    /// <summary>
    /// Class for generating GLSL shaders from SPIR-V modules
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

        /// <summary>
        /// GLSL definitions for all defined types (that are not built-in)
        /// </summary>
        public IEnumerable<string> Types
        {
            get
            {
                foreach (var type in Module.Types)
                    if (type.IsStructure)
                    {
                        yield return string.Format("struct {0}", type.GlslType);
                        yield return "{";
                        for (var i = 0; i < type.Members.Length; ++i)
                            yield return string.Format("  {0} m{1};", type.Members[i].Type.GlslType, i);
                        yield return "}";
                        yield return "";
                    }
            }
        }

        /// <summary>
        /// GLSL code for a function
        /// </summary>
        public IEnumerable<string> FunctionCode(ValidatedFunction f)
        {
            yield return string.Format("{0} function{1}({2})", 
                f.ReturnType.GlslType, 
                f.DeclarationLocation.ID, 
                f.ParameterTypes.Select((p, i) => p.GlslType + " _" + i).Aggregated(", "));
            yield return "{";
            // TODO: instructions
            yield return "}";
            yield return "";
        }
    }
}
