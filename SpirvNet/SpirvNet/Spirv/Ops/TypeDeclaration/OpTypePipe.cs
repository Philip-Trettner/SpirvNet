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
    /// OpTypePipe
    /// 
    /// Declare an OpenCL pipe object type.
    /// 
    /// Type is the data type of the pipe.
    /// 
    /// Qualifier is the pipe access qualifier.
    /// 
    /// Result &lt;id&gt; is the &lt;id&gt; of the new pipe type.
    /// </summary>
    [DependsOn(LanguageCapability.Kernel)]
    public sealed class OpTypePipe : TypeDeclarationInstruction
    {
        public override bool IsTypeDeclaration => true;
        public override OpCode OpCode => OpCode.TypePipe;
        public override ID? ResultID => Result;

        public ID Result;
        public ID Type;
        public AccessQualifier Qualifier;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Result) + ", " + StrOf(Type) + ", " + StrOf(Qualifier) + ")";
        public override string ArgString => "Type: " + StrOf(Type) + ", " + "Qualifier: " + StrOf(Qualifier);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TypePipe);
            var i = start + 1;
            Result = new ID(codes[i++]);
            Type = new ID(codes[i++]);
            Qualifier = (AccessQualifier)codes[i++];
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Result.Value);
            code.Add(Type.Value);
            code.Add((uint)Qualifier);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return Result;
                yield return Type;
            }
        }
        #endregion
    }
}
