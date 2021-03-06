﻿namespace SpirvNet.Spirv.Enums
{
    /// <summary>
    /// SPIR-V OpCodes
    /// </summary>
    public enum OpCode
    {
        Nop = 0,
        Source = 1,
        SourceExtension = 2,
        Extension = 3,
        ExtInstImport = 4,
        MemoryModel = 5,
        EntryPoint = 6,
        ExecutionMode = 7,
        TypeVoid = 8,
        TypeBool = 9,
        TypeInt = 10,
        TypeFloat = 11,
        TypeVector = 12,
        TypeMatrix = 13,
        TypeSampler = 14,
        TypeFilter = 15,
        TypeArray = 16,
        TypeRuntimeArray = 17,
        TypeStruct = 18,
        TypeOpaque = 19,
        TypePointer = 20,
        TypeFunction = 21,
        TypeEvent = 22,
        TypeDeviceEvent = 23,
        TypeReserveId = 24,
        TypeQueue = 25,
        TypePipe = 26,
        ConstantTrue = 27,
        ConstantFalse = 28,
        Constant = 29,
        ConstantComposite = 30,
        ConstantSampler = 31,
        ConstantNullPointer = 32,
        ConstantNullObject = 33,
        SpecConstantTrue = 34,
        SpecConstantFalse = 35,
        SpecConstant = 36,
        SpecConstantComposite = 37,
        Variable = 38,
        VariableArray = 39,
        Function = 40,
        FunctionParameter = 41,
        FunctionEnd = 42,
        FunctionCall = 43,
        ExtInst = 44,
        Undef = 45,
        Load = 46,
        Store = 47,
        Phi = 48,
        DecorationGroup = 49,
        Decorate = 50,
        MemberDecorate = 51,
        GroupDecorate = 52,
        GroupMemberDecorate = 53,
        Name = 54,
        MemberName = 55,
        String = 56,
        Line = 57,
        VectorExtractDynamic = 58,
        VectorInsertDynamic = 59,
        VectorShuffle = 60,
        CompositeConstruct = 61,
        CompositeExtract = 62,
        CompositeInsert = 63,
        CopyObject = 64,
        CopyMemory = 65,
        CopyMemorySized = 66,
        Sampler = 67,
        TextureSample = 68,
        TextureSampleDref = 69,
        TextureSampleLod = 70,
        TextureSampleProj = 71,
        TextureSampleGrad = 72,
        TextureSampleOffset = 73,
        TextureSampleProjLod = 74,
        TextureSampleProjGrad = 75,
        TextureSampleLodOffset = 76,
        TextureSampleProjOffset = 77,
        TextureSampleGradOffset = 78,
        TextureSampleProjLodOffset = 79,
        TextureSampleProjGradOffset = 80,
        TextureFetchTexel = 81,
        TextureFetchTexelOffset = 82,
        TextureFetchSample = 83,
        TextureFetchBuffer = 84,
        TextureGather = 85,
        TextureGatherOffset = 86,
        TextureGatherOffsets = 87,
        TextureQuerySizeLod = 88,
        TextureQuerySize = 89,
        TextureQueryLod = 90,
        TextureQueryLevels = 91,
        TextureQuerySamples = 92,
        AccessChain = 93,
        InBoundsAccessChain = 94,
        SNegate = 95,
        FNegate = 96,
        Not = 97,
        Any = 98,
        All = 99,
        ConvertFToU = 100,
        ConvertFToS = 101,
        ConvertSToF = 102,
        ConvertUToF = 103,
        UConvert = 104,
        SConvert = 105,
        FConvert = 106,
        ConvertPtrToU = 107,
        ConvertUToPtr = 108,
        PtrCastToGeneric = 109,
        GenericCastToPtr = 110,
        Bitcast = 111,
        Transpose = 112,
        IsNan = 113,
        IsInf = 114,
        IsFinite = 115,
        IsNormal = 116,
        SignBitSet = 117,
        LessOrGreater = 118,
        Ordered = 119,
        Unordered = 120,
        ArrayLength = 121,
        IAdd = 122,
        FAdd = 123,
        ISub = 124,
        FSub = 125,
        IMul = 126,
        FMul = 127,
        UDiv = 128,
        SDiv = 129,
        FDiv = 130,
        UMod = 131,
        SRem = 132,
        SMod = 133,
        FRem = 134,
        FMod = 135,
        VectorTimesScalar = 136,
        MatrixTimesScalar = 137,
        VectorTimesMatrix = 138,
        MatrixTimesVector = 139,
        MatrixTimesMatrix = 140,
        OuterProduct = 141,
        Dot = 142,
        ShiftRightLogical = 143,
        ShiftRightArithmetic = 144,
        ShiftLeftLogical = 145,
        LogicalOr = 146,
        LogicalXor = 147,
        LogicalAnd = 148,
        BitwiseOr = 149,
        BitwiseXor = 150,
        BitwiseAnd = 151,
        Select = 152,
        IEqual = 153,
        FOrdEqual = 154,
        FUnordEqual = 155,
        INotEqual = 156,
        FOrdNotEqual = 157,
        FUnordNotEqual = 158,
        ULessThan = 159,
        SLessThan = 160,
        FOrdLessThan = 161,
        FUnordLessThan = 162,
        UGreaterThan = 163,
        SGreaterThan = 164,
        FOrdGreaterThan = 165,
        FUnordGreaterThan = 166,
        ULessThanEqual = 167,
        SLessThanEqual = 168,
        FOrdLessThanEqual = 169,
        FUnordLessThanEqual = 170,
        UGreaterThanEqual = 171,
        SGreaterThanEqual = 172,
        FOrdGreaterThanEqual = 173,
        FUnordGreaterThanEqual = 174,
        DPdx = 175,
        DPdy = 176,
        Fwidth = 177,
        DPdxFine = 178,
        DPdyFine = 179,
        FwidthFine = 180,
        DPdxCoarse = 181,
        DPdyCoarse = 182,
        FwidthCoarse = 183,
        EmitVertex = 184,
        EndPrimitive = 185,
        EmitStreamVertex = 186,
        EndStreamPrimitive = 187,
        ControlBarrier = 188,
        MemoryBarrier = 189,
        ImagePointer = 190,
        AtomicInit = 191,
        AtomicLoad = 192,
        AtomicStore = 193,
        AtomicExchange = 194,
        AtomicCompareExchange = 195,
        AtomicCompareExchangeWeak = 196,
        AtomicIIncrement = 197,
        AtomicIDecrement = 198,
        AtomicIAdd = 199,
        AtomicISub = 200,
        AtomicUMin = 201,
        AtomicUMax = 202,
        AtomicAnd = 203,
        AtomicOr = 204,
        AtomicXor = 205,
        LoopMerge = 206,
        SelectionMerge = 207,
        Label = 208,
        Branch = 209,
        BranchConditional = 210,
        Switch = 211,
        Kill = 212,
        Return = 213,
        ReturnValue = 214,
        Unreachable = 215,
        LifetimeStart = 216,
        LifetimeStop = 217,
        CompileFlag = 218,
        AsyncGroupCopy = 219,
        WaitGroupEvents = 220,
        GroupAll = 221,
        GroupAny = 222,
        GroupBroadcast = 223,
        GroupIAdd = 224,
        GroupFAdd = 225,
        GroupFMin = 226,
        GroupUMin = 227,
        GroupSMin = 228,
        GroupFMax = 229,
        GroupUMax = 230,
        GroupSMax = 231,
        GenericCastToPtrExplicit = 232,
        GenericPtrMemSemantics = 233,
        ReadPipe = 234,
        WritePipe = 235,
        ReservedReadPipe = 236,
        ReservedWritePipe = 237,
        ReserveReadPipePackets = 238,
        ReserveWritePipePackets = 239,
        CommitReadPipe = 240,
        CommitWritePipe = 241,
        IsValidReserveId = 242,
        GetNumPipePackets = 243,
        GetMaxPipePackets = 244,
        GroupReserveReadPipePackets = 245,
        GroupReserveWritePipePackets = 246,
        GroupCommitReadPipe = 247,
        GroupCommitWritePipe = 248,
        EnqueueMarker = 249,
        EnqueueKernel = 250,
        GetKernelNDrangeSubGroupCount = 251,
        GetKernelNDrangeMaxSubGroupSize = 252,
        GetKernelWorkGroupSize = 253,
        GetKernelPreferredWorkGroupSizeMultiple = 254,
        RetainEvent = 255,
        ReleaseEvent = 256,
        CreateUserEvent = 257,
        IsValidEvent = 258,
        SetUserEventStatus = 259,
        CaptureEventProfilingInfo = 260,
        GetDefaultQueue = 261,
        BuildNDRange = 262,


        Unknown
    }
}
