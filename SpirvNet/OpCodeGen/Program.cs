using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpCodeGen
{
    class Program
    {
        private static string opCategory;

        static IEnumerable<OpCode> GenOps()
        {
            opCategory = "Misc";
            yield return Op("Nop");
            yield return Op("Undef", Id("ResultType"), Id("Result"));

            opCategory = "Debug";
            yield return Op("Source", Typed("SourceLanguage"), Nr("Version"));
            yield return Op("SourceExtension", Str("Extension"));
            yield return Op("Name", Id("Target"), Str("Name"));
            yield return Op("MemberName", Id("Type"), Nr("Member"), Str("Name"));
            yield return Op("String", Id("Result"), Str("Name"));
            yield return Op("Line", Id("Target"), Id("File"), Nr("Line"), Nr("Column"));

            opCategory = "Annotation";
            yield return Op("DecorationGroup", Id("Result"));
            yield return Op("Decorate", Id("Target"), Typed("Decoration"), NrArray("Args"));
            yield return Op("MemberDecorate", Id("StructureType"), Nr("Member"), Typed("Decoration"), NrArray("Args"));
            yield return Op("GroupDecorate", Id("DecorationGroup"), IdArray("Targets"));
            yield return Op("GroupMemberDecorate", Id("DecorationGroup"), IdArray("Targets"));

            opCategory = "Extension";
            yield return Op("Extension", Str("Name"));
            yield return Op("ExtInstImport", Id("Result"), Str("Name"));
            yield return Op("ExtInst", Id("ResultType"), Id("Result"), Id("Set"), Nr("Instruction"), IdArray("Operands"));

            opCategory = "ModeSetting";
            yield return Op("MemoryModel", Typed("AddressingModel"), Typed("MemoryModel"));
            yield return Op("EntryPoint", Typed("ExecutionModel"), Id("EntryPoint"));
            yield return Op("ExecutionMode", Id("EntryPoint"), Typed("ExecutionMode"), NrArray("Args")); // TODO: , IdOpt("VectorType")
            yield return Op("CompileFlag", Str("Flag")).Compat("Kernel");

            opCategory = "TypeDeclaration";
            yield return Op("TypeVoid", Id("Result"));
            yield return Op("TypeBool", Id("Result"));
            yield return Op("TypeInt", Id("Result"), Nr("Width"), Nr("Signedness"));
            yield return Op("TypeFloat", Id("Result"), Nr("Width"));
            yield return Op("TypeVector", Id("Result"), Id("ComponentType"), Nr("ComponentCount"));
            yield return Op("TypeMatrix", Id("Result"), Id("ColumnType"), Nr("ColumnCount"));
            yield return Op("TypeSampler", Id("Result"), Id("SampledType"), Typed("Dim"), Nr("Content"), Nr("Arrayed"), Nr("Compare"), Nr("MS"), IdOpt("Qualifier"));
            yield return Op("TypeFilter", Id("Result"));
            yield return Op("TypeArray", Id("Result"), Id("ElementType"), Id("Length"));
            yield return Op("TypeRuntimeArray", Id("Result"), Id("ElementType")).Compat("Shader");
            yield return Op("TypeStruct", Id("Result"), IdArray("MemberTypes"));
            yield return Op("TypeOpaque", Id("Result"), Str("OpaqueType")).Compat("Kernel");
            yield return Op("TypePointer", Id("Result"), Typed("StorageClass"), Id("Type"));
            yield return Op("TypeFunction", Id("Result"), Id("ReturnType"), IdArray("ParameterTypes"));
            yield return Op("TypeEvent", Id("Result")).Compat("Kernel");
            yield return Op("TypeDeviceEvent", Id("Result")).Compat("Kernel");
            yield return Op("TypeReserveId", Id("Result")).Compat("Kernel");
            yield return Op("TypeQueue", Id("Result")).Compat("Kernel");
            yield return Op("TypePipe", Id("Result"), Id("Type"), Typed("AccessQualifier")).Compat("Kernel");

            opCategory = "ConstantCreation";
            yield return Op("ConstantTrue", Id("ResultType"), Id("Result"));
            yield return Op("ConstantFalse", Id("ResultType"), Id("Result"));
            yield return Op("Constant", Id("ResultType"), Id("Result"), NrArray("Value"));
            yield return Op("ConstantComposite", Id("ResultType"), Id("Result"), IdArray("Constituents"));
            yield return Op("ConstantSampler", Id("ResultType"), Id("Result"), Nr("Mode"), Nr("Param"), Nr("Filter")).Compat("Kernel");
            yield return Op("ConstantNullPointer", Id("ResultType"), Id("Result")).Compat("Addr");
            yield return Op("ConstantNullObject", Id("ResultType"), Id("Result")).Compat("Kernel");
            yield return Op("SpecConstantTrue", Id("ResultType"), Id("Result")).Compat("Shader");
            yield return Op("SpecConstantFalse", Id("ResultType"), Id("Result")).Compat("Shader");
            yield return Op("SpecConstant", Id("ResultType"), Id("Result"), NrArray("Value")).Compat("Shader");
            yield return Op("SpecConstantComposite", Id("ResultType"), Id("Result"), IdArray("Constituents")).Compat("Shader");

            opCategory = "Memory";
            yield return Op("Variable", Id("ResultType"), Id("Result"), Typed("StorageClass"), IdOpt("Initializer"));
            yield return Op("VariableArray", Id("ResultType"), Id("Result"), Typed("StorageClass"), Id("N")).Compat("Addr");
            yield return Op("Load", Id("ResultType"), Id("Result"), Id("Pointer"), TypedArray("MemoryAccess"));
            yield return Op("Store", Id("Pointer"), Id("Object"), TypedArray("MemoryAccess"));
            yield return Op("CopyMemory", Id("Target"), Id("Source"), TypedArray("MemoryAccess"));
            yield return Op("CopyMemorySized", Id("Target"), Id("Source"), Id("Size"), TypedArray("MemoryAccess")).Compat("Addr");
            yield return Op("AccessChain", Id("ResultType"), Id("Result"), Id("Base"), IdArray("Indexes"));
            yield return Op("InBoundsAccessChain", Id("ResultType"), Id("Result"), Id("Base"), IdArray("Indexes"));
            yield return Op("ArrayLength", Id("ResultType"), Id("Result"), Id("Structure"), Nr("ArrayMember")).Compat("Shader");
            yield return Op("ImagePointer", Id("ResultType"), Id("Result"), Id("Image"), Id("Coordinate"), Id("Sample"));
            yield return Op("GenericPtrMemSemantics", Id("ResultType"), Id("Result"), Id("Ptr")).Compat("Kernel");

            opCategory = "Function";
            yield return Op("Function", Id("ResultType"), Id("Result"), Typed("FunctionControlMask"), Id("FunctionType"));
            yield return Op("FunctionParameter", Id("ResultType"), Id("Result"));
            yield return Op("FunctionEnd");
            yield return Op("FunctionCall", Id("ResultType"), Id("Result"), Id("Function"), IdArray("Arguments"));

            opCategory = "Texture";
            yield return Op("Sampler", Id("ResultType"), Id("Result"), Id("Sampler"), Id("Filter"));
            yield return Op("TextureSample", Id("ResultType"), Id("Result"), Id("Sampler"), Id("Coordinate"), IdOpt("Bias")).Compat("Shader");
            yield return Op("TextureSampleDref", Id("ResultType"), Id("Result"), Id("Sampler"), Id("Coordinate"), Id("Dref")).Compat("Shader");
            yield return Op("TextureSampleLod", Id("ResultType"), Id("Result"), Id("Sampler"), Id("Coordinate"), Id("Lod")).Compat("Shader");
            yield return Op("TextureSampleProj", Id("ResultType"), Id("Result"), Id("Sampler"), Id("Coordinate"), IdOpt("Bias")).Compat("Shader");
            yield return Op("TextureSampleGrad", Id("ResultType"), Id("Result"), Id("Sampler"), Id("Coordinate"), Id("dx"), Id("dy")).Compat("Shader");
            yield return Op("TextureSampleOffset", Id("ResultType"), Id("Result"), Id("Sampler"), Id("Coordinate"), Id("Offset"), IdOpt("Bias")).Compat("Shader");
            yield return Op("TextureSampleProjLod", Id("ResultType"), Id("Result"), Id("Sampler"), Id("Coordinate"), Id("Lod")).Compat("Shader");
            yield return Op("TextureSampleProjGrad", Id("ResultType"), Id("Result"), Id("Sampler"), Id("Coordinate"), Id("dx"), Id("dy")).Compat("Shader");
            yield return Op("TextureSampleLodOffset", Id("ResultType"), Id("Result"), Id("Sampler"), Id("Coordinate"), Id("Lod"), Id("Offset")).Compat("Shader");
            yield return Op("TextureSampleProjOffset", Id("ResultType"), Id("Result"), Id("Sampler"), Id("Coordinate"), Id("Offset"), IdOpt("Bias")).Compat("Shader");
            yield return Op("TextureSampleGradOffset", Id("ResultType"), Id("Result"), Id("Sampler"), Id("Coordinate"), Id("dx"), Id("dy"), Id("Offset")).Compat("Shader");
            yield return Op("TextureSampleProjLodOffset", Id("ResultType"), Id("Result"), Id("Sampler"), Id("Coordinate"), Id("Lod"), Id("Offset")).Compat("Shader");
            yield return Op("TextureSampleProjGradOffset", Id("ResultType"), Id("Result"), Id("Sampler"), Id("Coordinate"), Id("dx"), Id("dy"), Id("Offset")).Compat("Shader");
            yield return Op("TextureFetchTexel", Id("ResultType"), Id("Result"), Id("Sampler"), Id("Coordinate"), Id("Lod")).Compat("Shader");
            yield return Op("TextureFetchTexelOffset", Id("ResultType"), Id("Result"), Id("Sampler"), Id("Coordinate"), Id("Offset")).Compat("Shader");
            yield return Op("TextureFetchSample", Id("ResultType"), Id("Result"), Id("Sampler"), Id("Coordinate"), Id("Sample")).Compat("Shader");
            yield return Op("TextureFetchBuffer", Id("ResultType"), Id("Result"), Id("Sampler"), Id("Element")).Compat("Shader");
            yield return Op("TextureGather", Id("ResultType"), Id("Result"), Id("Sampler"), Id("Coordinate"), Id("Component")).Compat("Shader");
            yield return Op("TextureGatherOffset", Id("ResultType"), Id("Result"), Id("Sampler"), Id("Coordinate"), Id("Component"), Id("Offset")).Compat("Shader");
            yield return Op("TextureGatherOffsets", Id("ResultType"), Id("Result"), Id("Sampler"), Id("Coordinate"), Id("Component"), Id("Offsets")).Compat("Shader");
            yield return Op("TextureQuerySizeLod", Id("ResultType"), Id("Result"), Id("Sampler"), Id("Lod")).Compat("Shader");
            yield return Op("TextureQuerySize", Id("ResultType"), Id("Result"), Id("Sampler")).Compat("Shader");
            yield return Op("TextureQueryLod", Id("ResultType"), Id("Result"), Id("Sampler"), Id("Coordinate")).Compat("Shader");
            yield return Op("TextureQueryLevels", Id("ResultType"), Id("Result"), Id("Sampler")).Compat("Shader");
            yield return Op("TextureQuerySamples", Id("ResultType"), Id("Result"), Id("Sampler")).Compat("Shader");

            opCategory = "Conversion";
            yield return Op("ConvertFToU", Id("ResultType"), Id("Result"), Id("FloatValue"));
            yield return Op("ConvertFToS", Id("ResultType"), Id("Result"), Id("FloatValue"));
            yield return Op("ConvertSToF", Id("ResultType"), Id("Result"), Id("SignedValue"));
            yield return Op("ConvertUToF", Id("ResultType"), Id("Result"), Id("UnsignedValue"));
            yield return Op("UConvert", Id("ResultType"), Id("Result"), Id("UnsignedValue"));
            yield return Op("SConvert", Id("ResultType"), Id("Result"), Id("SignedValue"));
            yield return Op("FConvert", Id("ResultType"), Id("Result"), Id("FloatValue"));
            yield return Op("ConvertPtrToU", Id("ResultType"), Id("Result"), Id("Pointer")).Compat("Addr");
            yield return Op("ConvertUToPtr", Id("ResultType"), Id("Result"), Id("IntegerValue")).Compat("Addr");
            yield return Op("PtrCastToGeneric", Id("ResultType"), Id("Result"), Id("SourcePointer")).Compat("Kernel");
            yield return Op("GenericCastToPtr", Id("ResultType"), Id("Result"), Id("SourcePointer")).Compat("Kernel");
            yield return Op("Bitcast", Id("ResultType"), Id("Result"), Id("Operand"));
            yield return Op("GenericCastToPtrExplicit", Id("ResultType"), Id("Result"), Id("SourcePointer"), Typed("StorageClass")).Compat("Kernel");

            opCategory = "Composite";
            yield return Op("VectorExtractDynamic", Id("ResultType"), Id("Result"), Id("Vector"), Id("Index"));
            yield return Op("VectorInsertDynamic", Id("ResultType"), Id("Result"), Id("Component"), Id("Index"));
            yield return Op("VectorShuffle", Id("ResultType"), Id("Result"), Id("Vector1"), Id("Vector2"), NrArray("Components"));
            yield return Op("CompositeConstruct", Id("ResultType"), Id("Result"), IdArray("Constituents"));
            yield return Op("CompositeExtract", Id("ResultType"), Id("Result"), Id("Composite"), NrArray("Indexes"));
            yield return Op("CompositeInsert", Id("ResultType"), Id("Result"), Id("Object"), Id("Composite"), NrArray("Indexes"));
            yield return Op("CopyObject", Id("ResultType"), Id("Result"), Id("Operand"));
            yield return Op("Transpose", Id("ResultType"), Id("Result"), Id("Matrix")).Compat("Matrix");

            opCategory = "Arithmetic";
            yield return Op("SNegate", Id("ResultType"), Id("Result"), Id("Operand"));
            yield return Op("FNegate", Id("ResultType"), Id("Result"), Id("Operand"));
            yield return Op("Not", Id("ResultType"), Id("Result"), Id("Operand"));
            yield return Op("IAdd", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("FAdd", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("ISub", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("FSub", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("IMul", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("FMul", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("UDiv", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("SDiv", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("FDiv", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("UMod", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("SRem", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("SMod", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("FRem", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("FMod", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("VectorTimesScalar", Id("ResultType"), Id("Result"), Id("Vector"), Id("Scalar"));
            yield return Op("MatrixTimesScalar", Id("ResultType"), Id("Result"), Id("Matrix"), Id("Scalar")).Compat("Matrix");
            yield return Op("VectorTimesMatrix", Id("ResultType"), Id("Result"), Id("Vector"), Id("Matrix")).Compat("Matrix");
            yield return Op("MatrixTimesVector", Id("ResultType"), Id("Result"), Id("Matrix"), Id("Vector")).Compat("Matrix");
            yield return Op("MatrixTimesMatrix", Id("ResultType"), Id("Result"), Id("LeftMatrix"), Id("RightMatrix")).Compat("Matrix");
            yield return Op("OuterProduct", Id("ResultType"), Id("Result"), Id("Vector1"), Id("Vector2")).Compat("Matrix");
            yield return Op("Dot", Id("ResultType"), Id("Result"), Id("Vector1"), Id("Vector2"));
            yield return Op("ShiftRightLogical", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("ShiftRightArithmetic", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("ShiftLeftLogical", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("BitwiseOr", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("BitwiseXor", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("BitwiseAnd", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));

            opCategory = "RelationalLogical";
            yield return Op("Any", Id("ResultType"), Id("Result"), Id("Vector"));
            yield return Op("All", Id("ResultType"), Id("Result"), Id("Vector"));
            yield return Op("IsNan", Id("ResultType"), Id("Result"), Id("x"));
            yield return Op("IsInf", Id("ResultType"), Id("Result"), Id("x"));
            yield return Op("IsFinite", Id("ResultType"), Id("Result"), Id("x")).Compat("Kernel");
            yield return Op("IsNormal", Id("ResultType"), Id("Result"), Id("x")).Compat("Kernel");
            yield return Op("SignBitSet", Id("ResultType"), Id("Result"), Id("x")).Compat("Kernel");
            yield return Op("LessOrGreater", Id("ResultType"), Id("Result"), Id("x"), Id("y")).Compat("Kernel");
            yield return Op("Ordered", Id("ResultType"), Id("Result"), Id("x"), Id("y")).Compat("Kernel");
            yield return Op("Unordered", Id("ResultType"), Id("Result"), Id("x"), Id("y")).Compat("Kernel");
            yield return Op("LogicalOr", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("LogicalXor", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("LogicalAnd", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("Select", Id("ResultType"), Id("Result"), Id("Condition"), Id("Object1"), Id("Object2"));
            yield return Op("IEqual", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("FOrdEqual", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("FUnordEqual", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("INotEqual", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("FOrdNotEqual", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("FUnordNotEqual", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("ULessThan", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("SLessThan", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("FOrdLessThan", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("FUnordLessThan", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("UGreaterThan", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("SGreaterThan", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("FOrdGreaterThan", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("FUnordGreaterThan", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("ULessThanEqual", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("SLessThanEqual", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("FOrdLessThanEqual", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("FUnordLessThanEqual", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("UGreaterThanEqual", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("SGreaterThanEqual", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("FOrdGreaterThanEqual", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));
            yield return Op("FUnordGreaterThanEqual", Id("ResultType"), Id("Result"), Id("Operand1"), Id("Operand2"));

            opCategory = "Derivative";
            yield return Op("DPdx", Id("ResultType"), Id("Result"), Id("P")).Compat("Shader");
            yield return Op("DPdy", Id("ResultType"), Id("Result"), Id("P")).Compat("Shader");
            yield return Op("Fwidth", Id("ResultType"), Id("Result"), Id("P")).Compat("Shader");
            yield return Op("DPdxFine", Id("ResultType"), Id("Result"), Id("P")).Compat("Shader");
            yield return Op("DPdyFine", Id("ResultType"), Id("Result"), Id("P")).Compat("Shader");
            yield return Op("FwidthFine", Id("ResultType"), Id("Result"), Id("P")).Compat("Shader");
            yield return Op("DPdxCoarse", Id("ResultType"), Id("Result"), Id("P")).Compat("Shader");
            yield return Op("DPdyCoarse", Id("ResultType"), Id("Result"), Id("P")).Compat("Shader");
            yield return Op("FwidthCoarse", Id("ResultType"), Id("Result"), Id("P")).Compat("Shader");

            opCategory = "FlowControl";
            yield return Op("Phi", Id("ResultType"), Id("Result"), IdArray("IDs"));
            yield return Op("LoopMerge", Id("Label"), Typed("LoopControl"));
            yield return Op("SelectionMerge", Id("Label"), Typed("SelectionControl"));
            yield return Op("Label", Id("Result"));
            yield return Op("Branch", Id("TargetLabel"));
            yield return Op("BranchConditional", Id("Condition"), Id("TrueLabel"), Id("FalseLabel"), NrArray("BranchWeights"));
            yield return Op("Switch", Id("Selector"), Id("Default"), PairArray(Nr("Literal"), Id("Label"), "Target"));
            yield return Op("Kill");
            yield return Op("Return");
            yield return Op("ReturnValue", Id("Value"));
            yield return Op("Unreachable");
            yield return Op("LifetimeStart", Id("Object"), Nr("Literal"));
            yield return Op("LifetimeStop", Id("Object"), Nr("Literal"));

            opCategory = "Atomic";
            yield return Op("AtomicInit", Id("Pointer"), Id("Value"));
            yield return Op("AtomicLoad", Id("ResultType"), Id("Result"), Id("Pointer"), Typed("ExecutionScope", "Scope"), Typed("MemorySemantics", "Semantics"));
            yield return Op("AtomicStore", Id("Pointer"), Typed("ExecutionScope", "Scope"), Typed("MemorySemantics", "Semantics"), Id("Value"));
            yield return Op("AtomicExchange", Id("ResultType"), Id("Result"), Id("Pointer"), Typed("ExecutionScope", "Scope"), Typed("MemorySemantics", "Semantics"), Id("Value"));
            yield return Op("AtomicCompareExchange", Id("ResultType"), Id("Result"), Id("Pointer"), Typed("ExecutionScope", "Scope"), Typed("MemorySemantics", "Semantics"), Id("Value"), Id("Comparator"));
            yield return Op("AtomicCompareExchangeWeak", Id("ResultType"), Id("Result"), Id("Pointer"), Typed("ExecutionScope", "Scope"), Typed("MemorySemantics", "Semantics"), Id("Value"), Id("Comparator"));
            yield return Op("AtomicIIncrement", Id("ResultType"), Id("Result"), Id("Pointer"), Typed("ExecutionScope", "Scope"), Typed("MemorySemantics", "Semantics"));
            yield return Op("AtomicIDecrement", Id("ResultType"), Id("Result"), Id("Pointer"), Typed("ExecutionScope", "Scope"), Typed("MemorySemantics", "Semantics"));
            yield return Op("AtomicIAdd", Id("ResultType"), Id("Result"), Id("Pointer"), Typed("ExecutionScope", "Scope"), Typed("MemorySemantics", "Semantics"), Id("Value"));
            yield return Op("AtomicISub", Id("ResultType"), Id("Result"), Id("Pointer"), Typed("ExecutionScope", "Scope"), Typed("MemorySemantics", "Semantics"), Id("Value"));
            yield return Op("AtomicUMin", Id("ResultType"), Id("Result"), Id("Pointer"), Typed("ExecutionScope", "Scope"), Typed("MemorySemantics", "Semantics"), Id("Value"));
            yield return Op("AtomicUMax", Id("ResultType"), Id("Result"), Id("Pointer"), Typed("ExecutionScope", "Scope"), Typed("MemorySemantics", "Semantics"), Id("Value"));
            yield return Op("AtomicAnd", Id("ResultType"), Id("Result"), Id("Pointer"), Typed("ExecutionScope", "Scope"), Typed("MemorySemantics", "Semantics"), Id("Value"));
            yield return Op("AtomicOr", Id("ResultType"), Id("Result"), Id("Pointer"), Typed("ExecutionScope", "Scope"), Typed("MemorySemantics", "Semantics"), Id("Value"));
            yield return Op("AtomicXor", Id("ResultType"), Id("Result"), Id("Pointer"), Typed("ExecutionScope", "Scope"), Typed("MemorySemantics", "Semantics"), Id("Value"));

            opCategory = "Primitive";
            yield return Op("EmitVertex").Compat("Geom");
            yield return Op("EndPrimitive").Compat("Geom");
            yield return Op("EmitStreamVertex", Id("Stream")).Compat("Geom");
            yield return Op("EndStreamPrimitive", Id("Stream")).Compat("Geom");

            opCategory = "Barrier";
            yield return Op("ControlBarrier", Typed("ExecutionScope", "Scope"));
            yield return Op("MemoryBarrier", Typed("ExecutionScope", "Scope"), Typed("MemorySemantics", "Semantics"));

            opCategory = "Group";
            yield return Op("AsyncGroupCopy", Id("ResultType"), Id("Result"), Typed("ExecutionScope", "Scope"), Id("Destination"), Id("Source"), Id("NumElements"), Id("Stride"), Id("Event")).Compat("Kernel");
            yield return Op("WaitGroupEvents", Id("ResultType"), Id("Result"), Typed("ExecutionScope", "Scope"), Id("NumEvents"), Id("EventsList")).Compat("Kernel");
            yield return Op("GroupAll", Id("ResultType"), Id("Result"), Typed("ExecutionScope", "Scope"), Id("Predicate")).Compat("Kernel");
            yield return Op("GroupAny", Id("ResultType"), Id("Result"), Typed("ExecutionScope", "Scope"), Id("Predicate")).Compat("Kernel");
            yield return Op("GroupBroadcast", Id("ResultType"), Id("Result"), Typed("ExecutionScope", "Scope"), Id("Value"), Id("LocalId")).Compat("Kernel");
            yield return Op("GroupIAdd", Id("ResultType"), Id("Result"), Typed("ExecutionScope", "Scope"), Id("X")).Compat("Kernel");
            yield return Op("GroupFAdd", Id("ResultType"), Id("Result"), Typed("ExecutionScope", "Scope"), Id("X")).Compat("Kernel");
            yield return Op("GroupFMin", Id("ResultType"), Id("Result"), Typed("ExecutionScope", "Scope"), Id("X")).Compat("Kernel");
            yield return Op("GroupUMin", Id("ResultType"), Id("Result"), Typed("ExecutionScope", "Scope"), Id("X")).Compat("Kernel");
            yield return Op("GroupSMin", Id("ResultType"), Id("Result"), Typed("ExecutionScope", "Scope"), Id("X")).Compat("Kernel");
            yield return Op("GroupFMax", Id("ResultType"), Id("Result"), Typed("ExecutionScope", "Scope"), Id("X")).Compat("Kernel");
            yield return Op("GroupUMax", Id("ResultType"), Id("Result"), Typed("ExecutionScope", "Scope"), Id("X")).Compat("Kernel");
            yield return Op("GroupSMax", Id("ResultType"), Id("Result"), Typed("ExecutionScope", "Scope"), Id("X")).Compat("Kernel");

            opCategory = "DeviceSideEnqueue";
            yield return Op("EnqueueMarker", Id("ResultType"), Id("Result"), Id("q"), Id("NumEvents"), Id("WaitEvents"), Id("RetEvent")).Compat("Kernel");
            yield return Op("EnqueueKernel", Id("ResultType"), Id("Result"), Id("q"), Typed("KernelEnqueueFlags", "Flags"), Id("NDRange"), Id("NumEvents"), Id("WaitEvents"), Id("RetEvent"), Id("Invoke"), Id("Param"), Id("ParamSize"), Id("ParamAlign"), IdArray("LocalSize")).Compat("Kernel");
            yield return Op("GetKernelNDrangeSubGroupCount", Id("ResultType"), Id("Result"), Id("NDRange"), Id("Invoke")).Compat("Kernel");
            yield return Op("GetKernelNDrangeMaxSubGroupSize", Id("ResultType"), Id("Result"), Id("NDRange"), Id("Invoke")).Compat("Kernel");
            yield return Op("GetKernelWorkGroupSize", Id("ResultType"), Id("Result"), Id("Invoke")).Compat("Kernel");
            yield return Op("GetKernelPreferredWorkGroupSizeMultiple", Id("ResultType"), Id("Result"), Id("Invoke")).Compat("Kernel");
            yield return Op("RetainEvent", Id("Event")).Compat("Kernel");
            yield return Op("ReleaseEvent", Id("Event")).Compat("Kernel");
            yield return Op("CreateUserEvent", Id("ResultType"), Id("Event")).Compat("Kernel");
            yield return Op("IsValidEvent", Id("ResultType"), Id("Result"), Id("Event")).Compat("Kernel");
            yield return Op("SetUserEventStatus", Id("Event"), Id("Status")).Compat("Kernel");
            yield return Op("CaptureEventProfilingInfo", Id("Event"), Typed("KernelProfilingInfo", "Info"), Id("Value")).Compat("Kernel");
            yield return Op("GetDefaultQueue", Id("ResultType"), Id("Result")).Compat("Kernel");
            yield return Op("BuildNDRange", Id("ResultType"), Id("Result"), Id("GlobalWorkSize"), Id("LocalWorkSize"), Id("GlobalWorkOffset")).Compat("Kernel");

            opCategory = "Pipe";
            yield return Op("ReadPipe", Id("ResultType"), Id("Result"), Id("P"), Id("Ptr")).Compat("Kernel");
            yield return Op("WritePipe", Id("ResultType"), Id("Result"), Id("P"), Id("Ptr")).Compat("Kernel");
            yield return Op("ReservedReadPipe", Id("ResultType"), Id("Result"), Id("P"), Id("ReserveId"), Id("Index"), Id("Ptr")).Compat("Kernel");
            yield return Op("ReservedWritePipe", Id("ResultType"), Id("Result"), Id("P"), Id("ReserveId"), Id("Index"), Id("Ptr")).Compat("Kernel");
            yield return Op("ReserveReadPipePackets", Id("ResultType"), Id("Result"), Id("P"), Id("NumPackets")).Compat("Kernel");
            yield return Op("ReserveWritePipePackets", Id("ResultType"), Id("Result"), Id("P"), Id("NumPackets")).Compat("Kernel");
            yield return Op("CommitReadPipe", Id("P"), Id("ReserveId")).Compat("Kernel");
            yield return Op("CommitWritePipe", Id("P"), Id("ReserveId")).Compat("Kernel");
            yield return Op("IsValidReserveId", Id("ResultType"), Id("Result"), Id("ReserveId")).Compat("Kernel");
            yield return Op("GetNumPipePackets", Id("ResultType"), Id("Result"), Id("P")).Compat("Kernel");
            yield return Op("GetMaxPipePackets", Id("ResultType"), Id("Result"), Id("P")).Compat("Kernel");
            yield return Op("GroupReserveReadPipePackets", Id("ResultType"), Id("Result"), Typed("ExecutionScope", "Scope"), Id("P"), Id("NumPackets")).Compat("Kernel");
            yield return Op("GroupReserveWritePipePackets", Id("ResultType"), Id("Result"), Typed("ExecutionScope", "Scope"), Id("P"), Id("NumPackets")).Compat("Kernel");
            yield return Op("GroupCommitReadPipe", Typed("ExecutionScope", "Scope"), Id("P"), Id("ReserveId")).Compat("Kernel");
            yield return Op("GroupCommitWritePipe", Typed("ExecutionScope", "Scope"), Id("P"), Id("ReserveId")).Compat("Kernel");
        }

        class OpField
        {
            public string Type;
            public string Name;
            public string Init;
            public Func<string, string[]> ReadCode;
            public Func<string, string[]> WriteCode;
        }

        class OpCode
        {
            public string Name; // without Op
            public readonly string Cat = opCategory;
            private string comment;

            public OpField[] Fields;

            private readonly List<string> compatibilities = new List<string>();

            public OpCode Compat(string compat)
            {
                compatibilities.Add(compat);
                return this;
            }

            public OpCode Cmt(string cmt)
            {
                comment = cmt;
                return this;
            }

            public IEnumerable<string> CreateLines()
            {
                yield return "using System;";
                yield return "using System.Collections.Generic;";
                yield return "using System.Linq;";
                yield return "using System.Text;";
                yield return "using System.Threading.Tasks;";
                yield return "using SpirvNet.Spirv.Enums;";
                yield return "";
                yield return "// This file is auto-generated and should not be modified manually.";
                yield return "";
                yield return "namespace SpirvNet.Spirv.Ops." + Cat;
                yield return "{";
                yield return "    /// <summary>";
                if (string.IsNullOrEmpty(comment))
                    yield return "    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf";
                else
                    foreach (var line in comment.Split('\n'))
                        yield return "    /// " + line.Replace("<id>", " ID").Replace("  ID", " ID");
                yield return "    /// </summary>";
                if (compatibilities.Count > 0)
                    yield return string.Format("    [DependsOn({0})]", compatibilities.Select(c => "LanguageCapability." + c).Aggregate((s1, s2) => s1 + " | " + s2));
                yield return string.Format("    public sealed class Op{0} : {1}Instruction", Name, Cat);
                yield return "    {";
                yield return string.Format("        public override bool Is{0} => true;", Cat);
                yield return string.Format("        public override OpCode OpCode => OpCode.{0};", Name);
                if (Fields.Any(f => f.Name == "Result"))
                    yield return string.Format("        public override ID? ResultID => Result;");
                if (Fields.Any(f => f.Name == "ResultType"))
                    yield return string.Format("        public override ID? ResultTypeID => ResultType;");
                if (Fields.Length > 0)
                {
                    yield return "";
                    foreach (var field in Fields)
                        yield return string.Format("        public {0} {1}{2};", field.Type, field.Name, string.IsNullOrEmpty(field.Init) ? "" : " = " + field.Init);
                }
                yield return "";
                yield return "        #region Code";
                yield return string.Format("        public override string ToString() => \"(\" + OpCode + \"(\" + (int)OpCode + \")\"{0} + \")\";", Fields.Length == 0 ? "" : Fields.Select(f => f.Name).Aggregate("", (s1, s2) => string.Format("{0} + \", \" + StrOf({1})", s1, s2)));
                yield return string.Format("        public override string ArgString => {0};", !Fields.Any(f => f.Name != "Result" && f.Name != "ResultType") ? "\"\"" : Fields.Where(f => f.Name != "Result" && f.Name != "ResultType").Select(f => "\"" + f.Name + ": \" + StrOf(" + f.Name + ")").Aggregate((s1, s2) => string.Format("{0} + \", \" + {1}", s1, s2)));

                //public abstract string ArgString { get; }

                yield return "";
                yield return "        protected override void FromCode(uint[] codes, int start)";
                yield return "        {";
                yield return string.Format("            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.{0});", Name);
                if (Fields.Length > 0)
                {
                    yield return "            var i = start + 1;";
                    foreach (var field in Fields)
                        foreach (var c in field.ReadCode(field.Name))
                            yield return "            " + c;
                }
                yield return "        }";
                yield return "";
                yield return "        protected override void WriteCode(List<uint> code)";
                yield return "        {";
                if (Fields.Length > 0)
                {
                    foreach (var field in Fields)
                        foreach (var c in field.WriteCode(field.Name))
                            yield return "            " + c;
                }
                else
                    yield return "            // no-op";
                yield return "        }";
                yield return "";
                yield return "        public override IEnumerable<ID> AllIDs";
                yield return "        {";
                yield return "            get";
                yield return "            {";
                var anyID = false;
                foreach (var field in Fields)
                {
                    switch (field.Type)
                    {
                        case "ID":
                            anyID = true;
                            yield return string.Format("                yield return {0};", field.Name);
                            break;
                        case "ID?":
                            anyID = true;
                            yield return string.Format("                if ({0}.HasValue)", field.Name);
                            yield return string.Format("                    yield return {0}.Value;", field.Name);
                            break;
                        case "ID[]":
                            anyID = true;
                            yield return string.Format("                if ({0} != null)", field.Name);
                            yield return string.Format("                    foreach (var id in {0})", field.Name);
                            yield return string.Format("                        yield return id;");
                            break;
                        case "Pair<LiteralNumber, ID>[]":
                            anyID = true;
                            yield return string.Format("                if ({0} != null)", field.Name);
                            yield return string.Format("                    foreach (var p in {0})", field.Name);
                            yield return string.Format("                        yield return p.Second;");
                            break;
                    }
                }
                if (!anyID)
                    yield return "                yield break;";
                yield return "            }";
                yield return "        }";
                yield return "        #endregion";

                yield return "    }";
                yield return "}";
            }
        }

        static OpField Id(string name)
        {
            return new OpField
            {
                Type = "ID",
                Name = name,
                ReadCode = n => new[] { string.Format("{0} = new ID(codes[i++]);", n) },
                WriteCode = n => new[] { string.Format("code.Add({0}.Value);", n) }
            };
        }
        static OpField IdArray(string name)
        {
            return new OpField
            {
                Type = "ID[]",
                Init = "{ }",
                Name = name,
                ReadCode = n => new[]
                {
                    "var length = WordCount - (i - start);",
                    string.Format("{0} = new ID[length];", n),
                    "for (var k = 0; k < length; ++k)",
                    string.Format("    {0}[k] = new ID(codes[i++]);", n),
                },
                WriteCode = n => new[]
                {
                    string.Format("if ({0} != null)", n),
                    string.Format("    foreach (var val in {0})", n),
                    string.Format("        code.Add(val.Value);")
                }
            };
        }
        static OpField IdOpt(string name)
        {
            return new OpField
            {
                Type = "ID?",
                Name = name,
                ReadCode = n => new[]
                {
                    "if (i - start < WordCount)",
                    string.Format("    {0} = new ID(codes[i++]);", n),
                    "else",
                    string.Format("    {0} = null;", n),
                },
                WriteCode = n => new[]
                {
                    string.Format("if ({0}.HasValue)", n),
                    string.Format("    code.Add({0}.Value.Value);", n)
                }
            };
        }
        static OpField Nr(string name)
        {
            return new OpField
            {
                Type = "LiteralNumber",
                Name = name,
                ReadCode = n => new[] { string.Format("{0} = new LiteralNumber(codes[i++]);", n) },
                WriteCode = n => new[] { string.Format("code.Add({0}.Value);", n) }
            };
        }
        static OpField NrArray(string name)
        {
            return new OpField
            {
                Type = "LiteralNumber[]",
                Init = "{ }",
                Name = name,
                ReadCode = n => new[]
                {
                    "var length = WordCount - (i - start);",
                    string.Format("{0} = new LiteralNumber[length];", n),
                    "for (var k = 0; k < length; ++k)",
                    string.Format("    {0}[k] = new LiteralNumber(codes[i++]);", n),
                },
                WriteCode = n => new[]
                {
                    string.Format("if ({0} != null)", n),
                    string.Format("    foreach (var val in {0})", n),
                    string.Format("        code.Add(val.Value);")
                }
            };
        }
        static OpField Str(string name)
        {
            return new OpField
            {
                Type = "LiteralString",
                Name = name,
                ReadCode = n => new[] { string.Format("{0} = LiteralString.FromCode(codes, ref i);", n) },
                WriteCode = n => new[] { string.Format("{0}.WriteCode(code);", n) }
            };
        }
        static OpField PairArray(OpField op1, OpField op2, string name)
        {
            return new OpField
            {
                Type = string.Format("Pair<{0}, {1}>[]", op1.Type, op2.Type),
                Init = "{ }",
                Name = name,
                ReadCode = n => new[]
                {
                    "var length = (WordCount - (i - start)) / 2;",
                    string.Format("{0} = new Pair<{1}, {2}>[length];", n, op1.Type, op2.Type),
                    "for (var k = 0; k < length; ++k)",
                    "{",
                    string.Format("    var f = new {0}(codes[i++]);", op1.Type),
                    string.Format("    var s = new {0}(codes[i++]);", op2.Type),
                    string.Format("    {0}[k] = new Pair<{1}, {2}>(f, s);", n, op1.Type, op2.Type),
                    "}",
                },
                WriteCode = n => new[]
                {
                    string.Format("if ({0} != null)", n),
                    string.Format("    foreach (var val in {0})", n),
                    string.Format("    {{"),
                    string.Format("        code.Add(val.First.Value);"),
                    string.Format("        code.Add(val.Second.Value);"),
                    string.Format("    }}"),
                }
            };
        }

        static OpField Typed(string typeAndName) => Typed(typeAndName, typeAndName);
        static OpField Typed(string type, string name)
        {
            return new OpField
            {
                Type = type,
                Name = name,
                ReadCode = n => new[] { string.Format("{0} = ({1})codes[i++];", n, type) },
                WriteCode = n => new[] { string.Format("code.Add((uint){0});", n) }
            };
        }
        static OpField TypedArray(string typeAndName) => TypedArray(typeAndName, typeAndName);
        static OpField TypedArray(string type, string name)
        {
            return new OpField
            {
                Type = type + "[]",
                Init = "{ }",
                Name = name,
                ReadCode = n => new[]
                {
                    "var length = WordCount - (i - start);",
                    string.Format("{0} = new {1}[length];", n, type),
                    "for (var k = 0; k < length; ++k)",
                    string.Format("    {0}[k] = ({1})codes[i++];", n, type),
                },
                WriteCode = n => new[]
                {
                    string.Format("if ({0} != null)", n),
                    string.Format("    foreach (var val in {0})", n),
                    string.Format("        code.Add((uint)val);")
                }
            };
        }

        static OpCode Op(string name, params OpField[] fields)
        {
            return new OpCode
            {
                Name = name,
                Fields = fields
            };
        }

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: path/to/SpirvNet/Spirv");
                return;
            }

            var path = Path.Combine(args[0], "Ops");
            if (!Directory.Exists(path))
            {
                Console.WriteLine(path + " does not exist. calling from wrong dir?");
                return;
            }

            var cats = new HashSet<string>();

            var ops = GenOps();
            foreach (var op in ops)
            {
                var filename = Path.Combine(path, op.Cat, string.Format("Op{0}.cs", op.Name));
                //new FileInfo(filename).Directory?.Create();

                cats.Add(op.Cat);

                File.WriteAllLines(filename, op.CreateLines());
                Console.WriteLine("Wrote " + filename);
            }

            foreach (var cat in cats)
            {
                var catname = cat + "Instruction";
                var filename = Path.Combine(path, cat, string.Format("{0}.cs", catname));

                var lines = new[]
                {
                    "namespace SpirvNet.Spirv.Ops." + cat,
                    "{",
                    string.Format("    public abstract class {0}Instruction : Instruction", cat),
                    "    {",
                    "        // intentionally empty",
                    "    }",
                    "}"
                };

                File.WriteAllLines(filename, lines);
                Console.WriteLine("Wrote " + filename);
            }
        }
    }
}
