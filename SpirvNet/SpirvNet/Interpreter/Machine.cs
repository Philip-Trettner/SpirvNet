using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpirvNet.Spirv;
using SpirvNet.Spirv.Enums;
using SpirvNet.Spirv.Ops.Arithmetic;
using SpirvNet.Spirv.Ops.FlowControl;
using SpirvNet.Validation;

namespace SpirvNet.Interpreter
{
    /// <summary>
    /// An execution machine for SPIR-V modules
    /// </summary>
    public class Machine
    {
        /// <summary>
        /// Currently loaded (validated) module
        /// </summary>
        public readonly ValidatedModule Module;

        /// <summary>
        /// Array of objects (and current values)
        /// Index = SSA location
        /// </summary>
        private readonly object[] values;

        public Machine(ValidatedModule module)
        {
            Module = module;

            if (module.Bound > 4194304)
                throw new NotSupportedException("Universal minimum is 4,194,304 locations. Probably a good limit for debug emulation.");

            values = new object[Module.Bound];

            // initialize constants
            foreach (var location in module.Locations)
                if (location.IsConstant)
                    values[location.ID] = location.Constant;
        }

        /// <summary>
        /// Sets the value of a given location
        /// </summary>
        private void Set(ID id, object obj, Instruction op)
        {
            var loc = Module.Locations[id.Value];

            if (loc.LocationType != LocationType.Intermediate)
                throw new ExecutionException(op, "Cannot assign an object to a non-intermediate location.");

            if (!loc.SpirvType.IsInstance(obj))
                throw new ExecutionException(op, string.Format("Cannot assign {0} ({1}) to location of type {2}.", obj, obj.GetType(), loc.SpirvType));

            values[id.Value] = obj;
        }
        /// <summary>
        /// Gets the object behind an ID
        /// </summary>
        private object Get(ID id)
        {
            return values[id.Value];
        }
        /// <summary>
        /// Typed Get
        /// </summary>
        private T Get<T>(ID id) => (T)Get(id);
        /// <summary>
        /// Returns the type of a given ID
        /// </summary>
        private SpirvType TypeOf(ID id)
        {
            return id.Value == 0 ? null : Module.Locations[id.Value].SpirvType;
        }

        /// <summary>
        /// Executes a function of this module
        /// </summary>
        public object Execute(ValidatedFunction function, params object[] args)
        {
            if (function.Module != Module)
                throw new NotSupportedException("Foreign functions not allowed");

            ValidatedBlock prevBlock = null, nextBlock = null;
            var currBlock = function.StartBlock;

            // assign parameters
            if (args.Length != function.ParameterLocations.Count)
                throw new ExecutionException(null, "Wrong number of parameters. Expected " + function.ParameterLocations.Count + ", got " + args.Length);

            for (var i = 0; i < function.ParameterLocations.Count; ++i)
            {
                var location = function.ParameterLocations[i];
                if (!location.SpirvType.IsInstance(args[i]))
                    throw new ExecutionException(null, "Parameter " + i + " is not of type " + location.SpirvType);
            }

            // execute function
            while (true)
                foreach (var instruction in currBlock.Instructions)
                {
                    var type = TypeOf(instruction.ResultTypeID ?? new ID(0));

                    switch (instruction.OpCode)
                    {
                        // arithmetics
                        case OpCode.FAdd:
                            {
                                var op = (OpFAdd)instruction;
                                if (type.IsFloating && type.BitWidth == 32)
                                    Set(op.Result, Get<float>(op.Operand1) + Get<float>(op.Operand2), op);
                                else if (type.IsFloating && type.BitWidth == 64)
                                    Set(op.Result, Get<double>(op.Operand1) + Get<double>(op.Operand2), op);
                                else throw new NotImplementedException();
                            }
                            break;

                        // flow-control
                        case OpCode.Branch:
                            Debug.Assert(currBlock.OutgoingBlocks.Count == 0);
                            nextBlock = currBlock.DefaultTarget;
                            break;

                        case OpCode.ReturnValue:
                            return Get(((OpReturnValue)instruction).Value);

                        default:
                            throw new NotImplementedException("Instruction " + instruction + " is either not supported or not implemented.");
                    }

                    // next block
                    if (nextBlock == null)
                        throw new InvalidOperationException("Unknown next block is kinda invalid.");
                    prevBlock = currBlock;
                    currBlock = nextBlock;
                }

            // unreachable
        }
    }
}
