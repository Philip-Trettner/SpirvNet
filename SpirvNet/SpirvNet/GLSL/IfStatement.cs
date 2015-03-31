using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv;

namespace SpirvNet.GLSL
{
    /// <summary>
    /// If-Branching
    /// </summary>
    class IfStatement : Statement
    {
        /// <summary>
        /// ID of the condition
        /// </summary>
        public ID ConditionID;
        /// <summary>
        /// Statement for "true"
        /// </summary>
        public Statement TrueStatement;
        /// <summary>
        /// Statement for "false"
        /// </summary>
        public Statement FalseStatement;

        public override IEnumerable<string> CodeLines
        {
            get
            {
                yield return string.Format("if ({0})", VarName(ConditionID));

                yield return "{";
                if (TrueStatement != null)
                    foreach (var line in TrueStatement.CodeLines)
                        yield return "  " + line;
                yield return "}";

                if (FalseStatement != null)
                {
                    yield return "else";
                    yield return "{";
                    foreach (var line in FalseStatement.CodeLines)
                        yield return "  " + line;
                    yield return "}";
                }
            }
        }
    }
}
