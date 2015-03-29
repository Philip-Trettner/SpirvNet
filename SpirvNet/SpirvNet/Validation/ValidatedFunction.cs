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
        public ValidatedBlock StartBlock { get; private set; }

        /// <summary>
        /// Parent module
        /// </summary>
        public readonly ValidatedModule Module;

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
        /// List of parameter locations
        /// </summary>
        public readonly List<Location> ParameterLocations = new List<Location>();

        /// <summary>
        /// Mapping from label to block
        /// </summary>
        public readonly Dictionary<uint, ValidatedBlock> LabelToBlock = new Dictionary<uint, ValidatedBlock>();

        public ValidatedFunction(Location declarationLocation, SpirvType functionType, ValidatedModule module)
        {
            DeclarationLocation = declarationLocation;
            FunctionType = functionType;
            Module = module;
            ReturnType = FunctionType.ReturnType;
            ParameterTypes.AddRange(FunctionType.ParameterTypes);
        }

        /// <summary>
        /// Adds a block
        /// </summary>
        public void AddBlock(ValidatedBlock block)
        {
            if (StartBlock == null)
                StartBlock = block;

            Blocks.Add(block);
            LabelToBlock.Add(block.BlockLabel.Result.Value, block);
        }

        /// <summary>
        /// Gets a DOT file for this function
        /// </summary>
        public IEnumerable<string> DotFile
        {
            get
            {
                yield return "digraph Function {";
                foreach (var block in Blocks)
                    yield return block.DotNode;
                foreach (var b1 in Blocks)
                    foreach (var b2 in b1.OutgoingBlocks)
                        yield return string.Format("  b{0}->b{1};", b1.BlockID.Value, b2.BlockID.Value);
                yield return "}";
            }
        }
    }
}
