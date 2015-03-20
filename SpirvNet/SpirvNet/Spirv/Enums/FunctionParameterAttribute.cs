namespace SpirvNet.Spirv.Enums
{
    /// <summary>
    /// Adds additional information to the return type and to each parameter of a function
    /// </summary>
    public enum FunctionParameterAttribute
    {
        /// <summary>
        /// Value should be zero extended if needed
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        Zext = 0,
        /// <summary>
        /// Value should be sign extended if needed
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        Sext = 1,
        /// <summary>
        /// This indicates that the pointer parameter should really be
        /// passed by value to the function. Only valid for pointer
        /// parameters (not for ret value)
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        ByVal = 2,
        /// <summary>
        /// Indicates that the pointer parameter specifies the address
        /// of a structure that is the return value of the function in the
        /// source program. Only applicable to the first parameter
        /// which must be a pointer parameters
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        Sret = 3,
        /// <summary>
        /// Indicates that the memory pointed by a pointer parameter
        /// is not accessed via pointer values which are not derived
        /// from this pointer parameter. Only valid for pointer
        /// parameters. Not valid on return values
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        NoAlias = 4,
        /// <summary>
        /// The callee does not make a copy of the pointer parameter
        /// into a location that is accessible after returning from the
        /// callee. Only valid for pointer parameters. Not valid on
        /// return values
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        NoCapture = 5,
        /// <summary>
        /// CL TBD
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        SVM = 6,
        /// <summary>
        /// Can only read the memory pointed by a pointer
        /// parameter. Only valid for pointer parameters. Not valid
        /// on return values
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        NoWrite = 7,
        /// <summary>
        /// Cannot dereference the memory pointed by a pointer
        /// parameter. Only valid for pointer parameters. Not valid
        /// on return values
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        NoReadWrite = 8
    }
}