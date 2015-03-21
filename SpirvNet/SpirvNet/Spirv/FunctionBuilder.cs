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

        /// <summary>
        /// Function end instruction
        /// </summary>
        private OpFunctionEnd opFunctionEnd;

        public FunctionBuilder(string name)
        {
            Name = name;
        }

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
            yield return opFunctionEnd;
        } 
    }
}
