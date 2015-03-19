namespace SpirvNet.Spirv.Enums
{
    /// <summary>
    /// Define the filter mode of read image extended instructions.
    /// </summary>
    public enum SamplerFilterMode
    {
        /// <summary>
        /// Use filter nearset mode when performing a read image
        /// operation.
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        Nearest = 16,
        /// <summary>
        /// Use filter linear mode when performing a read image
        /// operation.
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        Linear = 32
    }
}