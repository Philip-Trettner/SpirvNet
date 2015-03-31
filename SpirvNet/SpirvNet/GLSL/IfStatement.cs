using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpirvNet.GLSL
{
    /// <summary>
    /// If-Branching
    /// </summary>
    class IfStatement : Statement
    {
        /// <summary>
        /// Statement for "true"
        /// </summary>
        public Statement TrueStatement;
        /// <summary>
        /// Statement for "false"
        /// </summary>
        public Statement FalseStatement;
    }
}
