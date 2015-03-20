using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpirvNet.Spirv
{
    public static class UniversalLimits
    {
        public const int MaxLiteralNameLength = 1024;
        public const int MaxLiteralStringeLength = 1024;
        public const int MaxInstructionWordCount = 264;
        public const int MaxResultIdBound = 4194304;
        public const int MaxControlFlowNestingDepth = 1024;
        public const int MaxGlobalVariables = 65536;
        public const int MaxLocalVariables = 524288;
        public const int MaxExecutionModesPerEntryPoint = 256;
        public const int MaxIndexForChainComposite = 256;
        public const int MaxFunctionParameters = 256;
        public const int MaxFunctionCallArguments = 256;
        public const int MaxExtInstArguments = 256;
        public const int MaxSwitchPairs = 16384;
        public const int MaxStructMembers = 16384;
        public const int MaxStructureNestingDeoth = 256;
    }
}
