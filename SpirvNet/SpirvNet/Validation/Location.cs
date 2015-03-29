using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv;
using SpirvNet.Spirv.Enums;
using SpirvNet.Spirv.Ops.Annotation;
using SpirvNet.Spirv.Ops.ConstantCreation;
using SpirvNet.Spirv.Ops.Debug;
using SpirvNet.Spirv.Ops.Extension;
using SpirvNet.Spirv.Ops.FlowControl;
using SpirvNet.Spirv.Ops.Function;
using SpirvNet.Spirv.Ops.TypeDeclaration;

namespace SpirvNet.Validation
{
    public enum LocationType
    {
        /// <summary>
        /// Unallocated
        /// </summary>
        None,
        /// <summary>
        /// A named string (from an OpString)
        /// </summary>
        String,
        /// <summary>
        /// An OpTypeXYZ (from a TypeDeclarationInstruction)
        /// </summary>
        Type,
        /// <summary>
        /// An Intermediate (function instructions) or A constant (from a ConstantCreationInstruction)
        /// </summary>
        Intermediate,
        /// <summary>
        /// A target label (from an OpLabel)
        /// </summary>
        Label,
        /// <summary>
        /// A function (from an OpFunction)
        /// </summary>
        Function,
        /// <summary>
        /// A function parameter (from an OpFunctionParameter)
        /// </summary>
        FunctionParameter,
        /// <summary>
        /// An imported instruction (from an OpExtInstImport)
        /// </summary>
        ImportedInstruction,
        /// <summary>
        /// A decoration group (from an OpDecorationGroup)
        /// </summary>
        DecorationGroup
    }

    /// <summary>
    /// Location of a validated module
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Location ID
        /// </summary>
        public readonly uint ID;

        /// <summary>
        /// Location ID (as ID)
        /// </summary>
        public ID LocationID => new ID(ID);

        /// <summary>
        /// Debug name of this location
        /// </summary>
        public string DebugName { get; private set; }

        /// <summary>
        /// Type of this location
        /// </summary>
        public LocationType LocationType { get; private set; } = LocationType.None;

        /// <summary>
        /// SPIR-V type of this location (either declared or used type)
        /// </summary>
        public SpirvType SpirvType { get; private set; }

        /// <summary>
        /// Function control mask (valid for functions)
        /// </summary>
        public FunctionControlMask FunctionControlMask { get; private set; }
        /// <summary>
        /// List of function paras (valid for function)
        /// </summary>
        public List<Location> FunctionParameters { get; private set; }

        /// <summary>
        /// Location of instruction block
        /// </summary>
        public ValidatedBlock Block { get; private set; }
        /// <summary>
        /// Location inside a Function
        /// </summary>
        public ValidatedFunction Function { get; private set; }

        /// <summary>
        /// Inside-a-function instruction
        /// If null, this is a constant
        /// </summary>
        public Instruction IntermediateOp { get; private set; }
        /// <summary>
        /// If true, this is a constant
        /// </summary>
        public bool IsConstant => IntermediateOp == null && LocationType == LocationType.Intermediate;

        /// <summary>
        /// Name of the instruction iff this is an instruction
        /// Name of the string iff this is a string
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// A constant value
        /// </summary>
        public object Constant { get; private set; }

        /// <summary>
        /// If non-null, contains all original source locations
        /// </summary>
        public List<SourceLocation> SourceLocations { get; private set; }

        /// <summary>
        /// Decorations
        /// </summary>
        public List<OpDecorate> Decorations { get; private set; }
        /// <summary>
        /// Member decorations
        /// </summary>
        public List<OpMemberDecorate> MemberDecorations { get; private set; }

        public Location(uint id)
        {
            ID = id;
        }

        /// <summary>
        /// Debug name
        /// </summary>
        public void SetDebugName(string name)
        {
            if (string.IsNullOrEmpty(DebugName))
                DebugName = name;
            else DebugName += ", " + name;
        }

        /// <summary>
        /// Throws a validation exceptin if types do not match
        /// </summary>
        private void TypeCheck(SpirvType found, SpirvType expected, Instruction op)
        {
            if (found.ToString() != expected.ToString())
                throw new ValidationException(op, string.Format("Expected type {0} ({2}) but found {1} ({3}).", expected, found, expected.TypeID, found.TypeID));
        }

        /// <summary>
        /// fills this location from an OpExtInstImport
        /// </summary>
        public void FillFromExtInstImport(OpExtInstImport op)
        {
            LocationType = LocationType.ImportedInstruction;
            Name = op.Name.Value;
        }
        /// <summary>
        /// fills this location from an OpString
        /// </summary>
        public void FillFromString(OpString op)
        {
            LocationType = LocationType.String;
            Name = op.Name.Value;
        }

