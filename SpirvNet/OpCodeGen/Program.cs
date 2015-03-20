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
            opCategory = "Annotation";
            yield return Op("DecorationGroup", Id("Result"));
            yield return Op("Decorate", Id("Target"), Typed("Decoration")); // TODO: Special
            yield return Op("MemberDecorate", Id("StructureType"), Nr("Member"), Typed("Decoration", "Decoration")); // TODO: Special
            yield return Op("GroupDecorate", Id("DecorationGroup")); // TODO: How do these work? array of IDs
            yield return Op("GroupMemberDecorate", Id("DecorationGroup")); // TODO: How do these work? array of IDs

            opCategory = "Extension";
            yield return Op("Extension", Str("Name"));
            yield return Op("ExtInstImport", Id("Result"), Str("Name"));
            yield return Op("ExtInst", Id("ResultType"), Id("Result"), Id("Set"), Nr("Instruction")); // TODO: Operands

            opCategory = "ModeSetting";
            yield return Op("MemoryModel", Typed("AddressingModel"), Typed("MemoryModel"));
            yield return Op("EntryPoint", Typed("ExecutionModel"), Id("EntryPoint"));
            yield return Op("ExecutionMode", Id("EntryPoint"), Typed("ExecutionMode")); // TODO: Literals
            yield return Op("CompileFlag", Str("Flag")).Compat("Kernel");

            opCategory = "TypeDeclaration";
            yield return Op("TypeVoid", Id("Result"));
            yield return Op("TypeBool", Id("Result"));
            yield return Op("TypeInt", Id("Result"), Nr("Width"), Nr("Signedness"));
            yield return Op("TypeFloat", Id("Result"), Nr("Width"));
            yield return Op("TypeVector", Id("Result"), Id("ComponentType"), Nr("ComponentCount"));
            yield return Op("TypeMatrix", Id("Result"), Id("ColumnType"), Nr("ColumnCount"));
            yield return Op("TypeSampler", Id("Result"), Id("SampledType"), Typed("Dim"), Nr("Content"), Nr("Arrayed"), Nr("Compare"), Nr("MS")); // TODO: optional qualifier
            yield return Op("TypeFilter", Id("Result"));
            yield return Op("TypeArray", Id("Result"), Id("ElementType"), Id("Length"));
            yield return Op("TypeRuntimeArray", Id("Result"), Id("ElementType")).Compat("Shader");
            yield return Op("TypeStruct", Id("Result")); // TODO: Member types
            yield return Op("TypeOpaque", Id("Result"), Str("OpaqueType")).Compat("Kernel");
            yield return Op("TypePointer", Id("Result"), Typed("StorageClass"), Id("Type"));
            yield return Op("TypeFunction", Id("Result"), Id("ReturnType")); // TODO: Parameter types
            yield return Op("TypeEvent", Id("Result")).Compat("Kernel");
            yield return Op("TypeDeviceEvent", Id("Result")).Compat("Kernel");
            yield return Op("TypeReserveId", Id("Result")).Compat("Kernel");
            yield return Op("TypeQueue", Id("Result")).Compat("Kernel");
            yield return Op("TypePipe", Id("Result"), Id("Type"), Typed("AccessQualifier")).Compat("Kernel");

            opCategory = "ConstantCreation";
            yield return Op("ConstantTrue", Id("ResultType"), Id("Result"));
            yield return Op("ConstantFalse", Id("ResultType"), Id("Result"));
            yield return Op("Constant", Id("ResultType"), Id("Result"), Nr("Value")); // TODO: multiple words?
            yield return Op("ConstantComposite", Id("ResultType"), Id("Result")); // TODO: Constituents IDs
            yield return Op("ConstantSampler", Id("ResultType"), Id("Result"), Nr("Mode"), Nr("Param"), Nr("Filter")).Compat("Kernel");
            yield return Op("ConstantNullPointer", Id("ResultType"), Id("Result")).Compat("Addr");
            yield return Op("ConstantNullObject", Id("ResultType"), Id("Result")).Compat("Kernel");
            yield return Op("SpecConstantTrue", Id("ResultType"), Id("Result")).Compat("Shader");
            yield return Op("SpecConstantFalse", Id("ResultType"), Id("Result")).Compat("Shader");
            yield return Op("SpecConstant", Id("ResultType"), Id("Result"), Nr("Value")).Compat("Shader"); // TODO: multiple words?
            yield return Op("SpecConstantComposite", Id("ResultType"), Id("Result")).Compat("Shader"); // TODO: Constituents IDs

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
        }

        class OpField
        {
            public string Type;
            public string Name;
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
                yield return "namespace SpirvNet.Spirv.Ops." + Cat;
                yield return "{";
                yield return "    /// <summary>";
                if (string.IsNullOrEmpty(comment))
                    yield return "    /// TODO: Copy comment from https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf";
                foreach (var line in comment.Split('\n'))
                    yield return "    /// " + line.Replace("<id>", " ID").Replace("  ID", " ID");
                yield return "    /// </summary>";
                yield return string.Format("    [DependsOn({0})]", compatibilities.Select(c => "LanguageCapability." + c).Aggregate((s1, s2) => s1 + " | " + s2));
                yield return string.Format("    public sealed class Op{0} : Instruction", Name);
                yield return "    {";
                yield return string.Format("        public override OpCode OpCode => OpCode.{0};", Name);
                foreach (var field in Fields)
                    yield return string.Format("        public {0} {1};", field.Type, field.Name);
                yield return "    }";
                yield return "}";
            }
        }

        static OpField Id(string name)
        {
            return new OpField
            {
                Type = "ID",
                Name = name
            };
        }
        static OpField IdArray(string name)
        {
            return new OpField
            {
                Type = "ID[]",
                Name = name
            };
        }
        static OpField IdOpt(string name)
        {
            return new OpField
            {
                Type = "ID?",
                Name = name
            };
        }
        static OpField Nr(string name)
        {
            return new OpField
            {
                Type = "LiteralNumber",
                Name = name
            };
        }
        static OpField Str(string name)
        {
            return new OpField
            {
                Type = "LiteralString",
                Name = name
            };
        }

        static OpField Typed(string typeAndName) => Typed(typeAndName, typeAndName);
        static OpField Typed(string type, string name)
        {
            return new OpField
            {
                Type = type,
                Name = name
            };
        }
        static OpField TypedArray(string typeAndName) => TypedArray(typeAndName, typeAndName);
        static OpField TypedArray(string type, string name)
        {
            return new OpField
            {
                Type = type + "[]",
                Name = name
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

            var ops = GenOps();
            foreach (var op in ops)
            {
                var filename = Path.Combine(path, op.Cat, string.Format("Op{0}.cs", op.Name));
                //new FileInfo(filename).Directory?.Create();

                File.WriteAllLines(filename, op.CreateLines());
                Console.WriteLine("Wrote " + filename);
            }
        }
    }
}
