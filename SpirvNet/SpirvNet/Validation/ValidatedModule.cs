using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SpirvNet.Spirv;
using SpirvNet.Spirv.Enums;
using SpirvNet.Spirv.Ops.Annotation;
using SpirvNet.Spirv.Ops.ConstantCreation;
using SpirvNet.Spirv.Ops.Debug;
using SpirvNet.Spirv.Ops.Extension;
using SpirvNet.Spirv.Ops.FlowControl;
using SpirvNet.Spirv.Ops.Function;
using SpirvNet.Spirv.Ops.Memory;
using SpirvNet.Spirv.Ops.ModeSetting;
using SpirvNet.Spirv.Ops.TypeDeclaration;

namespace SpirvNet.Validation
{
    /// <summary>
    /// A validated and analysed version of a module
    /// </summary>
    public class ValidatedModule : ITypeProvider
    {
        /// <summary>
        /// Reference to original module
        /// </summary>
        public readonly Module OriginalModule;

        /// <summary>
        /// Max bounds (tight)
        /// </summary>
        public readonly uint Bound;

        /// <summary>
        /// Minimal set of required capabilities
        /// </summary>
        public readonly LanguageCapability Capabilities = LanguageCapability.None;

        /// <summary>
        /// Source language version
        /// </summary>
        public uint SourceLanguageVersion { get; private set; }
        /// <summary>
        /// Source language
        /// </summary>
        public SourceLanguage SourceLanguage { get; private set; }
        /// <summary>
        /// Addressing model
        /// </summary>
        public AddressingModel AddressingModel { get; private set; }
        /// <summary>
        /// Memory model
        /// </summary>
        public MemoryModel MemoryModel { get; private set; }

        /// <summary>
        /// List of source extensions
        /// </summary>
        public readonly List<string> SourceExtensions = new List<string>();
        /// <summary>
        /// List of ompile flags
        /// </summary>
        public readonly List<string> CompileFlags = new List<string>();
        /// <summary>
        /// List of SPIR-V extensions
        /// </summary>
        public readonly List<string> Extensions = new List<string>();

        /// <summary>
        /// List of entry points
        /// </summary>
        public readonly List<EntryPoint> EntryPoints = new List<EntryPoint>();

        /// <summary>
        /// Locations and location info used by the module
        /// </summary>
        public readonly Location[] Locations;

        private ValidatedModule(Module originalModule)
        {
            OriginalModule = originalModule;

            // tight bounds
            Bound = originalModule.Instructions.Max(i => i.AllIDs.Select(id => id.Value).Concat(new[] { 0u }).Max()) + 1;

            // tight caps
            foreach (var instruction in originalModule.Instructions)
                Capabilities |= instruction.RequiredCapabilities();

            // locs
            Locations = new Location[Bound];
            for (var id = 0u; id < Locations.Length; ++id)
                Locations[id] = new Location(id);
        }

        /// <summary>
        /// Throws a validation exception if the location is not of a given type
        /// </summary>
        private void LocationTypeCheck(ID id, LocationType type, Instruction instruction)
        {
            RangeCheck(id, instruction);
            if (Locations[id.Value].LocationType != type)
                throw new ValidationException(instruction, "Location " + id + " is not of type " + type);
        }

        /// <summary>
        /// Throws a validation exception if id not in range
        /// </summary>
        private void RangeCheck(ID id, Instruction instruction)
        {
            if (id.Value >= Locations.Length)
                throw new ValidationException(instruction, "ID access out of bounds.");
        }

        /// <summary>
        /// Asserts that a location is empty
        /// </summary>
        private void AssertEmptyLocation(Instruction instruction)
        {
            if (!instruction.ResultID.HasValue)
                throw new NotSupportedException("Instruction has no result ID");
            AssertEmptyLocation(instruction.ResultID.Value.Value, instruction);
        }

        private void AssertEmptyLocation(uint id, Instruction instruction)
        {
            if (id >= Locations.Length)
                throw new ValidationException(instruction, "ID access out of bounds.");

            if (Locations[id].LocationType != LocationType.None)
                throw new ValidationException(instruction, "ID " + id + " is already in use.");
        }

