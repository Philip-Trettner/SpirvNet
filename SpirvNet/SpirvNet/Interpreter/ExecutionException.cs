using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv;

namespace SpirvNet.Interpreter
{
    /// <summary>
    /// Thrown when an execution error occurred
    /// </summary>
    [Serializable]
    public class ExecutionException : Exception
    {
        public Instruction Instruction { get; set; }

        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ExecutionException()
        {
        }

        public ExecutionException(Instruction instruction, string message) : base((instruction == null ? "" : instruction + ": ") + message)
        {
            Instruction = instruction;
        }
    }
}
