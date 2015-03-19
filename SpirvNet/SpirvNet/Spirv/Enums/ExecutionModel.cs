using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpirvNet.Spirv.Enums
{
    /// <summary>
    /// Used by OpEntryPoint
    /// </summary>
    public enum ExecutionModel
    {
        /// <summary>
        /// Vertex shading stage
        /// </summary>
        [DependsOn(LanguageCapability.Shader)]
        Vertex = 0,
        /// <summary>
        /// Tessellation control (or hull) shading stage
        /// </summary>
        [DependsOn(LanguageCapability.Tess)]
        TessellationControl = 1,
        /// <summary>
        /// Tessellation evaluation (or domain) shading stage
        /// </summary>
        [DependsOn(LanguageCapability.Tess)]
        TessellationEvaluation = 2,
        /// <summary>
        /// Geometry shading stage
        /// </summary>
        [DependsOn(LanguageCapability.Geom)]
        Geometry = 3,
        /// <summary>
        /// Fragment shading stage
        /// </summary>
        [DependsOn(LanguageCapability.Shader)]
        Fragment = 4,
        /// <summary>
        /// Graphical compute shading stage
        /// </summary>
        [DependsOn(LanguageCapability.Shader)]
        GLCompute = 5,
        /// <summary>
        /// Compute kernel
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        Kernel = 6
    }
}
