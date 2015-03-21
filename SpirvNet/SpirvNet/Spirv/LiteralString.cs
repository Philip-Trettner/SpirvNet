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

        public void WriteCode(List<uint> code)
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
                var val = BitConverter.ToUInt32(bytes, i);
                maxI = i + 4;
                code.Add(val);
            }

            // null termination
            {
                var b0 = maxI + 0 >= bytes.Length ? (byte)0u : bytes[maxI + 0];
                var b1 = maxI + 1 >= bytes.Length ? (byte)0u : bytes[maxI + 1];
                var b2 = maxI + 2 >= bytes.Length ? (byte)0u : bytes[maxI + 2];
                var b3 = maxI + 3 >= bytes.Length ? (byte)0u : bytes[maxI + 3];
                var zb = new[] {b0, b1, b2, b3};
                var val = BitConverter.ToUInt32(zb, 0);
                
                code.Add(val);
            }
        }

        /// <summary>
        /// Reads the literal string from a code array
        /// </summary>
        public static LiteralString FromCode(uint[] code, ref int i)
        {
            // zero shortcut
            if (code[i] == 0)
            {
                ++i;
                return new LiteralString { Value = "" };
            }

            // restore bytes
            var bytes = new List<byte>();
            while (true)
            {
                var c = code[i];
                bytes.AddRange(BitConverter.GetBytes(c));
                if (bytes[bytes.Count - 4] == 0 ||
                    bytes[bytes.Count - 3] == 0 ||
                    bytes[bytes.Count - 2] == 0 ||
                    bytes[bytes.Count - 1] == 0)
                    break;
                ++i;
            }
            // remove trailing zeros
            while (bytes.Count > 0 && bytes[bytes.Count - 1] == 0)
                bytes.RemoveAt(bytes.Count - 1);

            // decode string
            var s = Encoding.UTF8.GetString(bytes.ToArray());
            return new LiteralString { Value = s };
        }

        public override string ToString() => "\"" + Value + "\"(" + (Value?.Length ?? -1) + ")";
    }
}
