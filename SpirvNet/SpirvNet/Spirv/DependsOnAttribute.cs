using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv.Enums;

namespace SpirvNet.Spirv
{

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    internal sealed class DependsOnAttribute : Attribute
    {
        public LanguageCapability Capability { get; private set; }
        
        public DependsOnAttribute(LanguageCapability capability)
        {
            Capability = capability;
        }
    }
}
