using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

// This file is auto-generated and should not be modified manually.

namespace SpirvNet.Spirv.Ops.Memory
{
    /// <summary>
    /// OpImagePointer
    /// 
    /// Form a pointer to a texel of an image.  Use of such a pointer is limited to atomic operations.
    /// 
    /// Image is a pointer to a variable of type of OpTypeSampler.
    /// 
    /// Coordinate and Sample specify which texel and sample within the image to form an address of.
    /// 
    /// TBD.  This requires an Image storage class to be added.
    /// </summary>
    public sealed class OpImagePointer : MemoryInstruction
    {
        public override bool IsMemory => true;
        public override OpCode OpCode => OpCode.ImagePointer;
        public override ID? ResultID => Result;
        public override ID? ResultTypeID => ResultType;

        public ID ResultType;
        public ID Result;
        public ID Image;
        public ID Coordinate;
        public ID Sample;

        #region Code
        public override string ToString() => "(" + OpCode + "(" + (int)OpCode + ")" + ", " + StrOf(ResultType) + ", " + StrOf(Result) + ", " + StrOf(Image) + ", " + StrOf(Coordinate) + ", " + StrOf(Sample) + ")";
        public override string ArgString => "Image: " + StrOf(Image) + ", " + "Coordinate: " + StrOf(Coordinate) + ", " + "Sample: " + StrOf(Sample);

        protected override void FromCode(uint[] codes, int start)
        {
            System.Diagnostics.Debug.Assert((codes[start] & 0x0000FFFF) == (uint)OpCode.ImagePointer);
            var i = start + 1;
            ResultType = new ID(codes[i++]);
            Result = new ID(codes[i++]);
            Image = new ID(codes[i++]);
            Coordinate = new ID(codes[i++]);
            Sample = new ID(codes[i++]);
        }

        protected override void WriteCode(List<uint> code)
        {
            code.Add(ResultType.Value);
            code.Add(Result.Value);
            code.Add(Image.Value);
            code.Add(Coordinate.Value);
            code.Add(Sample.Value);
        }

        public override IEnumerable<ID> AllIDs
        {
            get
            {
                yield return ResultType;
                yield return Result;
                yield return Image;
                yield return Coordinate;
                yield return Sample;
            }
        }
        #endregion
    }
}
