using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.TypeDeclaration
{
    /// <summary>
    /// OpTypeFunction
    /// 
    /// Declare a new function type.  OpFunction will use this to declare the return type and parameter types of a function.
    /// 
    /// Return Type is the type of the return value of functions of this type. If the function has no return value, Return Type should be from OpTypeVoid.
    /// 
    /// Parameter N Type is the type &lt;id&gt; of the type of parameter N.
    /// 
    /// Result &lt;id&gt; is the &lt;id&gt; of the new function type.
    /// </summary>
    public sealed class OpTypeFunction : TypeDeclarationInstruction
    {
        public override bool IsTypeDeclaration => true;
        public override OpCode OpCode => OpCode.TypeFunction;
        public override ID? ResultID => Result;

        public ID Result;
        public ID ReturnType;
        public ID[] Parameters = { };

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Result) + ", " + StrOf(ReturnType) + ", " + StrOf(Parameters) + ")";
        public override string ArgString => "ReturnType: " + StrOf(ReturnType) + ", " + "Parameters: " + StrOf(Parameters);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TypeFunction);
            var i = start + 1;
            Result = new ID(codes[i++]);
            ReturnType = new ID(codes[i++]);
            var length = WordCount - (i - start);
            Parameters = new ID[length];
            for (var k = 0; k < length; ++k)
                Parameters[k] = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Result.Value);
            code.Add(ReturnType.Value);
            if (Parameters != null)
                foreach (var val in Parameters)
                    code.Add(val.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Result;
                yield return ReturnType;
                if (Parameters != null)
                    foreach (var id in Parameters)
                        yield return id;
            }
        }
        #endregion
    }
}