        /// <summary>
        /// Fills this location from an OpDecorationGroup
        /// </summary>
        public void FillFromDecorationGroup(OpDecorationGroup op)
        {
            LocationType = LocationType.DecorationGroup;
        }

        /// <summary>
        /// Fills this location from a type declaration
        /// </summary>
        public void FillFromType(TypeDeclarationInstruction op, ITypeProvider typeProvider)
        {
            LocationType = LocationType.Type;
            var opInt = op as OpTypeInt;
            var opFloat = op as OpTypeFloat;
            var opVector = op as OpTypeVector;
            var opMatrix = op as OpTypeMatrix;
            var opFunction = op as OpTypeFunction;
            var opStruct = op as OpTypeStruct;

            if (op is OpTypeVoid)
                SpirvType = new SpirvType(LocationID, SpirvTypeEnum.Void);
            else if (op is OpTypeBool)
                SpirvType = new SpirvType(LocationID, SpirvTypeEnum.Boolean);
            else if (opInt != null)
                SpirvType = new SpirvType(LocationID, SpirvTypeEnum.Integer, opInt.Width.Value, opInt.Signedness.Value);
            else if (opFloat != null)
                SpirvType = new SpirvType(LocationID, SpirvTypeEnum.Floating, opFloat.Width.Value);
            else if (opVector != null)
                SpirvType = new SpirvType(LocationID, SpirvTypeEnum.Vector, elementCount: opVector.ComponentCount.Value, elementType: typeProvider.TypeFor(opVector.ComponentType, op));
            else if (opMatrix != null)
                SpirvType = new SpirvType(LocationID, SpirvTypeEnum.Matrix, elementCount: opMatrix.ColumnCount.Value, elementType: typeProvider.TypeFor(opMatrix.ColumnType, op));
            else if (opFunction != null)
                SpirvType = new SpirvType(LocationID, SpirvTypeEnum.Function,
                    returnType: typeProvider.TypeFor(opFunction.ReturnType, op),
                    parameterTypes: opFunction.ParameterTypes.Select(p => typeProvider.TypeFor(p, op)).ToArray());
            else if (opStruct != null)
                SpirvType = new SpirvType(LocationID, SpirvTypeEnum.Structure, structMembers: opStruct.MemberTypes.Select((t, i) => new StructMember(i, null, typeProvider.TypeFor(t, op))).ToArray());
            else throw new NotImplementedException("Unknown type decl: " + op);
            // TODO: More types!
        }

        /// <summary>
        /// Fills this location from a type declaration
        /// </summary>
        public void FillFromConstant(ConstantCreationInstruction op, ITypeProvider typeProvider)
        {
            LocationType = LocationType.Intermediate;
            var opConstTrue = op as OpConstantTrue;
            var opConstFalse = op as OpConstantFalse;
            var opConst = op as OpConstant;
            var opComp = op as OpConstantComposite;
            if (!op.ResultTypeID.HasValue)
                throw new ValidationException(op, "ConstantCreationInstruction without ResultType.");
            var type = typeProvider.TypeFor(op.ResultTypeID.Value, op);
            SpirvType = type;

            if (opConstTrue != null)
            {
                if (!type.IsBoolean)
                    throw new ValidationException(op, "Expected boolean type, found " + type);
                Constant = true;
            }
            else if (opConstFalse != null)
            {
                if (!type.IsBoolean)
                    throw new ValidationException(op, "Expected boolean type, found " + type);
                Constant = false;
            }
            else if (opConst != null)
            {
                switch (type.TypeEnum)
                {
                    case SpirvTypeEnum.Integer:
                        if (type.IsSigned && type.BitWidth == 32)
                            Constant = opConst.Value.ToInt32();
                        else if (type.IsSigned && type.BitWidth == 64)
                            Constant = opConst.Value.ToInt64();
                        else if (!type.IsSigned && type.BitWidth == 32)
                            Constant = opConst.Value.ToUInt32();
                        else if (!type.IsSigned && type.BitWidth == 64)
                            Constant = opConst.Value.ToUInt64();
                        else
                            throw new ValidationException(op, "Only 32 or 64 bit width valid, found " + type);
                        break;
                    case SpirvTypeEnum.Floating:
                        if (type.BitWidth == 32)
                            Constant = opConst.Value.ToFloat32();
                        else if (type.BitWidth == 64)
                            Constant = opConst.Value.ToFloat64();
                        else
                            throw new ValidationException(op, "Only 32 or 64 bit width valid, found " + type);
                        break;

                    default:
                        throw new ValidationException(op, "OpConstant only valid for scalar integer and floating types, found " + type);
                }
            }
            else if (opComp != null)
            {
                switch (type.TypeEnum)
                {
                    case SpirvTypeEnum.Structure:
                        Constant = new SpirvStruct(SpirvType, opComp.Constituents.Select(c => typeProvider.ConstantFor(c, op)).ToArray());
                        break;

                    default:
                        throw new NotImplementedException("constant for non-struct composite not implemented yet");
                }
            }
            else throw new NotImplementedException("Unknown constant creation: " + op);
            // TODO: More constants!
        }

