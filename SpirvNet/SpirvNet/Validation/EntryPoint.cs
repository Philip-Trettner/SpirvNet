using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv;
using SpirvNet.Spirv.Enums;
using SpirvNet.Spirv.Ops.ModeSetting;

namespace SpirvNet.Validation
{
    /// <summary>
    /// An entry point
    /// </summary>
    public class EntryPoint
    {
        /// <summary>
        /// Location ID of the entry point
        /// </summary>
        public readonly ID EnryPointID;

        /// <summary>
        /// Execution model
        /// </summary>
        public readonly ExecutionModel ExecutionModel;

        /// <summary>
        /// Execution modes
        /// </summary>
        public readonly List<OpExecutionMode> ExecutionModes = new List<OpExecutionMode>(); 

        public EntryPoint(ID enryPointID, ExecutionModel executionModel)
        {
            EnryPointID = enryPointID;
            ExecutionModel = executionModel;
        }
    }
}
