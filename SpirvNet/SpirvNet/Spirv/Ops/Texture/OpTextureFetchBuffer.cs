using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Texture
{
    /// <summary>
    /// OpTextureFetchBuffer
    /// 
    /// Fetch an element out of a buffer texture.
    /// 
    /// Result Type must be a vector of four components of the same type as Sampled Type of Sampler&#8217;s type.
    /// 
    /// Sampler must be an object of a type made by OpTypeSampler. It must have a Dimensionality of Buffer.
    /// 
    /// Element is a scalar integer index into the buffer.
    /// </summary>
    [DependsOn(LanguageCapability.Shader)]
    public sealed class OpTextureFetchBuffer : TextureInstruction
    {
        public override bool IsTexture => true;
        public override OpCode OpCode => OpCode.TextureFetchBuffer;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Sampler;
        public ID Element;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Sampler) + ", " + StrOf(Element) + ")";
        public override string ArgString => "Sampler: " + StrOf(Sampler) + ", " + "Element: " + StrOf(Element);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.TextureFetchBuffer);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Sampler = new ID(codes[i++]);
            Element = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Sampler.Value);
            code.Add(Element.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Sampler;
                yield return Element;
            }
        }
        #endregion
    }
}
