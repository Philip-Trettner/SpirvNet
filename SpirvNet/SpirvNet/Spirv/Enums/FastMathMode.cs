namespace SpirvNet.Spirv.Enums
{
    /// <summary>
    /// Enables fast math operations which are otherwise unsafe.
    /// Only valid on OpFAdd, OpFSub, OpFMul, OpFDiv, OpFRem, and OpFMod instructions.
    /// </summary>
    public enum FastMathMode
    {
        /// <summary>
        /// Assume parameters and result are not NaN.
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        NotNaN = 0,
        /// <summary>
        /// Assume parameters and result are not +/- Inf
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        NotInf = 2,
        /// <summary>
        /// Treat the sign of a zero parameter or result as
        /// insignificant
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        NSZ = 4,
        /// <summary>
        /// Allow the usage of reciprocal rather than perform a
        /// division
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        AllowRecip = 8,
        /// <summary>
        /// Allow algebraic transformations according to real-number
        /// associative and distibutive algebra. This flag implies all
        /// the others
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        Fast = 16
    }
}