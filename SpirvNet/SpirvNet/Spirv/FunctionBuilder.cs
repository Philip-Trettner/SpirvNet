using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// Function decl
        /// </summary>
        private OpFunction opFunction;

        /// <summary>
        /// Function parameters
        /// </summary>
        private readonly List<OpFunctionParameter> opParameters = new List<OpFunctionParameter>();

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
        /// Function type
        /// </summary>
        public OpTypeFunction FunctionType { get; private set; }

        /// <summary>
        /// If name non-null, it is used for debugging
        /// </summary>
        public FunctionBuilder(string name = null)
        {
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
        public void SetupFunction(ID returnType, IDAllocator allocator, FunctionControlMask mask = FunctionControlMask.None)
        {
            FunctionType = new OpTypeFunction
            {
                Result = allocator.CreateID(),
                ReturnType = returnType,
                ParameterTypes = opParameters.Select(op => op.ResultType).ToArray()
            };

            opFunction = new OpFunction
            {
                FunctionControlMask = mask,
                FunctionType = FunctionType.Result,
                Result = allocator.CreateID(),
                ResultType = returnType
            };

            if (Name != null)
            {
                AdditionalNames.Add(new OpName { Target = opFunction.Result, Name = { Value = Name } });
                AdditionalNames.Add(new OpName { Target = FunctionType.Result, Name = { Value = Name } });
            }
        }

        /// <summary>
        /// Adds a function parameter
        /// If paraName non-null, it is added as a name
        /// </summary>
        public void AddParameter(OpFunctionParameter parameter, string paraName = null)
        {
            if (opFunction != null)
                throw new InvalidOperationException("Parameters cannot be changed after function was set up");

            opParameters.Add(parameter);

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
            if (opFunction == null)
                throw new InvalidOperationException("setupFunction was not called");

            yield return opFunction;
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
