﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using SpirvNet.DotNet.CFG;
using SpirvNet.DotNet.SSA;
using SpirvNet.Spirv.Enums;
using SpirvNet.Spirv.Ops.Annotation;
using SpirvNet.Spirv.Ops.ConstantCreation;
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
        private readonly List<ConstantCreationInstruction> constants = new List<ConstantCreationInstruction>();

        // functions
        private readonly List<FunctionBuilder> functions = new List<FunctionBuilder>();

        /// <summary>
        /// ID Allocator
        /// </summary>
        public readonly IDAllocator Allocator = new IDAllocator();
        /// <summary>
        /// Type Builder
        /// </summary>
        public readonly TypeBuilder TypeBuilder;

        public ModuleBuilder(AddressingModel addressingModel = AddressingModel.Logical, MemoryModel memoryModel = MemoryModel.Simple)
        {
            TypeBuilder = new TypeBuilder(Allocator);

            opMemoryModel = new OpMemoryModel
            {
                AddressingModel = addressingModel,
                MemoryModel = memoryModel
            };
        }

        /// <summary>
        /// Sets the source language and version
        /// </summary>
        public void SetSource(SourceLanguage language, uint version)
            => opSource = new OpSource { SourceLanguage = language, Version = { Value = version } };

        /// <summary>
        /// Adds a source extension
        /// </summary>
        public void AddSourceExtension(string extension)
            => opSourceExtensions.Add(new OpSourceExtension { Extension = { Value = extension } });

        /// <summary>
        /// Adds a compile flag
        /// </summary>
        public void AddCompileFlag(string flag)
            => opCompileFlags.Add(new OpCompileFlag { Flag = { Value = flag } });

        /// <summary>
        /// Adds an extension
        /// </summary>
        public void AddExtension(string extension)
            => opExtensions.Add(new OpExtension { Name = { Value = extension } });

        /// <summary>
        /// Adds an import with a name and a given id
        /// </summary>
        public void AddImport(ID resultID, string name)
            => opImports.Add(new OpExtInstImport { Result = resultID, Name = { Value = name } });

        /// <summary>
        /// Adds an import with a name
        /// Creates a new ID and returns it
        /// </summary>
        public ID AddImport(string name)
        {
            var id = Allocator.CreateID();
            AddImport(id, name);
            return id;
        }

        /// <summary>
        /// Adds a string with a given id
        /// </summary>
        public void AddString(ID resultID, string name)
            => opStrings.Add(new OpString
            {
                Result = resultID,
                Name = { Value = name }
            });

        /// <summary>
        /// Adds a string
        /// Creates a new ID and returns it
        /// </summary>
        public ID AddString(string name)
        {
            var id = Allocator.CreateID();
            AddString(id, name);
            return id;
        }

        /// <summary>
        /// Adds a name  with a given id
        /// </summary>
        public void AddName(ID resultID, string name)
            => opNames.Add(new OpName
            {
                Target = resultID,
                Name = { Value = name }
            });

        /// <summary>
        /// Adds a member name for a given type
        /// </summary>
        public void AddMemberName(ID typeID, uint member, string name)
            => opMemberNames.Add(new OpMemberName
            {
                Type = typeID,
                Member = { Value = member },
                Name = { Value = name }
            });

        /// <summary>
        /// Adds a line debug info
        /// </summary>
        public void AddLine(ID targetID, ID fileID, uint line, uint column)
            => opLines.Add(new OpLine
            {
                Target = targetID,
                File = fileID,
                Line = { Value = line },
                Column = { Value = column }
            });

        /// <summary>
        /// Adds a new type
        /// </summary>
        public void AddType(TypeDeclarationInstruction type) => types.Add(type);
        /// <summary>
        /// Adds a new constant
        /// </summary>
        public void AddConstant(ConstantCreationInstruction constant) => constants.Add(constant);

        /// <summary>
        /// Adds a function (builder)
        /// (Can be modified until module creation)
        /// </summary>
        public void AddFunction(FunctionBuilder function) => functions.Add(function);

        /// <summary>
        /// Adds a decoration
        /// </summary>
        public void AddDecoration(AnnotationInstruction op) => decorations.Add(op);

        /// <summary>
        /// Creates a function from a C# function
        /// </summary>
        public FunctionBuilder CreateFunction(MethodDefinition method)
        {
            var cfg = new ControlFlowGraph(method);
            var builder = new FunctionBuilder(method.FullName);
            var frame = new MethodFrame(cfg, TypeBuilder, Allocator);
            frame.Build(builder);
            functions.Add(builder);
            return builder;
        }

        /// <summary>
        /// Creates the model
        /// (Instructions are shared, so do not reuse them)
        /// </summary>
        public Module CreateModule()
        {
            // last-minute creations
            {
                // register types and names
                foreach (var type in TypeBuilder.CreateTypeOps())
                    AddType(type);
                foreach (var kvp in TypeBuilder.CreateTypeNames())
                    AddName(kvp.Key, kvp.Value);

                // register constants and names
                foreach (var constant in TypeBuilder.Constants)
                    AddConstant(constant);
                foreach (var kvp in TypeBuilder.CreateConstantNames())
                    AddName(kvp.Key, kvp.Value);

                // register function types and names
                foreach (var func in functions)
                    AddType(func.FunctionType);
                foreach (var func in functions)
                    opNames.AddRange(func.AdditionalNames);
            }

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
            // constants
            mod.Instructions.AddRange(constants);

            // functions
            foreach (var func in functions)
                mod.Instructions.AddRange(func.GenerateInstructions());

            return mod;
        }
    }
}
