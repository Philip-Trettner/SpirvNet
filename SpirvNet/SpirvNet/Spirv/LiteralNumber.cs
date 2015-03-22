using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpirvNet.Spirv
{
    /// <summary>
    /// A numeric value consuming one or more words. 
    /// When a numeric value is larger than one word, low-order words appear first.
    /// </summary>
    public struct LiteralNumber
    {
        public uint Value;

        public LiteralNumber(uint val)
        {
            Value = val;
        }

        public LiteralNumber(byte[] val, int start)
        {
            Value = BitConverter.ToUInt32(val, start);
        }

        public static LiteralNumber[] ArrayFor(byte[] vals)
        {
            var nrs = new LiteralNumber[vals.Length / 4];
            for (var i = 0; i < vals.Length; i += 4)
                nrs[i / 4] = new LiteralNumber(vals, i);
            return nrs;
        }

        public static LiteralNumber[] ArrayFor(int val) => ArrayFor(BitConverter.GetBytes(val));
        public static LiteralNumber[] ArrayFor(uint val) => ArrayFor(BitConverter.GetBytes(val));
        public static LiteralNumber[] ArrayFor(float val) => ArrayFor(BitConverter.GetBytes(val));
        public static LiteralNumber[] ArrayFor(long val) => ArrayFor(BitConverter.GetBytes(val));
        public static LiteralNumber[] ArrayFor(ulong val) => ArrayFor(BitConverter.GetBytes(val));
        public static LiteralNumber[] ArrayFor(double val) => ArrayFor(BitConverter.GetBytes(val));

        public override string ToString() => Value.ToString();
    }
}
