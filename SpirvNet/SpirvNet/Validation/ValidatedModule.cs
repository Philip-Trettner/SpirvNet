using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebugPage;
using NUnit.Framework;
using SpirvNet.Helper;
using SpirvNet.Spirv;
using SpirvNet.Spirv.Enums;
using SpirvNet.Spirv.Ops;
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
        /// List of validated functions
        /// </summary>
        public readonly List<ValidatedFunction> Functions = new List<ValidatedFunction>();

        /// <summary>
        /// Locations and location info used by the module
        /// </summary>
        public readonly Location[] Locations;

        /// <summary>
        /// All contained types
        /// </summary>
        public readonly List<SpirvType> Types = new List<SpirvType>();

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
        /// Throws a validation exception if the location is not an intermediate with a given predicate
        /// </summary>
        private void IntermediateTypeCheck(ID id, Func<SpirvType, bool> pred, Instruction instruction, string msg)
        {
            LocationTypeCheck(id, LocationType.Intermediate, instruction);
            if (!pred(Locations[id.Value].SpirvType))
                throw new ValidationException(instruction, msg);
        }

        /// <summary>
        /// Throws a validation exception if id not in range
        /// </summary>
        private void RangeCheck(ID id, Instruction instruction)
        {
            if (id.Value == 0)
                throw new ValidationException(instruction, "ID cannot be zero.");
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
            if (OriginalModule.Bound < Bound)
                throw new ValidationException(null, "Bound too small.");
            if (OriginalModule.Instructions.Count == 0)
                throw new ValidationException(null, "No instructions found.");

            foreach (var instruction in OriginalModule.Instructions)
                if (instruction is OpUnknown)
                    throw new ValidationException(instruction, "Unknown instructions cannot be validated.");

            var instructions = OriginalModule.Instructions;
            var i = 0;
            var state = ModuleValidationState.MV00_OpSource;

            var memberNames = new List<OpMemberName>();

            ValidatedFunction currFunc = null;
            ValidatedBlock currBlock = null;
            var branches = new List<Tuple<FlowControlInstruction, ValidatedBlock, ID, uint?>>();

            // range checks
            foreach (var op in instructions)
                foreach (var id in op.AllIDs)
                    RangeCheck(id, op);

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
                            EntryPoints.Add(new EntryPoint(op.EntryPoint, op.ExecutionModel, op));
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
                            Types.Add(Locations[res.Value].SpirvType);
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
                            LocationTypeCheck(op.FunctionType, LocationType.Type, op);
                            if (!Locations[op.FunctionType.Value].SpirvType.IsFunction)
                                throw new ValidationException(op, "FunctionType not a function type.");
                            currFunc = new ValidatedFunction(Locations[op.Result.Value], Locations[op.FunctionType.Value].SpirvType, this);
                            Functions.Add(currFunc);
                            Locations[op.Result.Value].FillFromFunction(op, currFunc, this);
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
                            Locations[op.Result.Value].FillFromFunctionParameter(op, currFunc, this);
                            currFunc.DeclarationLocation.AddFunctionParameter(Locations[op.Result.Value], op, this);
                            currFunc.ParameterLocations.Add(Locations[op.Result.Value]);
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
                            currBlock = new ValidatedBlock(op, currFunc, currFunc.Blocks.Count);
                            currFunc.AddBlock(currBlock);
                            Locations[op.Result.Value].FillFromLabel(op, currBlock);
                            ++i;

                            state = ModuleValidationState.MV13_2_1_OpFunctionBlockVars;
                        }
                        else if (inst is OpFunctionEnd)
                        {
                            if (currFunc == null)
                                throw new ValidationException(inst, "Must be inside an OpFunction.");

                            currFunc = null;
                            currBlock = null;
                            ++i;
                            state = ModuleValidationState.MV13_0_OpFunction;
                        }
                        else throw new ValidationException(inst, "Unexpected instruction. Expected OpLabel instruction (first of block).");
                        break;
                    case ModuleValidationState.MV13_2_1_OpFunctionBlockVars:
                        if (inst is OpVariable)
                        {
                            if (currFunc == null)
                                throw new ValidationException(inst, "Block variable outside of a function.");

                            if (currBlock != currFunc.StartBlock)
                                throw new ValidationException(inst, "Block variables are only allowed in topmost block.");

                            var op = inst as OpVariable;
                            AssertEmptyLocation(op);
                            Locations[op.Result.Value].FillFromFunctionInstruction(op, currBlock, this);
                            currBlock.Instructions.Add(op);
                            ++i;
                        }
                        else if (inst is OpVariableArray)
                        {
                            if (currFunc == null)
                                throw new ValidationException(inst, "Block variable outside of a function.");

                            if (currBlock != currFunc.StartBlock)
                                throw new ValidationException(inst, "Block variables are only allowed in topmost block.");

                            var op = inst as OpVariableArray;
                            AssertEmptyLocation(op);
                            Locations[op.Result.Value].FillFromFunctionInstruction(op, currBlock, this);
                            currBlock.Instructions.Add(op);
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
                            currBlock.Instructions.Add(op);
                            ++i;
                        }
                        else // Op*
                            state = ModuleValidationState.MV13_2_3_OpFunctionBlockInstruction;
                        break;
                    case ModuleValidationState.MV13_2_3_OpFunctionBlockInstruction:
                        if (currFunc == null)
                            throw new ValidationException(inst, "FlowControl outside of a function.");

                        if (inst.IsFlowControl)
                        {
                            if (inst is OpBranch)
                            {
                                var op = inst as OpBranch;
                                RangeCheck(op.TargetLabel, op);
                                branches.Add(new Tuple<FlowControlInstruction, ValidatedBlock, ID, uint?>(op, currBlock, op.TargetLabel, null));
                            }
                            else if (inst is OpBranchConditional)
                            {
                                var op = inst as OpBranchConditional;
                                RangeCheck(op.TrueLabel, op);
                                RangeCheck(op.FalseLabel, op);
                                branches.Add(new Tuple<FlowControlInstruction, ValidatedBlock, ID, uint?>(op, currBlock, op.FalseLabel, 0u));
                                branches.Add(new Tuple<FlowControlInstruction, ValidatedBlock, ID, uint?>(op, currBlock, op.TrueLabel, 1u));
                            }
                            else if (inst is OpSwitch)
                            {
                                var op = inst as OpSwitch;
                                RangeCheck(op.Default, op);
                                branches.Add(new Tuple<FlowControlInstruction, ValidatedBlock, ID, uint?>(op, currBlock, op.Default, null));
                                foreach (var kvp in op.Target)
                                    branches.Add(new Tuple<FlowControlInstruction, ValidatedBlock, ID, uint?>(op, currBlock, kvp.Second, kvp.First.Value));
                            }
                            else if (inst is OpKill)
                            {
                                // no-op
                            }
                            else if (inst is OpReturn)
                            {
                                if (!currFunc.ReturnType.IsVoid)
                                    throw new ValidationException(inst, "OpReturn in non-void function.");
                            }
                            else if (inst is OpReturnValue)
                            {
                                var op = inst as OpReturnValue;
                                RangeCheck(op.Value, op);
                                IntermediateTypeCheck(op.Value, t => SpirvType.Compatible(t, currFunc.ReturnType), op, "ReturnValue does not match func return type.");
                            }
                            else if (inst is OpUnreachable)
                            {
                                // no-op
                            }
                            else if (inst is OpPhi)
                            {
                                throw new ValidationException(inst, "Phis may only appear at the start of a block.");
                            }
                            else throw new NotImplementedException("Not implemented");

                            ++i;
                            // new block
                            currBlock.Instructions.Add(inst);
                            currBlock.BlockEnd = inst as FlowControlInstruction;
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
                            else
                            {
                                // TODO: need decoding?
                            }
                            currBlock.AddInstruction(op);
                            ++i;
                        }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (state != ModuleValidationState.MV13_0_OpFunction)
                throw new ValidationException(OriginalModule.Instructions.Last(), "Function is not finished.");

            // Entry point validation
            foreach (var ep in EntryPoints)
            {
                LocationTypeCheck(ep.EnryPointID, LocationType.Function, ep.Instruction);
                ep.SetFunction(Locations[ep.EnryPointID.Value].Function);
            }

            // Branches
            foreach (var branch in branches)
            {
                var op = branch.Item1;
                var block = branch.Item2;
                var id = branch.Item3;
                var literal = branch.Item4;

                LocationTypeCheck(id, LocationType.Label, op);
                block.AddBranchTarget(Locations[id.Value].Block, literal, op);
            }

            // TODO: member names
            // TODO: member decorations

            // Dominator analysis
            foreach (var function in Functions)
                function.DominatorAnalysis();

            // Component analysis
            foreach (var function in Functions)
                function.ComponentAnalysis();
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

        /// <summary>
        /// gets the type for a given type ID
        /// can throw if ID not a type
        /// (The instruction is for error msg)
        /// </summary>
        public SpirvType TypeFor(ID typeId, Instruction instruction)
        {
            LocationTypeCheck(typeId, LocationType.Type, instruction);
            return Locations[typeId.Value].SpirvType;
        }

        /// <summary>
        /// Returns the constant behind a given location
        /// (The instruction is for error msg)
        /// </summary>
        public object ConstantFor(ID location, Instruction instruction)
        {
            LocationTypeCheck(location, LocationType.Intermediate, instruction);
            if (!Locations[location.Value].IsConstant)
                throw new ValidationException(instruction, "Location " + location + " is not a constant at this point.");
            return Locations[location.Value].Constant;
        }

        /// <summary>
        /// ID to str
        /// </summary>
        public string IDStr(ID id)
        {
            if (id.Value < Locations.Length && !string.IsNullOrEmpty(Locations[id.Value].DebugName))
                return id + "(" + Locations[id.Value].DebugName + ")";
            return id.ToString();
        }
        /// <summary>
        /// ID to str or ""
        /// </summary>
        public string IDStr(ID? id) => id.HasValue ? IDStr(id.Value) : "";

        /// <summary>
        /// Adds debug output to a page
        /// </summary>
        public void AddDebugPageTo(PageElement e)
        {
            {
                e.AddContent("Stats", "h3");
                var t = e.AddChild(new DebugTable());
                t.SetHeader("Name", "Value");
                t.AddRow("Bound", Bound.ToString());
                t.AddRow("Capabilities", Capabilities.ToString());
                t.AddRow("Source Language", SourceLanguage.ToString());
                t.AddRow("Source Language Version", SourceLanguageVersion.ToString());
                t.AddRow("Addressing Model", AddressingModel.ToString());
                t.AddRow("Memory Model", MemoryModel.ToString());
                t.AddRow("Source Extensions", SourceExtensions.Count == 0 ? "" : SourceExtensions.Aggregate((s1, s2) => s1 + "<br />" + s2));
                t.AddRow("Compile Flags", CompileFlags.Count == 0 ? "" : CompileFlags.Aggregate((s1, s2) => s1 + "<br />" + s2));
                t.AddRow("Extensions", Extensions.Count == 0 ? "" : Extensions.Aggregate((s1, s2) => s1 + "<br />" + s2));
            }

            {
                e.AddContent("Entry Points", "h3");
                var t = e.AddChild(new DebugTable());
                t.SetHeader("ID", "Execution Model", "Execution Modes");
                foreach (var ep in EntryPoints)
                    t.AddRow(IDStr(ep.EnryPointID), ep.ExecutionModel.ToString(),
                        ep.ExecutionModes.Select(m => m.Mode).Aggregated(", "));
            }

            {
                e.AddContent("Functions", "h3");
                var t = e.AddChild(new DebugTable());
                t.SetHeader("ID", "Type ID", "Result ID", "Parameter IDs");
                foreach (var f in Functions)
                    t.AddRow(IDStr(f.DeclarationLocation.LocationID), IDStr(f.FunctionType.TypeID),
                        IDStr(f.ReturnType.TypeID), f.ParameterTypes.Select(p => IDStr(p.TypeID)).Aggregated(", "));
            }
        }

        /// <summary>
        /// Adds debug output to a page
        /// </summary>
        public void AddDebugPageFuncsTo(PageElement e)
        {
            {
                var p = e.AddPanel(PageHelper.Tagged("h3", "Constants"));
                var t = p.Body.AddChild(new DebugTable());
                t.SetHeader("ID", "Value");
                foreach (var loc in Locations.Where(l => l.IsConstant))
                    t.AddRow(IDStr(loc.LocationID), loc.Constant?.ToString());
            }

            foreach (var f in Functions)
            {
                var p = e.AddPanel(PageHelper.Tagged("h3", "Function " + IDStr(f.DeclarationLocation.LocationID)));

                {
                    var p2 = p.Body.AddPanel("Stats");
                    var t = p2.Body.AddChild(new DebugTable());
                    t.SetHeader("Type", "Name", "ID");
                    t.AddRow(IDStr(f.FunctionType.TypeID), "Function", IDStr(f.DeclarationLocation.LocationID));
                    t.AddRow(IDStr(f.ReturnType.TypeID), "Result", "");
                    for (var i = 0; i < f.ParameterTypes.Count; ++i)
                        t.AddRow(IDStr(f.ParameterTypes[i].TypeID), "Parameter " + (i + 1),
                            IDStr(f.ParameterLocations[i].LocationID));
                }

                p.Body.AddPanel("Control Flow Graph").Body.AddDotGraph(f.DotFile);
                p.Body.AddPanel("Dominator Tree").Body.AddDotGraph(f.DominatorDotFile);
                p.Body.AddPanel("Strongly-connected Components").Body.AddDotGraph(f.ComponentDotFile);
                p.Body.AddPanel("Dominator Tree with SCCs").Body.AddDotGraph(f.ComponentDominatorDotFile);
            }
        }
    }
}
