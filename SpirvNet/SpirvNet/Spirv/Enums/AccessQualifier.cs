namespace SpirvNet.Spirv.Enums
{
    /// <summary>
    /// Defines the access permissions of OpTypeSampler and OpTypePipe object.
    /// Used by OpTypePipe
    /// </summary>
    public enum AccessQualifier
    {
        /// <summary>
        /// A read-only object
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        ReadOnly = 0,
        /// <summary>
        /// A write-only object
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        WriteOnly = 1,
        /// <summary>
        /// A readable and writable object
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        ReadWrite = 2
    }
}