        /// <summary>
        /// Fills this location with a function decl
        /// </summary>
        public void FillFromFunction(OpFunction op, ValidatedFunction function, ITypeProvider typeProvider)
        {
            LocationType = LocationType.Function;
            SpirvType = typeProvider.TypeFor(op.FunctionType, op);
            FunctionControlMask = op.FunctionControlMask;
            Function = function;
            FunctionParameters = new List<Location>();

            if (SpirvType.TypeEnum != SpirvTypeEnum.Function)
                throw new ValidationException(op, "FunctionType must be a function type, but found " + SpirvType);
            var resType = typeProvider.TypeFor(op.ResultType, op);
            if (op.ResultType != SpirvType.ReturnType.TypeID)
                throw new ValidationException(op, string.Format("Result type does not match with declared return type ({0} ({2}) vs. {1} ({3}))", resType, SpirvType.ReturnType, op.ResultType, SpirvType.ReturnType.TypeID));
        }

        /// <summary>
        /// Fills this location with a function parameter decl
        /// </summary>
        public void FillFromFunctionParameter(OpFunctionParameter op, ValidatedFunction function, ITypeProvider typeProvider)
        {
            LocationType = LocationType.FunctionParameter;
            SpirvType = typeProvider.TypeFor(op.ResultType, op);
            Function = function;
        }

        /// <summary>
        /// Fills this location from a label
        /// </summary>
        public void FillFromLabel(OpLabel op, ValidatedBlock block)
        {
            LocationType = LocationType.Label;
            Block = block;
            Function = Block.Function;
        }

        /// <summary>
        /// Fills this location for a function instruction
        /// </summary>
        public void FillFromFunctionInstruction(Instruction op, ValidatedBlock block, ITypeProvider typeProvider)
        {
            LocationType = LocationType.Intermediate;
            IntermediateOp = op;
            Block = block;
            Function = Block.Function;

            // TODO: Type deduction
            if (op.ResultTypeID.HasValue)
            {
                SpirvType = typeProvider.TypeFor(op.ResultTypeID.Value, op);
            }
            else throw new NotImplementedException("Add missing type deduction");
        }

        /// <summary>
        /// Adds line information to this loccation
        /// </summary>
        public void AddLineInfo(string file, uint line, uint col)
        {
            if (SourceLocations == null)
                SourceLocations = new List<SourceLocation>();
            SourceLocations.Add(new SourceLocation(file, line, col));
        }

        /// <summary>
        /// Adds a reference to a function parameter and performs type validation
        /// </summary>
        public void AddFunctionParameter(Location loc, OpFunctionParameter op, ITypeProvider typeProvider)
        {
            if (LocationType != LocationType.Function)
                throw new ValidationException(op, "Function parameter for non-function location " + LocationID);

            var paraIdx = FunctionParameters.Count;
            if (paraIdx >= SpirvType.ParameterTypes.Length)
                throw new ValidationException(op, "More actual parameters than function type declared.");

            var paraType = typeProvider.TypeFor(op.ResultType, op);
            if (paraType.ToString() != SpirvType.ParameterTypes[paraIdx].ToString())
                throw new ValidationException(op, string.Format("Actual parameter type differs from declared type: ({0} ({2}) vs. {1} ({3}))",
                    paraType, SpirvType.ParameterTypes[paraIdx], paraType.TypeID, SpirvType.ParameterTypes[paraIdx].TypeID));

            FunctionParameters.Add(loc);
        }

        /// <summary>
        /// Adds a decoration to this location
        /// </summary>
        public void AddDecoration(OpDecorate op)
        {
            if (LocationType == LocationType.DecorationGroup)
                throw new ValidationException(op, "Cannot decorate a decoration group after it was introduced.");

            if (Decorations == null)
                Decorations = new List<OpDecorate>();
            Decorations.Add(op);
        }

        /// <summary>
        /// Adds a member decoration to this location
        /// </summary>
        public void AddMemberDecoration(OpMemberDecorate op)
        {
            if (MemberDecorations == null)
                MemberDecorations = new List<OpMemberDecorate>();
            MemberDecorations.Add(op);
        }
    }
}
