using System;

namespace SpirvNet.Spirv.Enums
{
    /// <summary>
    /// Capability requirements for a module are declared early in a module.
    /// - A validator can validate that the module uses only declared capabilities.
    /// - An implementation may reject modules requesting capabilities it does not support.
    /// </summary>
    [Flags]
    public enum LanguageCapability
    {
        None = 0x0,
        /// <summary>
        /// Native matrices
        /// </summary>
        Matrix = 0x1,
        /// <summary>
        /// Shaders for vertex, fragment, and compute stages
        /// </summary>
        [DependsOn(Matrix)]
        Shader = 0x2,
        /// <summary>
        /// Geometry shaders
        /// </summary>
        [DependsOn(Shader)]
        Geom = 0x4,
        /// <summary>
        /// Tessellation shaders
        /// </summary>
        [DependsOn(Shader)]
        Tess = 0x8,
        /// <summary>
        /// Physical addressing, to allow non-logical addressing modes
        /// </summary>
        Addr = 0x10,
        /// <summary>
        /// Ability to have partially linked modules and libraries
        /// </summary>
        Link = 0x20,
        /// <summary>
        /// Kernels for OpenCL
        /// </summary>
        Kernel = 0x40
    }
}