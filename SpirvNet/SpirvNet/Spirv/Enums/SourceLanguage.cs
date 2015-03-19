using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpirvNet.Spirv.Enums
{
    /// <summary>
    /// The source language is an annotation, with no semantics that effect 
    /// the meaning of other parts of the module. Used by OpSource
    /// </summary>
    public enum SourceLanguage
    {
        Unknown = 0,
        ESSL = 1,
        GLSL = 2,
        OpenCL = 3
    }
}
