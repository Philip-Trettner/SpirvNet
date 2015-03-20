using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpirvNet.Spirv
{
    /// <summary>
    /// A nul-terminated stream of characters consuming an integral number of words. The character set is Unicode in
    /// the UTF-8 encoding scheme.The UTF-8 octets (8-bit bytes) are packed four per word, following the little-endian convention
    /// (i.e., the first octet is in the lowest-order 8-bits of the word). The final word contains the string’s nul-termination character(0),
    /// and all contents past the end of the string in the final word are padded with 0
    /// </summary>
    public struct LiteralString
    {
        public string Value;

        public void Generate(List<uint> code)
        {
            if (Value == null)
            {
                code.Add(0u);
                return;
            }

            var bytes = Encoding.UTF8.GetBytes(Value);
            var maxI = 0;

            // encoding
            for (var i = 0; i + 4 <= bytes.Length; i += 4)
            {
                var b0 = bytes[i + 0];
                var b1 = bytes[i + 1];
                var b2 = bytes[i + 2];
                var b3 = bytes[i + 3];
                maxI = i + 4;

                var val = (uint)(b0 + (b1 << 8) + (b2 << 16) + (b3 << 24));
                code.Add(val);
            }

            // null termination
            {
                var b0 = maxI + 0 >= bytes.Length ? (byte)0u : bytes[maxI + 0];
                var b1 = maxI + 1 >= bytes.Length ? (byte)0u : bytes[maxI + 1];
                var b2 = maxI + 2 >= bytes.Length ? (byte)0u : bytes[maxI + 2];
                var b3 = maxI + 3 >= bytes.Length ? (byte)0u : bytes[maxI + 3];

                var val = (uint)(b0 + (b1 << 8) + (b2 << 16) + (b3 << 24));
                code.Add(val);
            }
        }

        public override string ToString() => '"' + Value + '"';
    }
}
