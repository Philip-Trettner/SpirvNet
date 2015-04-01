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
    /// OpTypePointer
    /// 
    /// Declare a new pointer type.
    /// 
    /// Storage Class is the Storage Class of the memory holding the object pointed to.
    /// 
    /// Type is the type of the object pointed to.
    /// 
    /// Result &lt;id&gt; is the &lt;id&gt; of the new pointer type.
    /// </summary>
    public sealed class OpTypePointer : TypeDeclarationInstruction
    {
        public override bool IsTypeDeclaration => true;
        public override OpCode OpCode => OpCode.TypePointer;
        public override ID? ResultID => Result;

        public ID Result;
        public StorageClass StorageClass;
        public ID Type;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(Result) + ", " + StrOf(StorageClass) + ", " + StrOf(Type) + ")";
        public override string ArgString => "StorageClass: " + StrOf(StorageClass) + ", " + "Type: " + StrOf(Type);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TypePointer);
            var i = start + 1;
            Result = new ID(codes[i++]);
            StorageClass = (StorageClass)codes[i++];
            Type = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(Result.Value);
            code.Add((uint)StorageClass);
            code.Add(Type.Value);
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
