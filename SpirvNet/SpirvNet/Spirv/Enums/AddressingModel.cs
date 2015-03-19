namespace SpirvNet.Spirv.Enums
{
    /// <summary>
    /// Used by OpMemoryModel
    /// </summary>
    public enum AddressingModel
    {
        Logical = 0,
        /// <summary>
        /// Indicates a 32-bit module, where the address width is equal to 32 bits.
        /// </summary>
        [DependsOn(LanguageCapability.Addr)]
        Physical32 = 1,
        /// <summary>
        /// Indicates a 64-bit module, where the address width is equal to 64 bits.
        /// </summary>
        [DependsOn(LanguageCapability.Addr)]
        Physical64 = 2
    }
}