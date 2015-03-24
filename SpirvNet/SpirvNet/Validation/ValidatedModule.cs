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
using SpirvNet.Spirv.Ops.Debug;
using SpirvNet.Spirv.Ops.Extension;
using SpirvNet.Spirv.Ops.ModeSetting;

namespace SpirvNet.Validation
{
    /// <summary>
    /// A validated and analysed version of a module
    /// </summary>
    public class ValidatedModule
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
                        else throw new ValidationException(inst, "Missing OpMemoryModel instruction.");
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
                        break;

                    // functions
                    case ModuleValidationState.MV13_0_OpFunction:
                        break;
                    case ModuleValidationState.MV13_1_OpFunctionParameters:
                        break;
                    case ModuleValidationState.MV13_2_0_OpFunctionBlockLabel:
                        break;
                    case ModuleValidationState.MV13_2_1_OpFunctionBlockVars:
                        break;
                    case ModuleValidationState.MV13_2_2_OpFunctionBlockInstruction:
                        break;
                    case ModuleValidationState.MV13_2_3_OpFunctionBlockBranch:
                        break;
                    case ModuleValidationState.MV13_3_OpFunctionEnd:
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
    }
}
