namespace SpirvNet.Spirv.Enums
{
    /// <summary>
    /// Associate a linkage type to functions or global variables. By default, functions and global variables are private to a module and
    /// cannot be accessed by other modules
    /// </summary>
    public enum LinkageType
    {
        /// <summary>
        /// Accessible by other modules as well
        /// </summary>
        [DependsOn(LanguageCapability.Link)]
        Export = 0,
        /// <summary>
        /// A declaration for a global identifier that exists in another
        /// module
        /// </summary>
        [DependsOn(LanguageCapability.Link)]
        Import = 1
    }
}