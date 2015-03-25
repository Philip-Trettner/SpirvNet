using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv;

namespace SpirvNet.Validation
{
    /// <summary>
    /// Thrown when module validation failed
    /// </summary>
    [Serializable]
    public class ValidationException : Exception
    {
        public Instruction Instruction { get; set; }

        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ValidationException()
        {
        }

        public ValidationException(Instruction instruction, string message) : base((instruction == null ? "" : instruction + ": ") + message)
        {
            Instruction = instruction;
        }
    }
}
