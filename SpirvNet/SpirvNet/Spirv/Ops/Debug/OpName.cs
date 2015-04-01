using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Debug
{
    /// <summary>
    /// OpName
    /// 
    /// Name a Result &lt;id&gt;. This has no semantic impact and can safely be removed from a module.
    /// 
    /// Target is the Result &lt;id&gt; to name. It can be the Result &lt;id&gt; of any other instruction; a variable, function, type, intermediate result, etc.
    /// 
    /// Name is the string to name &lt;id&gt; with.
    /// </summary>
    public sealed class OpName : DebugInstruction
    {
        public override bool IsDebug => true;
        public override OpCode OpCode => OpCode.Name;

        public ID Target;
        public LiteralString Name;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Target) + ", " + StrOf(Name) + ")";
        public override string ArgString => "Target: " + StrOf(Target) + ", " + "Name: " + StrOf(Name);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.Name);
            var i = start + 1;
            Target = new ID(codes[i++]);
            Name = LiteralString.FromCode(codes, ref i);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Target.Value);
            Name.WriteCode(code);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Target;
            }
        }
        #endregion
    }
}
