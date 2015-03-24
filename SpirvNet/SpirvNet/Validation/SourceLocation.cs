using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpirvNet.Validation
{
    /// <summary>
    /// Debug information for source locations
    /// </summary>
    public class SourceLocation
    {
        public readonly string File;
        public readonly uint Line;
        public readonly uint Column;

        public SourceLocation(string file, uint line, uint column)
        {
            File = file;
            Line = line;
            Column = column;
        }
    }
}
