namespace SpirvNet.Spirv.Enums
{
    /// <summary>
    /// Dimensionality of a texture. Used by OpTypeSampler.
    /// </summary>
    public enum Dim
    {
        Dim1D = 0,
        Dim2D = 1,
        Dim3D = 2,
        [DependsOn(LanguageCapability.Shader)]
        Cube = 3,
        [DependsOn(LanguageCapability.Shader)]
        Rect = 4,
        Buffer = 5
    }
}