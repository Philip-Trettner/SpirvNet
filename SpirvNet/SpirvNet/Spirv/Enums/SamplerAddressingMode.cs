namespace SpirvNet.Spirv.Enums
{
    /// <summary>
    /// Define the addressing mode of read image extended instructions.
    /// </summary>
    public enum SamplerAddressingMode
    {
        /// <summary>
        /// The image coordinates used to sample elements of the
        /// image refer to a location inside the image, otherwise the
        /// results are undefined.
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        None = 0,
        /// <summary>
        /// Out-of-range image coordinates are clamped to the extent.
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        ClampEdge = 2,
        /// <summary>
        /// Out-of-range image coordinates will return a border color
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        Clamp = 4,
        /// <summary>
        /// Out-of-range image coordinates are wrapped to the valid
        /// range. Can only be used with normalized coordinates.
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        Repeat = 6,
        /// <summary>
        /// Flip the image coordinate at every integer junction. Can
        /// only be used with normalized coordinates.
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        RepeatMirrored = 8
    }
}