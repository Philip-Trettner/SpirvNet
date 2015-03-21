using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpirvNet.Spirv.Ops
{
    public struct Pair<U, V>
    {
        public U First;
        public V Second;

        public Pair(U u, V v)
        {
            First = u;
            Second = v;
        }

        public override string ToString() => string.Format("({0}, {1})", First, Second);
    }
}
