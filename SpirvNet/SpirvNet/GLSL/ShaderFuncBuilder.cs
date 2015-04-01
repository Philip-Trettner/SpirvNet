using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Validation;

namespace SpirvNet.GLSL
{
    /// <summary>
    /// Builder for a shader function
    /// </summary>
    class ShaderFuncBuilder
    {
        /// <summary>
        /// The function in questions
        /// </summary>
        public readonly ValidatedFunction Function;

        /// <summary>
        /// the core statements
        /// </summary>
        private readonly CompoundStatement statement = new CompoundStatement();

        public ShaderFuncBuilder(ValidatedFunction function)
        {
            Function = function;

            // cautionary checks
            foreach (var comp in function.Components)
                CheckComp(comp);

            return;
            var graph = function.CreateGraph();
            GenerateFor(graph.First(), function.CreateGraph(), statement, null);
        }

        /// <summary>
        /// Checks if components are supported
        /// </summary>
        private static void CheckComp(ValidatedComponent comp)
        {
            if (comp.EntryBlocks.Count != 1)
                throw new NotSupportedException("Components with " + comp.EntryBlocks.Count + " entry points are not supported (yet)");

            foreach (var block in comp.Blocks)
                if (!block.DominatedBy(comp.EntryBlock))
                    throw new NotSupportedException("All component nodes must be dominated by the entry block.");

            foreach (var sc in comp.SubComponents)
                CheckComp(sc);
        }

        /// <summary>
        /// Recursive generation of blocks
        /// </summary>
        private void GenerateFor(BlockSubnode block, List<BlockSubnode> graph, CompoundStatement parent, ValidatedComponent comp)
        {
            // exit
            if (block.Outgoing.Count == 0)
            {
                parent.Statements.Add(new BlockStatement(block.Block));
            }
            // sub-components (loops)
            else if (block.Block.InnerComponent != comp)
            {
                Debug.Assert(comp == null || block.Block.Components.Contains(comp));
                Debug.Assert(block.Block.InnerComponent.EntryBlock == block.Block);
            }
            // branching
            else if (block.Outgoing.Count > 1)
            {
                //var join =
            }
            // linear
            else if (block.Outgoing.Count == 1)
            {
                parent.Statements.Add(new BlockStatement(block.Block));
                GenerateFor(block.Outgoing[0], graph, parent, comp);
            }
            else throw new NotSupportedException("Unknown case");
        }

        /// <summary>
        /// GLSL shader code lines
        /// </summary>
        public IEnumerable<string> CodeLines => statement.CodeLines;
    }
}
