using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpirvNet.GLSL
{
    /// <summary>
    /// A list of statements
    /// </summary>
    class CompoundStatement : Statement
    {
        /// <summary>
        /// Sub-statements
        /// </summary>
        public readonly List<Statement> Statements = new List<Statement>();
    }
}
