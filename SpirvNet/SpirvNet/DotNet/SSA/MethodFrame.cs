using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.DotNet.CFG;

namespace SpirvNet.DotNet.SSA
{
    /// <summary>
    /// Frame analysis for a method
    /// </summary>
    class MethodFrame
    {
        /// <summary>
        /// List of states
        /// </summary>
        public readonly List<MethodFrameState> States = new List<MethodFrameState>();

        /// <summary>
        /// Represented CFG
        /// </summary>
        public readonly ControlFlowGraph CFG;

        /// <summary>
        /// Argument locations
        /// </summary>
        public readonly TypedLocation[] ArgLocations;
        /// <summary>
        /// local var locations
        /// </summary>
        public readonly TypedLocation[] LocalVars;
        /// <summary>
        /// local var types
        /// </summary>
        public readonly SpirvType[] LocalVarTypes;

        /// <summary>
        /// Next SSA location
        /// </summary>
        private uint nextLocation = 0;

        /// <summary>
        /// Associated type builder
        /// </summary>
        public readonly TypeBuilder TypeBuilder;

        /// <summary>
        /// Number of function args
        /// </summary>
        public readonly int ArgCount;
        /// <summary>
        /// Number of local vars
        /// </summary>
        public readonly int VarCount;
        /// <summary>
        /// Max stack size
        /// </summary>
        public readonly int StackSize;
        /// <summary>
        /// If true, local vars are initialized
        /// </summary>
        public readonly bool InitLocalVars;

        /// <summary>
        /// Creates a new SSA location
        /// </summary>
        public TypedLocation CreateLocation(SpirvType type) => new TypedLocation(type, nextLocation++);

        /// <summary>
        /// Helper for vertex -> state
        /// </summary>
        public MethodFrameState FromVertex(Vertex v) => States[v.Index];

        public MethodFrame(ControlFlowGraph cfg, TypeBuilder typeBuilder)
        {
            // init options
            CFG = cfg;
            TypeBuilder = typeBuilder;
            ArgCount = cfg.Method.Parameters.Count;
            VarCount = cfg.Method.Body.Variables.Count;
            StackSize = cfg.Method.Body.MaxStackSize;
            InitLocalVars = cfg.Method.Body.InitLocals;

            // init arg locations
            ArgLocations = new TypedLocation[ArgCount];
            for (var i = 0; i < ArgCount; ++i)
            {
                var argType = TypeBuilder.Create(cfg.Method.Parameters[i].ParameterType);
                ArgLocations[i] = CreateLocation(argType);
            }

            // init local var
            LocalVars = new TypedLocation[VarCount];
            LocalVarTypes = new SpirvType[VarCount];
            for (var i = 0; i < ArgCount; ++i)
                LocalVarTypes[i] = TypeBuilder.Create(cfg.Method.Body.Variables[i].VariableType);
            if (InitLocalVars)
                for (var i = 0; i < ArgCount; ++i)
                    LocalVars[i] = CreateLocation(LocalVarTypes[i]);

            // create stack frames
            foreach (var vertex in cfg.Vertices)
                States.Add(new MethodFrameState(vertex, this));
            // connectivity
            foreach (var state in States)
                state.BuildConnections();
            // decode
            States[0].DecodeOp(null);
            // finally, phis
            foreach (var state in States)
                state.CreatePhis();
        }
    }
}
