namespace SpirvNet.Spirv.Enums
{
    /// <summary>
    /// Used by OpSelectionMerge
    /// </summary>
    public enum SelectionControl
    {
        /// <summary>
        /// No control requested
        /// </summary>
        NoControl = 0,
        /// <summary>
        /// Strong request, to the extent possible, to remove the flow
        /// control for this selection
        /// </summary>
        Flatten = 1,
        /// <summary>
        /// Strong request, to the extent possible, to keep this selection
        /// as flow control
        /// </summary>
        DontFlatten = 2
    }
}