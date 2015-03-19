namespace SpirvNet.Spirv.Enums
{
    /// <summary>
    /// Associate a rounding mode to a floating-point conversion instruction.
    /// By default
    /// - Conversions from floating-point to integer types use the round-toward-zero rounding mode.
    /// - Conversions to floating-point types use the round-to-nearest-even rounding mode.
    /// </summary>
    public enum RoundingMode
    {
        /// <summary>
        /// Round to nearest even.
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        RTE = 0,
        /// <summary>
        /// Round towards zero
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        RTZ = 1,
        /// <summary>
        /// Round towards positive infinity
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        RTP = 2,
        /// <summary>
        /// Round towards negative infinity.
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        RTN = 3
    }
}