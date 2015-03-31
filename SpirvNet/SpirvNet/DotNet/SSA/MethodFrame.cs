using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebugPage;
using Mono.Cecil;
using SpirvNet.DotNet.CFG;
using SpirvNet.Spirv;
using SpirvNet.Spirv.Enums;
using SpirvNet.Spirv.Ops.FlowControl;
using SpirvNet.Spirv.Ops.Function;

namespace SpirvNet.DotNet.SSA
{
    /// <summary>
    /// Frame analysis for a method
    /// </summary>
    public class MethodFrame
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
        /// Associated type builder
        /// </summary>
        public readonly TypeBuilder TypeBuilder;
        /// <summary>
        /// Provider for called methods
        /// </summary>
        public readonly IFunctionProvider FunctionProvider;

        /// <summary>
        /// Function parameter types
        /// </summary>
        public readonly SpirvType[] ParameterTypes;
        /// <summary>
        /// Function return type
        /// </summary>
        public readonly SpirvType ReturnType;

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
        /// True iff method frame was analysed
        /// </summary>
        public bool Analysed { get; set; } = false;

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
        /// List of function blocks
        /// </summary>
        public readonly List<MethodBlock> Blocks = new List<MethodBlock>();

        /// <summary>
        /// Helper for vertex -> state
        /// </summary>
        public MethodFrameState FromVertex(Vertex v) => States[v.Index];

        public MethodFrame(ControlFlowGraph cfg, TypeBuilder typeBuilder, IDAllocator allocator, IFunctionProvider functionProvider = null)
        {
            // init options
            CFG = cfg;
            TypeBuilder = typeBuilder;
            Allocator = allocator;
            FunctionProvider = functionProvider;
            Method = cfg.Method;
            ArgCount = Method.Parameters.Count;
            VarCount = Method.Body.Variables.Count;
            StackSize = Method.Body.MaxStackSize;
            InitLocalVars = Method.Body.InitLocals;
            HasThis = Method.HasThis;

            if (HasThis) ++ArgCount; // zero is this

            // types
            ParameterTypes = Method.Parameters.Select(p => typeBuilder.Create(p.ParameterType)).ToArray();
            ReturnType = typeBuilder.Create(Method.ReturnType);

            // init arg locations
            // TODO: Init code
            ArgLocations = new TypedLocation[ArgCount];
            for (var i = 0; i < ArgCount; ++i)
            {
                var pi = !HasThis ? i : i - 1;
                if (i == 0 && HasThis)
                {
                    // no-op (this not really supported)
                    ArgLocations[0] = TypedLocation.SpecialThis;
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

            // zero-init vars
            if (InitLocalVars)
                for (var i = 0; i < VarCount; ++i)
                    LocalVars[i] = new TypedLocation(TypeBuilder.ConstantZero(LocalVarTypes[i]), LocalVarTypes[i]);
        }

        /// <summary>
        /// Analyses this method if not alreay done
        /// </summary>
        public void Analyse()
        {
            if (Analysed)
                return;

            // create stack frames
            foreach (var vertex in CFG.Vertices)
                States.Add(new MethodFrameState(vertex, this));
            // connectivity
            foreach (var state in States)
                state.BuildConnections();
            // decode
            States[0].DecodeOp(null);

            // update block connectivity
            foreach (var s1 in States)
                foreach (var s2 in s1.Outgoing)
                    MethodBlock.AddConnection(s1, s2);
            foreach (var block in Blocks)
                block.AddMissingBranches();
            foreach (var block in Blocks)
                block.Validate();

            // finally, phis
            foreach (var state in States)
                state.CreatePhis();

            Analysed = true;
        }

        /// <summary>
        /// Performs initial function setup (no instruction generation or analysing yet)
        /// </summary>
        public void Setup(FunctionBuilder builder)
        {
            // register parameters
            for (var i = 0; i < Method.Parameters.Count; ++i)
            {
                var para = Method.Parameters[i];
                var name = para.Name;
                var argloc = ArgLocations[HasThis ? i + 1 : i];

                builder.AddParameter(new OpFunctionParameter { Result = argloc.ID, ResultType = argloc.Type.TypeID }, argloc.Type, name);
            }

            // setup function
            var returnType = TypeBuilder.Create(Method.ReturnType);
            builder.SetupFunction(returnType, TypeBuilder, Allocator);
        }
        /// <summary>
        /// Builds the function instruction (must be called after Setup)
        /// </summary>
        public void Build(FunctionBuilder builder)
        {
            // analyse on demand
            Analyse();

            // add code
            foreach (var state in States)
                foreach (var op in state.CreateOps())
                    builder.AddOp(op);
        }

        public IEnumerable<string> DotFile
        {
            get
            {
                // analyse on demand
                Analyse();

                yield return "digraph MethodFrame {";
                var i = 0;
                foreach (var block in Blocks)
                {
                    yield return string.Format("  subgraph cluster_{0} {{", i++);
                    yield return string.Format("    label=\"{0}\";", "Block " + i);
                    foreach (var s in block.States)
                        foreach (var line in s.DotLines)
                            yield return "    " + line;
                    yield return "  }";
                }
                foreach (var v1 in States)
                    foreach (var v2 in v1.Outgoing)
                        yield return string.Format("v{0} -> v{1};", v1.Vertex.Index, v2.Vertex.Index);
                yield return "}";
            }
        }

        /// <summary>
        /// Adds a dot file 
        /// </summary>
        public void AddDebugPageTo(PageElement e)
        {
            // analyse on demand
            Analyse();

            e.AddDotGraph(DotFile);
        }
    }
}
