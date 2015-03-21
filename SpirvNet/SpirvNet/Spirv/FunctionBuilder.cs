using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Ops.Function;

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

        public FunctionBuilder(string name)
        {
            Name = name;
        }


        /// <summary>
        /// Adds a function parameter
        /// </summary>
        public void AddParameter(OpFunctionParameter parameter) => opParameters.Add(parameter);

        /// <summary>
        /// Adds a block to this func
        /// </summary>
        public void AddBlock(BlockBuilder block) => blocks.Add(block);

        /// <summary>
        /// Generate func instructions (opFunc - opFuncEnd)
        /// </summary>
        public IEnumerable<Instruction> GenerateInstructions()
        {
            yield return opFunction;
            foreach (var para in opParameters)
                yield return para;
            foreach (var block in blocks)
                foreach (var op in block.GenerateInstructions())
                    yield return op;
            yield return new OpFunctionEnd();
        } 
    }
}
