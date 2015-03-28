using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SpirvNet.Spirv
{
    /// <summary>
    /// A SSA-style ID
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ID : IEquatable<ID>
    {
        /// <summary>
        /// Return the invalid ID zero
        /// </summary>
        public static ID Invalid => new ID(0);

        /// <summary>
        /// Numerical value
        /// </summary>
        public readonly uint Value;

        public bool Equals(ID other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ID && Equals((ID)obj);
        }

        public override int GetHashCode()
        {
            return (int)Value;
        }

        public static bool operator ==(ID left, ID right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ID left, ID right)
        {
            return !left.Equals(right);
        }

        public ID(uint id)
        {
            Value = id;
        }

        public override string ToString() => "#" + Value;
    }
}
