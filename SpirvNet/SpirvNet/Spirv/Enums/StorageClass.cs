namespace SpirvNet.Spirv.Enums
{
    /// <summary>
    /// Class of storage for declared variables (does not include intermediate values). Used by:
    /// - OpTypePointer
    /// - OpVariable
    /// - OpVariableArray
    /// - OpGenericCastToPtrExplicit
    /// </summary>
    public enum StorageClass
    {
        /// <summary>
        /// Shared externally, read-only memory, visible across all
        /// instantiations or work groups. Graphics uniform memory.
        /// OpenCL Constant memory
        /// </summary>
        UniformConstant = 0,
        /// <summary>
        /// Input from pipeline. Read only.
        /// </summary>
        [DependsOn(LanguageCapability.Shader)]
        Input = 1,
        /// <summary>
        /// Shared externally, visible across all instantiations or work
        /// groups
        /// </summary>
        [DependsOn(LanguageCapability.Shader)]
        Uniform = 2,
        /// <summary>
        /// Output to pipeline.
        /// </summary>
        [DependsOn(LanguageCapability.Shader)]
        Output = 3,
        /// <summary>
        /// Shared across all work items within a work group.
        /// OpenGL "shared". OpenCL local memory.
        /// </summary>
        WorkgroupLocal = 4,
        /// <summary>
        /// Visible across all work items of all work groups. OpenCL
        /// global memory
        /// </summary>
        WorkgroupGlobal = 5,
        /// <summary>
        /// Accessible across functions within a module, non-IO (not
        /// visible outside the module)
        /// </summary>
        [DependsOn(LanguageCapability.Shader)]
        PrivateGlobal = 6,
        /// <summary>
        /// A variable local to a function
        /// </summary>
        [DependsOn(LanguageCapability.Shader)]
        Function = 7,
        /// <summary>
        /// A generic pointer, which overloads StoragePrivate,
        /// StorageLocal, StorageGlobal. not a real storage class
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        Generic = 8,
        /// <summary>
        /// Private to a work-item and is not visible to another
        /// work-item. OpenCL private memory
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        Private = 9,
        /// <summary>
        /// For holding atomic counters
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        AtomicCounter = 10
    }
}