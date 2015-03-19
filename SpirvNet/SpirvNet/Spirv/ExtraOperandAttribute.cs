using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

namespace SpirvNet.Spirv
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    internal sealed class ExtraOperandAttribute : Attribute
    {
        // This is a positional argument
        public ExtraOperandAttribute(OperandType type, string desc)
        {
            Type = type;
            Description = desc;
        }

        public string Description { get; private set; }
        public OperandType Type { get; private set; }
    }
}
