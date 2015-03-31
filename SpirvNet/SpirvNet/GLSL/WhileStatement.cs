using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpirvNet.GLSL
{
    /// <summary>
    /// A while-loop
    /// </summary>
    class WhileStatement : Statement
    {
        /// <summary>
        /// Statements in the while loop
        /// </summary>
        public readonly List<Statement> Statements = new List<Statement>();

        public override IEnumerable<string> CodeLines
        {
            get
            {
                yield return "while (true)";
                yield return "{";
                foreach (var statement in Statements)
                    foreach (var line in statement.CodeLines)
                        yield return "  " + line;
                yield return "}";
            }
        }
    }
}
