namespace SpirvNet.Spirv.Enums
{
    /// <summary>
    /// Declare the modes this module's stage will execute in. Used by OpExecutionMode.
    /// </summary>
    public enum ExecutionMode
    {
        /// <summary>
        /// Number of times to invoke the geometry stage for
        /// each input primitive received. The default is to run
        /// once for each input primitive. If greater than the
        /// target-dependent maximum, it will fail to compile.
        /// Only valid with the Geometry Execution Model
        /// </summary>
        [ExtraOperand(OperandType.LiteralNumber, "Number of invocations")]
        [DependsOn(LanguageCapability.Geom)]
        Invocations = 0,
        /// <summary>
        /// Requests the tessellation primitive generator to divide
        /// edges into a collection of equal-sized segments. Only
        /// valid with one of the tessellation Execution Models.
        /// </summary>
        [DependsOn(LanguageCapability.Tess)]
        SpacingEqual = 1,
        /// <summary>
        /// Requests the tessellation primitive generator to divide
        /// edges into an even number of equal-length segments
        /// plus two additional shorter fractional segments. Only
        /// valid with one of the tessellation Execution Models.
        /// </summary>
        [DependsOn(LanguageCapability.Tess)]
        SpacingFractionalEven = 2,
        /// <summary>
        /// Requests the tessellation primitive generator to divide
        /// edges into an odd number of equal-length segments
        /// plus two additional shorter fractional segments. Only
        /// valid with one of the tessellation Execution Models
        /// </summary>
        [DependsOn(LanguageCapability.Tess)]
        SpacingFractionalOdd = 3,
        /// <summary>
        /// Requests the tessellation primitive generator to
        /// generate triangles in clockwise order. Only valid with
        /// one of the tessellation Execution Models.
        /// </summary>
        [DependsOn(LanguageCapability.Tess)]
        VertexOrderCw = 4,
        /// <summary>
        /// Requests the tessellation primitive generator to
        /// generate triangles in counter-clockwise order. Only
        /// valid with one of the tessellation Execution Models.
        /// </summary>
        [DependsOn(LanguageCapability.Tess)]
        VertexOrderCcw = 5,
        /// <summary>
        /// Pixels appear centered on whole-number pixel
        /// offsets. E.g., the coordinate (0.5, 0.5) appears to
        /// move to (0.0, 0.0). Only valid with the Fragment
        /// Execution Model.
        /// </summary>
        [DependsOn(LanguageCapability.Shader)]
        PixelCenterInteger = 6,
        /// <summary>
        /// Pixel coordinates appear to originate in the upper left,
        /// and increase toward the right and downward. Only
        /// valid with the Fragment Execution Model.
        /// </summary>
        [DependsOn(LanguageCapability.Shader)]
        OriginUpperLeft = 7,
        /// <summary>
        /// Fragment tests are to be performed before fragment
        /// shader execution. Only valid with the Fragment
        /// Execution Model.
        /// </summary>
        [DependsOn(LanguageCapability.Shader)]
        EarlyFragmentTests = 8,
        /// <summary>
        /// Requests the tessellation primitive generator to
        /// generate a point for each distinct vertex in the
        /// subdivided primitive, rather than to generate lines or
        /// triangles. Only valid with one of the tessellation
        /// Execution Models
        /// </summary>
        [DependsOn(LanguageCapability.Tess)]
        PointMode = 9,
        /// <summary>
        /// This stage will run in transform feedback-capturing
        /// mode and this module is responsible for describing
        /// the transform-feedback setup. See the XfbBuffer,
        /// Offset, and Stride Decorations
        /// </summary>
        [DependsOn(LanguageCapability.Shader)]
        Xfb = 10,
        /// <summary>
        /// This mode must be declared if this module
        /// potentially changes the fragment’s depth. Only valid
        /// with the Fragment Execution Model.
        /// </summary>
        [DependsOn(LanguageCapability.Shader)]
        DepthReplacing = 11,
        /// <summary>
        /// TBD: this should probably be removed. Depth
        /// testing will always be performed after the shader has
        /// executed. Only valid with the Fragment Execution
        /// Model.
        /// </summary>
        [DependsOn(LanguageCapability.Shader)]
        DepthAny = 12,
        /// <summary>
        /// External optimizations may assume depth
        /// modifications will leave the fragment’s depth as
        /// greater than or equal to the fragment’s interpolated
        /// depth value (given by the z component of the
        /// FragCoord Built-In decorated variable). Only valid
        /// with the Fragment Execution Model.
        /// </summary>
        [DependsOn(LanguageCapability.Shader)]
        DepthGreater = 13,
        /// <summary>
        /// External optimizations may assume depth
        /// modifications leave the fragment’s depth less than the
        /// fragment’s interpolated depth value, (given by the z
        /// component of the FragCoord Built-In decorated
        /// variable). Only valid with the Fragment Execution
        /// Model.
        /// </summary>
        [DependsOn(LanguageCapability.Shader)]
        DepthLess = 14,
        /// <summary>
        /// External optimizations may assume this stage did not
        /// modify the fragment’s depth. However,
        /// DepthReplacing mode must accurately represent
        /// depth modification. Only valid with the Fragment
        /// Execution Model
        /// </summary>
        [DependsOn(LanguageCapability.Shader)]
        DepthUnchanged = 15,
        /// <summary>
        /// Indicates the work-group size in the x, y, and z
        /// dimensions. Only valid with the GLCompute or
        /// Kernel Execution Models
        /// </summary>
        [ExtraOperand(OperandType.LiteralNumber, "x size")]
        [ExtraOperand(OperandType.LiteralNumber, "y size")]
        [ExtraOperand(OperandType.LiteralNumber, "z size")]
        LocalSize = 16,
        /// <summary>
        /// A hint to the compiler, which indicates the most
        /// likely to be used work-group size in the x, y, and z
        /// dimensions. Only valid with the Kernel Execution
        /// Model
        /// </summary>
        [ExtraOperand(OperandType.LiteralNumber, "x size")]
        [ExtraOperand(OperandType.LiteralNumber, "y size")]
        [ExtraOperand(OperandType.LiteralNumber, "z size")]
        [DependsOn(LanguageCapability.Kernel)]
        LocalSizeHint = 17,
        /// <summary>
        /// Stage input primitive is points. Only valid with the
        /// Geometry Execution Model.
        /// </summary>
        [DependsOn(LanguageCapability.Geom)]
        InputPoints = 18,
        /// <summary>
        /// Stage input primitive is lines. Only valid with the
        /// Geometry Execution Model.
        /// </summary>
        [DependsOn(LanguageCapability.Geom)]
        InputLines = 19,
        /// <summary>
        /// Stage input primitive is lines adjacency. Only valid
        /// with the Geometry Execution Model.
        /// </summary>
        [DependsOn(LanguageCapability.Geom)]
        InputLinesAdjacency = 20,
        /// <summary>
        /// For a geometry stage, input primitive is triangles. For
        /// a tessellation stage, requests the tessellation primitive
        /// generator to generate triangles. Only valid with the
        /// Geometry or one of the tessellation Execution
        /// Models.
        /// </summary>
        [DependsOn(LanguageCapability.Geom | LanguageCapability.Tess)]
        InputTriangles = 21,
        /// <summary>
        /// InputTrianglesAdjacency
        /// Geometry stage input primitive is triangles
        /// adjacency. Only valid with the Geometry Execution
        /// Model
        /// </summary>
        [DependsOn(LanguageCapability.Geom)]
        InputTrianglesAdjacency = 22,
        /// <summary>
        /// Requests the tessellation primitive generator to
        /// generate quads. Only valid with one of the
        /// tessellation Execution Models
        /// </summary>
        [DependsOn(LanguageCapability.Tess)]
        InputQuads = 23,
        /// <summary>
        /// Requests the tessellation primitive generator to
        /// generate isolines. Only valid with one of the
        /// tessellation Execution Models.
        /// </summary>
        [DependsOn(LanguageCapability.Tess)]
        InputIsolines = 24,
        /// <summary>
        /// For a geometry stage, the maximum number of
        /// vertices the shader will ever emit in a single
        /// invocation. For a tessellation-control stage, the
        /// number of vertices in the output patch produced by
        /// the tessellation control shader, which also specifies
        /// the number of times the tessellation control shader is
        /// invoked. Only valid with the Geometry or one of the
        /// tessellation Execution Models.
        /// </summary>
        [ExtraOperand(OperandType.LiteralNumber, "Vertex count")]
        [DependsOn(LanguageCapability.Geom | LanguageCapability.Tess)]
        OutputVertices = 25,
        /// <summary>
        /// Stage output primitive is points. Only valid with the
        /// Geometry Execution Model.
        /// </summary>
        [DependsOn(LanguageCapability.Geom)]
        OutputPoints = 26,
        /// <summary>
        /// Stage output primitive is line strip. Only valid with
        /// the Geometry Execution Model.
        /// </summary>
        [DependsOn(LanguageCapability.Geom)]
        OutputLineStrip = 27,
        /// <summary>
        /// Stage output primitive is triangle strip. Only valid
        /// with the Geometry Execution Model.
        /// </summary>
        [DependsOn(LanguageCapability.Geom)]
        OutputTriangleStrip = 28,
        /// <summary>
        /// A hint to the compiler, which indicates that most
        /// operations used in the entry point are explicitly
        /// vectorized using a particular vector type.Only valid
        /// with the Kernel Execution Model
        /// </summary>
        [ExtraOperand(OperandType.Id, "Vector type")]
        [DependsOn(LanguageCapability.Kernel)]
        VecTypeHint = 29,
        /// <summary>
        /// Indicates that floating-point-expressions contraction
        /// is disallowed. Only valid with the Kernel Execution
        /// Model.
        /// </summary>
        [DependsOn(LanguageCapability.Kernel)]
        ContractionOff = 30
    }
}