using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebugPage;
using Mono.Cecil;

namespace SpirvNet.DotNet.CFG
{
    /// <summary>
    /// CFG Graph of a .NET function
    /// </summary>
    public class ControlFlowGraph
    {
        /// <summary>
        /// Represented method
        /// </summary>
        public readonly MethodDefinition Method;

        /// <summary>
        /// List of vertices
        /// </summary>
        public readonly List<Vertex> Vertices = new List<Vertex>();

        /// <summary>
        /// Mapping from instruction offset to vertex index
        /// </summary>
        public readonly Dictionary<int, int> OffsetToIndex = new Dictionary<int, int>();

        /// <summary>
        /// Creates the CFG from a method
        /// </summary>
        public ControlFlowGraph(MethodDefinition method)
        {
            Method = method;
            if (method.Body == null)
                throw new InvalidOperationException("Method has no body");

            // allocate vertices
            foreach (var instruction in method.Body.Instructions)
            {
                var v = new Vertex(instruction, Vertices.Count);
                OffsetToIndex.Add(instruction.Offset, Vertices.Count);
                Vertices.Add(v);
            }

            // build CFG
            foreach (var v in Vertices)
                v.Build(this);
        }

        public IEnumerable<string> DotFile
        {
            get
            {
                yield return "digraph CFG {";
                foreach (var v in Vertices)
                    foreach (var line in v.DotLines)
                        yield return "  " + line;
                yield return "}";
            }
        }
        
        /// <summary>
        /// Adds a dot file 
        /// </summary>
        public void AddDebugPageTo(PageElement e)
        {
            e.AddDotGraph(DotFile);
        }
    }
}
