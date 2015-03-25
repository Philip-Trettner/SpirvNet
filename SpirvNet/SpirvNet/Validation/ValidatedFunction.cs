using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv;

namespace SpirvNet.Validation
{
    /// <summary>
    /// A validated and analysed function consisting of blocks
    /// </summary>
    public class ValidatedFunction
    {
        /// <summary>
        /// Start block
        /// </summary>
        public ValidatedBlock FirstBlock { get; private set; }

        /// <summary>
        /// Function return type
        /// </summary>
        public readonly SpirvType ReturnType;
        /// <summary>
        /// Function parameter types
        /// </summary>
        public readonly List<SpirvType> ParameterTypes = new List<SpirvType>();
        /// <summary>
        /// Function decl type
        /// </summary>
        public readonly SpirvType FunctionType;

        /// <summary>
        /// Location where function is declared
        /// </summary>
        public readonly Location DeclarationLocation;

        /// <summary>
        /// List of function blocks
        /// </summary>
        public readonly List<ValidatedBlock> Blocks = new List<ValidatedBlock>();

        /// <summary>
        /// Mapping from label to block
        /// </summary>
        public readonly Dictionary<uint, ValidatedBlock> LabelToBlock = new Dictionary<uint, ValidatedBlock>();

        public ValidatedFunction(Location declarationLocation, SpirvType functionType)
        {
            DeclarationLocation = declarationLocation;
            FunctionType = functionType;
            ReturnType = FunctionType.ReturnType;
            ParameterTypes.AddRange(FunctionType.ParameterTypes);
        }

        /// <summary>
        /// Adds a block
        /// </summary>
        public void AddBlock(ValidatedBlock block)
        {
            if (FirstBlock != null)
                FirstBlock = block;

            Blocks.Add(block);
            LabelToBlock.Add(block.BlockLabel.Result.Value, block);
        }
    }
}
