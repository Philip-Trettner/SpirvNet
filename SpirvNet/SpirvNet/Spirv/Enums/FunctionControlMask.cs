using System;

namespace SpirvNet.Spirv.Enums
{
    /// <summary>
    /// Used by OpFunction.
    /// </summary>
    [Flags]
    public enum FunctionControlMask
    {
        None = 0,
        /// <summary>
        /// Strong request, to the extent possible, to inline the function
        /// </summary>
        InLine = 1,
        /// <summary>
        /// Strong request, to the extent possible, to not inline the
        /// function
        /// </summary>
        DontInline = 2,
        /// <summary>
        /// Compiler can assume this function has no side effect, but
        /// might read global memory or read through dereferenced
        /// function parameters. Always computes the same result for
        /// the same argument values
        /// </summary>
        Pure = 4,
        /// <summary>
        /// Compiler can assume this function has no side effects, and
        /// will not access global memory or dereference function
        /// parameters. Always computes the same result for the same
        /// argument values
        /// </summary>
        Const = 8
    }
}