        /// <summary>
        /// Analyses and validates a module
        /// </summary>
        [SuppressMessage("ReSharper", "HeuristicUnreachableCode")]
        private void Analyse()
        {
            var instructions = OriginalModule.Instructions;
            var i = 0;
            var state = ModuleValidationState.MV00_OpSource;

            var memberNames = new List<OpMemberName>();

            ValidatedFunction currFunc = null;
            ValidatedBlock currBlock = null;

            while (i < instructions.Count)
            {
                var inst = instructions[i];

                switch (state)
                {
                    // header
                    case ModuleValidationState.MV00_OpSource:
                        if (inst is OpSource)
                        {
                            var op = inst as OpSource;
                            SourceLanguage = op.SourceLanguage;
                            SourceLanguageVersion = op.Version.Value;
                            ++i;
                        }
                        // no else: this is only one
                        state = ModuleValidationState.MV01_OpSourceExtension;
                        break;
                    case ModuleValidationState.MV01_OpSourceExtension:
                        if (inst is OpSourceExtension)
                        {
                            var op = inst as OpSourceExtension;
                            SourceExtensions.Add(op.Extension.Value);
                            ++i;
                        }
                        else // Op*
                            state = ModuleValidationState.MV02_OpCompileFlag;
                        break;
                    case ModuleValidationState.MV02_OpCompileFlag:
                        if (inst is OpCompileFlag)
                        {
                            var op = inst as OpCompileFlag;
                            CompileFlags.Add(op.Flag.Value);
                            ++i;
                        }
                        else // Op*
                            state = ModuleValidationState.MV03_OpExtension;
                        break;
                    case ModuleValidationState.MV03_OpExtension:
                        if (inst is OpExtension)
                        {
                            var op = inst as OpExtension;
                            Extensions.Add(op.Name.Value);
                            ++i;
                        }
                        else // Op*
                            state = ModuleValidationState.MV04_OpExtInstImport;
                        break;
                    case ModuleValidationState.MV04_OpExtInstImport:
                        if (inst is OpExtInstImport)
                        {
                            var op = inst as OpExtInstImport;
                            AssertEmptyLocation(op);
                            Locations[op.Result.Value].FillFromExtInstImport(op);
                            ++i;
                        }
                        else // Op*
                            state = ModuleValidationState.MV05_OpMemoryModel;
                        break;

                    // memory model
                    case ModuleValidationState.MV05_OpMemoryModel:
                        if (inst is OpMemoryModel)
                        {
                            var op = inst as OpMemoryModel;
                            AddressingModel = op.AddressingModel;
                            MemoryModel = op.MemoryModel;
                            state = ModuleValidationState.MV06_OpEntryPoint;
                            ++i;
                        }
                        else throw new ValidationException(inst, "Unexpected instruction. Expected OpMemoryModel instruction.");
                        break;

                    // entry points
                    case ModuleValidationState.MV06_OpEntryPoint:
                        if (inst is OpEntryPoint)
                        {
                            var op = inst as OpEntryPoint;
                            EntryPoints.Add(new EntryPoint(op.EntryPoint, op.ExecutionModel));
                            ++i;
                        }
                        else // Op*
                            state = ModuleValidationState.MV07_OpExecutionMode;
                        break;
                    case ModuleValidationState.MV07_OpExecutionMode:
                        if (inst is OpExecutionMode)
                        {
                            var op = inst as OpExecutionMode;
                            var ep = EntryPoints.FirstOrDefault(p => p.EnryPointID == op.EntryPoint);
                            if (ep == null)
                                throw new ValidationException(op, "OpExecutionMode for non-entry-point " + op.EntryPoint);
                            ++i;
                        }
                        else // Op*
                            state = ModuleValidationState.MV08_OpString;
                        break;

                    // debug and annotation instructions
                    case ModuleValidationState.MV08_OpString:
                        if (inst is OpString)
                        {
                            var op = inst as OpString;
                            AssertEmptyLocation(op);
                            Locations[op.Result.Value].FillFromString(op);
                            ++i;
                        }
                        else // Op*
                            state = ModuleValidationState.MV09_OpNameAndMemberName;
                        break;
                    case ModuleValidationState.MV09_OpNameAndMemberName:
                        if (inst is OpName)
                        {
                            var op = inst as OpName;
                            RangeCheck(op.Target, op);
                            Locations[op.Target.Value].SetDebugName(op.Name.Value);
                            ++i;
                        }
                        else if (inst is OpMemberName)
                        {
                            var op = inst as OpMemberName;
                            memberNames.Add(op);
                            ++i;
                        }
                        else // Op*
                            state = ModuleValidationState.MV10_OpLine;
                        break;
                    case ModuleValidationState.MV10_OpLine:
                        if (inst is OpLine)
                        {
                            var op = inst as OpLine;
                            RangeCheck(op.Target, op);
                            LocationTypeCheck(op.File, LocationType.String, op);
                            Locations[op.Target.Value].AddLineInfo(Locations[op.File.Value].Name, op.Line.Value, op.Column.Value);
                            ++i;
                        }
                        else // Op*
                            state = ModuleValidationState.MV11_OpDecorations;
                        break;

                    // decorations
                    case ModuleValidationState.MV11_OpDecorations:
                        if (inst is OpDecorate)
                        {
                            var op = inst as OpDecorate;
                            RangeCheck(op.Target, op);
                            Locations[op.Target.Value].AddDecoration(op);
                            ++i;
                        }
                        else if (inst is OpMemberDecorate)
                        {
                            var op = inst as OpMemberDecorate;
                            RangeCheck(op.StructureType, op);
                            Locations[op.StructureType.Value].AddMemberDecoration(op);
                            ++i;
                        }
                        else if (inst is OpDecorationGroup)
                        {
                            var op = inst as OpDecorationGroup;
                            AssertEmptyLocation(op);
                            Locations[op.Result.Value].FillFromDecorationGroup(op);
                            ++i;
                        }
                        else if (inst is OpGroupDecorate)
                        {
                            var op = inst as OpGroupDecorate;
                            LocationTypeCheck(op.DecorationGroup, LocationType.DecorationGroup, op);
                            foreach (var target in op.Targets)
                            {
                                RangeCheck(target, op);
                                foreach (var decoration in Locations[op.DecorationGroup.Value].Decorations)
                                    Locations[target.Value].AddDecoration(decoration);
                            }
                            ++i;
                        }
                        else if (inst is OpGroupMemberDecorate)
                        {
                            var op = inst as OpGroupMemberDecorate;
                            LocationTypeCheck(op.DecorationGroup, LocationType.DecorationGroup, op);
                            foreach (var target in op.Targets)
                            {
                                RangeCheck(target, op);
                                foreach (var decoration in Locations[op.DecorationGroup.Value].Decorations)
                                    Locations[target.Value].AddDecoration(decoration); // TODO: somehow, this should be a member decoration
                            }
                            ++i;
                        }
                        else // Op*
                            state = ModuleValidationState.MV12_OpTypesConstantsGlobalVars;
                        break;

                    // types, constants, global vars
                    case ModuleValidationState.MV12_OpTypesConstantsGlobalVars:
                        if (inst.IsTypeDeclaration)
                        {
                            var res = inst.ResultID.Value;
                            AssertEmptyLocation(inst);
                            Locations[res.Value].FillFromType(inst as TypeDeclarationInstruction, this);
                            ++i;
                        }
                        else if (inst.IsConstantCreation)
                        {
                            var res = inst.ResultID.Value;
                            AssertEmptyLocation(inst);
                            Locations[res.Value].FillFromConstant(inst as ConstantCreationInstruction, this);
                            ++i;
                        }
                        else if (inst is OpVariable)
                        {
                            ++i;
                            throw new NotImplementedException();
                        }
                        else if (inst is OpVariableArray)
                        {
                            ++i;
                            throw new NotImplementedException();
                        }
                        else // Op*
                            state = ModuleValidationState.MV13_0_OpFunction;
                        break;

                    // functions
                    case ModuleValidationState.MV13_0_OpFunction:
                        if (inst is OpFunction)
                        {
                            if (currFunc != null)
                                throw new ValidationException(inst, "Nested functions are not allowed.");

                            var op = inst as OpFunction;
                            AssertEmptyLocation(op);
                            Locations[op.Result.Value].FillFromFunction(op, this);
                            currFunc = new ValidatedFunction(Locations[op.Result.Value]);
                            currBlock = null;
                            ++i;
                            state = ModuleValidationState.MV13_1_OpFunctionParameters;
                        }
                        else throw new ValidationException(inst, "Unexpected instruction. Expected OpFunction instruction.");
                        break;
                    case ModuleValidationState.MV13_1_OpFunctionParameters:
                        if (inst is OpFunctionParameter)
                        {
                            if (currFunc == null)
                                throw new ValidationException(inst, "Function parameter outside of a function.");

                            var op = inst as OpFunctionParameter;
                            AssertEmptyLocation(op);
                            Locations[op.Result.Value].FillFromFunctionParameter(op, this);
                            currFunc.DeclarationLocation.AddFunctionParameter(Locations[op.Result.Value], op, this);
                            ++i;
                        }
                        else // Op*
                            state = ModuleValidationState.MV13_2_0_OpFunctionBlockLabel;
                        break;
                    // blocks
                    case ModuleValidationState.MV13_2_0_OpFunctionBlockLabel:
                        if (inst is OpLabel)
                        {
                            if (currFunc == null)
                                throw new ValidationException(inst, "Block label outside of a function.");

                            if (currBlock != null)
                                throw new ValidationException(inst, "No nested blocks.");

                            var op = inst as OpLabel;
                            AssertEmptyLocation(op);
                            currBlock = new ValidatedBlock(op, currFunc);
                            Locations[op.Result.Value].FillFromLabel(op, currBlock);
                            ++i;

                            state = ModuleValidationState.MV13_2_1_OpFunctionBlockVars;
                        }
                        else throw new ValidationException(inst, "Unexpected instruction. Expected OpLabel instruction (first of block).");
                        break;
                    case ModuleValidationState.MV13_2_1_OpFunctionBlockVars:
                        if (inst is OpVariable)
                        {
                            if (currFunc == null)
                                throw new ValidationException(inst, "Block variable outside of a function.");

                            if (currBlock != currFunc.FirstBlock)
                                throw new ValidationException(inst, "Block variables are only allowed in topmost block.");

                            var op = inst as OpVariable;
                            AssertEmptyLocation(op);
                            Locations[op.Result.Value].FillFromFunctionInstruction(op, currBlock, this);
                            ++i;
                        }
                        else if (inst is OpVariableArray)
                        {
                            if (currFunc == null)
                                throw new ValidationException(inst, "Block variable outside of a function.");

                            if (currBlock != currFunc.FirstBlock)
                                throw new ValidationException(inst, "Block variables are only allowed in topmost block.");

                            var op = inst as OpVariableArray;
                            AssertEmptyLocation(op);
                            Locations[op.Result.Value].FillFromFunctionInstruction(op, currBlock, this);
                            ++i;
                        }
                        else // Op*
                            state = ModuleValidationState.MV13_2_2_OpFunctionBlockPhis;
                        break;
                    case ModuleValidationState.MV13_2_2_OpFunctionBlockPhis:
                        if (inst is OpPhi)
                        {
                            if (currFunc == null)
                                throw new ValidationException(inst, "Block OpPhi outside of a function.");

                            var op = inst as OpPhi;
                            AssertEmptyLocation(op);
                            Locations[op.Result.Value].FillFromFunctionInstruction(op, currBlock, this);
                            ++i;
                        }
                        else // Op*
                            state = ModuleValidationState.MV13_2_3_OpFunctionBlockInstruction;
                        break;
                    case ModuleValidationState.MV13_2_3_OpFunctionBlockInstruction:
                        if (inst.IsFlowControl)
                        {
                            if (inst is OpBranch)
                            {
                                throw new NotImplementedException("TODO");
                            }
                            else if (inst is OpBranchConditional)
                            {
                                throw new NotImplementedException("TODO");
                            }
                            else if (inst is OpSwitch)
                            {
                                throw new NotImplementedException("TODO");
                            }
                            else if (inst is OpKill)
                            {
                                throw new NotImplementedException("TODO");
                            }
                            else if (inst is OpReturn)
                            {
                                throw new NotImplementedException("TODO");
                            }
                            else if (inst is OpReturnValue)
                            {
                                throw new NotImplementedException("TODO");
                            }
                            else if (inst is OpUnreachable)
                            {
                                throw new NotImplementedException("TODO");
                            }
                            else if (inst is OpPhi)
                            {
                                throw new ValidationException(inst, "Phis may only appear at the start of a block.");
                            }
                            else throw new NotImplementedException("Not implemented");

                            ++i;
                            // new block
                            currBlock = null;
                            state = ModuleValidationState.MV13_2_0_OpFunctionBlockLabel;
                        }
                        else if (inst is OpLabel)
                        {
                            throw new ValidationException(inst, "Nested blocks are not allowed.");
                        }
                        else if (inst is OpFunctionEnd)
                        {
                            throw new ValidationException(inst, "Function cannot end before block ended.");
                        }
                        else
                        {
                            var op = inst;
                            if (op.ResultID.HasValue)
                            {
                                AssertEmptyLocation(op);
                                Locations[op.ResultID.Value.Value].FillFromFunctionInstruction(op, currBlock, this);
                            }
                            currBlock.AddInstruction(op);
                            ++i;
                        }
                        break;
                    // function end
                    case ModuleValidationState.MV13_3_OpFunctionEnd:
                        if (inst is OpFunctionEnd)
                        {
                            if (currFunc == null)
                                throw new ValidationException(inst, "Must be inside an OpFunction.");

                            currFunc = null;
                            currBlock = null;
                            ++i;
                            state = ModuleValidationState.MV13_0_OpFunction;
                        }
                        else throw new ValidationException(inst, "Unexpected instruction. Expected OpFunctionEnd instruction.");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // TODO's
            throw new NotImplementedException("Entry point validation");
            throw new NotImplementedException("Validate " + memberNames);
            throw new NotImplementedException("Validate member decorations");
        }

        /// <summary>
        /// Creates a validated version of the module (or throws a ValidationException)
        /// </summary>
        public static ValidatedModule Validate(Module module)
        {
            var vmod = new ValidatedModule(module);
            vmod.Analyse();
            return vmod;
        }

        public SpirvType TypeFor(ID typeId, Instruction instruction)
        {
            LocationTypeCheck(typeId, LocationType.Type, instruction);
            return Locations[typeId.Value].SpirvType;
        }
    }
}
