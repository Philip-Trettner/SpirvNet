namespace SpirvNet.Spirv.Enums
{
    /// <summary>
    /// Used by OpLoopMerge.
    /// </summary>
    public enum LoopControl
    {
        /// <summary>
        /// No control requested.
        /// </summary>
        NoControl = 0,
        /// <summary>
        /// Strong request, to the extent possible, to unroll or unwind
        /// this loop
        /// </summary>
        Unroll = 1,
        /// <summary>
        /// Strong request, to the extent possible, to keep this loop as a
        /// loop, without unrolling.
        /// </summary>
        DontUnroll = 2
    }
}