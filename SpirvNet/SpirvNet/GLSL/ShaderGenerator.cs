using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using DebugPage;
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
                        yield return "// type: " + type;
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
            var name = f.IsEntryPoint ? "main" : "function" + f.DeclarationLocation.ID;

            yield return "// function: " + Module.IDStr(f.DeclarationLocation.LocationID);
            yield return string.Format("{0} {1}({2})", 
                f.ReturnType.GlslType, 
                name, 
                f.ParameterTypes.Select((p, i) => p.GlslType + " _" + i).Aggregated(", "));
            yield return "{";
            // TODO: instructions
            yield return "}";
            yield return "";
        }

        /// <summary>
        /// Total shader file code
        /// </summary>
        public IEnumerable<string> ShaderCode
        {
            get
            {
                yield return "#version 450 core";
                yield return "";

                yield return "//////////////////";
                yield return "// Types";
                yield return "";
                foreach (var line in Types)
                    yield return line;

                yield return "//////////////////";
                yield return "// Functions";
                yield return "";
                // TODO: ensure ordering
                foreach (var function in Module.Functions)
                    foreach (var line in FunctionCode(function))
                        yield return line;
            }
        }

        /// <summary>
        /// Adds debug information to a file
        /// </summary>
        public void AddDebugPageTo(PageElement e)
        {
            e.AddContent("Shader File", "h3");
            e.AddCode(ShaderCode.Aggregated("\n"), "c");
        }
    }
}
