namespace SpirvNet.Spirv.Enums
{
    /// <summary>
    /// Used by OpMemoryModel
    /// </summary>
    public enum MemoryModel
    {
        /// <summary>
        /// No shared memory consistency issues
        /// </summary>
        [DependsOn(LanguageCapability.Shader)]
        Simple = 0,
        /// <summary>
        /// Memory model needed by later versions of GLSL and ESSL.
        /// Works across multiple versions
        /// </summary>
        [DependsOn(LanguageCapability.Shader)]
        GLSL450 = 1,
        /// <summary>
        /// OpenCL 1.2 memory model
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        OpenCL1_2 = 2,
        /// <summary>
        /// OpenCL 2.0 memory model
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        OpenCL2_0 = 3,
        /// <summary>
        /// OpenCL 2.1 memory model
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        OpenCL2_1 = 4
    }
}