using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.DotNet.CFG;
using SpirvNet.Spirv;

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
        /// ID Allocator
        /// </summary>
        public readonly IDAllocator Allocator;

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
        /// If true, has this parameter
        /// </summary>
        public readonly bool HasThis;

        /// <summary>
        /// Creates a new SSA location
        /// </summary>
        public TypedLocation CreateLocation(SpirvType type)
        {
            var loc = new TypedLocation(type, Allocator);
            locations.Add(loc);
            return loc;
        }

        /// <summary>
        /// List of all known locs
        /// </summary>
        private readonly List<TypedLocation> locations = new List<TypedLocation>();

        /// <summary>
        /// Helper for vertex -> state
        /// </summary>
        public MethodFrameState FromVertex(Vertex v) => States[v.Index];

        public MethodFrame(ControlFlowGraph cfg, TypeBuilder typeBuilder, IDAllocator allocator)
        {
            // init options
            CFG = cfg;
            TypeBuilder = typeBuilder;
            Allocator = allocator;
            ArgCount = cfg.Method.Parameters.Count;
            VarCount = cfg.Method.Body.Variables.Count;
            StackSize = cfg.Method.Body.MaxStackSize;
            InitLocalVars = cfg.Method.Body.InitLocals;
            HasThis = cfg.Method.HasThis;

            if (HasThis) ++ArgCount; // zero is this

            // init arg locations
            // TODO: Init code
            ArgLocations = new TypedLocation[ArgCount];
            for (var i = 0; i < ArgCount; ++i)
            {
                var pi = !HasThis ? i : i - 1;
                var argType = i == 0 && HasThis ?
                    TypeBuilder.Create(typeof(void)) :
                    TypeBuilder.Create(cfg.Method.Parameters[pi].ParameterType);
                ArgLocations[i] = CreateLocation(argType);
            }

            // init local var
            LocalVars = new TypedLocation[VarCount];
            LocalVarTypes = new SpirvType[VarCount];
            for (var i = 0; i < VarCount; ++i)
                LocalVarTypes[i] = TypeBuilder.Create(cfg.Method.Body.Variables[i].VariableType);
            if (InitLocalVars)
                for (var i = 0; i < VarCount; ++i)
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
