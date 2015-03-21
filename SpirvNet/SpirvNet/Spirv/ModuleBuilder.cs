using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;
using SpirvNet.Spirv.Ops.Annotation;
using SpirvNet.Spirv.Ops.Debug;
using SpirvNet.Spirv.Ops.Extension;
using SpirvNet.Spirv.Ops.ModeSetting;
using SpirvNet.Spirv.Ops.TypeDeclaration;

namespace SpirvNet.Spirv
{
    /// <summary>
    /// Helper class for building a module
    /// 
    /// The instructions of a SPIR-V module must be in the following order:
    ///  1. Optional OpSource instruction stating the source language and version number.
    ///  2. Optional OpSourceExtension instructions (extensions used within the source language).
    ///  3. Optional OpCompileFlag instructions.
    ///  4. Optional OpExtension instructions (extensions to SPIR-V).
    ///  5. Optional OpExtInstImport instructions.
    ///  6. The single required OpMemoryModel instruction.
    ///  7. All entry point declarations, using OpEntryPoint.
    ///  8. All execution mode declarations, using OpExecutionMode.
    ///  9. All debug and annotation instructions, which must be in the following order:
    ///  a. all OpString
    ///  b. all OpName and all OpMemberName
    ///  c. all OpLine
    ///  d. all decoration instructions (OpDecorate, OpMemberDecorate, OpGroupDecorate, OpGroupMemberDecorate, and OpDecorationGroup).
    ///  10. All type declarations (OpTypeXXX instructions), all constant instructions, and all global variable declarations (all OpVariable
    ///      instructions whose Storage Class is not Function). All operands in all these instructions must be declared before
    ///      being used. Otherwise, they can be in any order.
    ///  11. All function definitions. A function definition is as follows.
    ///       a. Function declaration, using OpFunction.
    ///       b. Function parameter declarations, using OpFunctionParameter.
    ///       c. Block
    ///       d. Block
    ///       e. . . .
    ///       f. Function end, using OpFunctionEnd
    /// 
    /// </summary>
    public class ModuleBuilder
    {
        // Optional header
        private OpSource opSource;
        private readonly List<OpSourceExtension> opSourceExtensions = new List<OpSourceExtension>();
        private readonly List<OpCompileFlag> opCompileFlags = new List<OpCompileFlag>();
        private readonly List<OpExtension> opExtensions = new List<OpExtension>();
        private readonly List<OpExtInstImport> opImports = new List<OpExtInstImport>();

        // mem model
        private readonly OpMemoryModel opMemoryModel;

        // all entry point decls
        private readonly List<OpEntryPoint> opEntryPoints = new List<OpEntryPoint>();

        // all execution mode decls
        private readonly List<OpExecutionMode> opExecutionModes = new List<OpExecutionMode>();

        // debug instructions
        private readonly List<OpString> opStrings = new List<OpString>();
        private readonly List<OpName> opNames = new List<OpName>();
        private readonly List<OpMemberName> opMemberNames = new List<OpMemberName>();
        private readonly List<OpLine> opLines = new List<OpLine>();

        // decorations
        private readonly List<AnnotationInstruction> decorations = new List<AnnotationInstruction>();

        // types
        private readonly List<TypeDeclarationInstruction> types = new List<TypeDeclarationInstruction>();

        // functions
        private readonly List<FunctionBuilder> functions = new List<FunctionBuilder>();

        public ModuleBuilder(AddressingModel addressingModel = AddressingModel.Logical, MemoryModel memoryModel = MemoryModel.Simple)
        {
            opMemoryModel = new OpMemoryModel
            {
                AddressingModel = addressingModel,
                MemoryModel = memoryModel
            };
        }

        /// <summary>
        /// Adds a new type
        /// </summary>
        public void AddType(TypeDeclarationInstruction type) => types.Add(type);

        /// <summary>
        /// Adds a function (builder)
        /// (Can be modified until module creation)
        /// </summary>
        public void AddFunction(FunctionBuilder function) => functions.Add(function);

        /// <summary>
        /// Creates the model
        /// (Instructions are shared, so do not reuse them)
        /// </summary>
        public Module CreateModule()
        {
            var mod = new Module();

            // optional header
            if (opSource != null) mod.Instructions.Add(opSource);
            mod.Instructions.AddRange(opSourceExtensions);
            mod.Instructions.AddRange(opCompileFlags);
            mod.Instructions.AddRange(opExtensions);
            mod.Instructions.AddRange(opImports);

            // memory model
            mod.Instructions.Add(opMemoryModel);

            // entry points
            mod.Instructions.AddRange(opEntryPoints);

            // execution mode
            mod.Instructions.AddRange(opExecutionModes);

            // debug instructions
            mod.Instructions.AddRange(opStrings);
            mod.Instructions.AddRange(opNames);
            mod.Instructions.AddRange(opMemberNames);
            mod.Instructions.AddRange(opLines);

            // decorations
            mod.Instructions.AddRange(decorations);

            // types
            mod.Instructions.AddRange(types);

            // functions
            foreach (var func in functions)
                mod.Instructions.AddRange(func.GenerateInstructions());

            return mod;
        }
    }
}
