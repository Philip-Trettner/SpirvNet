using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpirvNet.Helper
{
    static class DotHelper
    {
        public static void Execute(string dotFilename, IEnumerable<string> dotFile)
        {
            File.WriteAllLines(dotFilename, dotFile);
            Process.Start("cmd", string.Format("/C \"dot -Tsvg {0} > {0}.svg\"", dotFilename))?.WaitForExit();
        }
    }
}
