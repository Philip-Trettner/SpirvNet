using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.DotNet.SSA;
using SpirvNet.Spirv.Enums;
using SpirvNet.Spirv.Ops.Debug;
using SpirvNet.Spirv.Ops.Function;
using SpirvNet.Spirv.Ops.TypeDeclaration;

namespace SpirvNet.Spirv
{
    /// <summary>
    /// SPIR-V function build helper
    /// </summary>
    public class FunctionBuilder
    {
        /// <summary>
        /// Func name
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Function decl (valid after setupFunction)
        /// </summary>
        public OpFunction OpFunction { get; private set; }

        /// <summary>
        /// Associated .NET method frame (if built from .NET func)
        /// </summary>
        public readonly MethodFrame Frame;

        /// <summary>
        /// Function parameters
        /// </summary>
        private readonly List<OpFunctionParameter> opParameters = new List<OpFunctionParameter>();
        /// <summary>
        /// List of para types
        /// </summary>
        public readonly List<SpirvType> ParameterTypes = new List<SpirvType>();

        /// <summary>
        /// List of blocks
        /// </summary>
        private readonly List<BlockBuilder> blocks = new List<BlockBuilder>();

        /// <summary>
        /// List of additional instructions not built by blocks
        /// </summary>
        private readonly List<Instruction> additionalInstructions = new List<Instruction>();

        /// <summary>
        /// Additional name tags
        /// </summary>
        public readonly List<OpName> AdditionalNames = new List<OpName>();

        /// <summary>
        /// Function type op
        /// </summary>
        public OpTypeFunction OpFunctionType { get; private set; }
        /// <summary>
        /// Function type
        /// </summary>
        public SpirvType FunctionType { get; private set; }

        /// <summary>
        /// If name non-null, it is used for debugging
        /// </summary>
        public FunctionBuilder(string name = null, MethodFrame frame = null)
        {
            Frame = frame;
            Name = name;
        }

        /// <summary>
        /// Adds an additional op
        /// </summary>
        public void AddOp(Instruction op) => additionalInstructions.Add(op);

        /// <summary>
        /// Setup function and function type
        /// Must be called at least once before module creation
        /// Must be called after all parameters are set up
        /// </summary>
        public void SetupFunction(SpirvType returnType, TypeBuilder typeBuilder, IDAllocator allocator, FunctionControlMask mask = FunctionControlMask.None)
        {
            FunctionType = new SpirvType(allocator.CreateID(), SpirvTypeEnum.Function, returnType: returnType, parameterTypes: ParameterTypes.ToArray());
            OpFunctionType = new OpTypeFunction
            {
                Result = FunctionType.TypeID,
                ReturnType = returnType.TypeID,
                ParameterTypes = opParameters.Select(op => op.ResultType).ToArray()
            };

            OpFunction = new OpFunction
            {
                FunctionControlMask = mask,
                FunctionType = OpFunctionType.Result,
                Result = allocator.CreateID(),
                ResultType = returnType.TypeID
            };

            if (Name != null)
            {
                AdditionalNames.Add(new OpName { Target = OpFunction.Result, Name = { Value = Name } });
                AdditionalNames.Add(new OpName { Target = OpFunctionType.Result, Name = { Value = Name } });
            }
        }

        /// <summary>
        /// Adds a function parameter
        /// If paraName non-null, it is added as a name
        /// </summary>
        public void AddParameter(OpFunctionParameter parameter, SpirvType type, string paraName = null)
        {
            if (OpFunction != null)
                throw new InvalidOperationException("Parameters cannot be changed after function was set up");

            opParameters.Add(parameter);
            ParameterTypes.Add(type);

            if (paraName != null)
                AdditionalNames.Add(new OpName { Target = parameter.Result, Name = { Value = paraName } });
        }

        /// <summary>
        /// Adds a block to this func
        /// </summary>
        public void AddBlock(BlockBuilder block) => blocks.Add(block);

        /// <summary>
        /// Generate func instructions (opFunc - opFuncEnd)
        /// </summary>
        public IEnumerable<Instruction> GenerateInstructions()
        {
            if (OpFunction == null)
                throw new InvalidOperationException("setupFunction was not called");

            yield return OpFunction;
            foreach (var para in opParameters)
                yield return para;
            foreach (var block in blocks)
                foreach (var op in block.GenerateInstructions())
                    yield return op;
            foreach (var instruction in additionalInstructions)
                yield return instruction;
            yield return new OpFunctionEnd();
        }
    }
}
