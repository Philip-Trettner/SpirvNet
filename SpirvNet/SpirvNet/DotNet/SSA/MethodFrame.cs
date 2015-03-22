using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using SpirvNet.DotNet.CFG;
using SpirvNet.Spirv;
using SpirvNet.Spirv.Enums;
using SpirvNet.Spirv.Ops.Function;

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
        /// Method info
        /// </summary>
        public readonly MethodDefinition Method;

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
            Method = cfg.Method;
            ArgCount = Method.Parameters.Count;
            VarCount = Method.Body.Variables.Count;
            StackSize = Method.Body.MaxStackSize;
            InitLocalVars = Method.Body.InitLocals;
            HasThis = Method.HasThis;

            if (HasThis) ++ArgCount; // zero is this

            // init arg locations
            // TODO: Init code
            ArgLocations = new TypedLocation[ArgCount];
            for (var i = 0; i < ArgCount; ++i)
            {
                var pi = !HasThis ? i : i - 1;
                if (i == 0 && HasThis)
                {
                    // no-op (this not really supported)
                }
                else
                {
                    var argType = TypeBuilder.Create(cfg.Method.Parameters[pi].ParameterType);
                    ArgLocations[i] = CreateLocation(argType);
                }
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

        /// <summary>
        /// Builds this method into a function
        /// </summary>
        public void Build(FunctionBuilder builder)
        {
            // register parameters
            for (var i = 0; i < Method.Parameters.Count; ++i)
            {
                var para = Method.Parameters[i];
                var name = para.Name;
                var argloc = ArgLocations[HasThis ? i + 1 : i];

                builder.AddParameter(new OpFunctionParameter { Result = argloc.ID, ResultType = argloc.Type.TypeID }, name);
            }

            // setup function
            var returnType = TypeBuilder.Create(Method.ReturnType);
            builder.SetupFunction(returnType.TypeID, Allocator);

            // add code
            foreach (var state in States)
                foreach (var op in state.CreateOps())
                    builder.AddOp(op);
        }

        public IEnumerable<string> DotFile
        {
            get
            {
                yield return "digraph MethodFrame {";
                foreach (var v in States)
                    foreach (var line in v.DotLines)
                        yield return "  " + line;
                yield return "}";
            }
        }
    }
}
