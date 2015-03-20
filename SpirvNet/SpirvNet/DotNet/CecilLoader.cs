using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Collections.Generic;

namespace SpirvNet.DotNet
{
    /// <summary>
    /// Static loader for assemblies in cecil
    /// </summary>
    static class CecilLoader
    {
        /// <summary>
        /// Cache
        /// </summary>
        private static readonly Dictionary<Assembly, AssemblyDefinition[]> Assemblies = new Dictionary<Assembly, AssemblyDefinition[]>();

        /// <summary>
        /// Returns all assembly definitions of a given assembly (caches result)
        /// </summary>
        public static AssemblyDefinition[] DefinitionFor(Assembly asm)
        {
            if (asm == null)
                return new AssemblyDefinition[] { };

            AssemblyDefinition[] definitions;
            if (Assemblies.TryGetValue(asm, out definitions))
                return definitions;

            definitions = asm.GetFiles().Select(AssemblyDefinition.ReadAssembly).ToArray();
            Assemblies.Add(asm, definitions);
            return definitions;
        }

        /// <summary>
        /// Returns the definition for a given method
        /// (Searches through the type and should therefore not be used often)
        /// </summary>
        public static MethodDefinition DefinitionFor(MethodInfo method)
        {
            var type = method.DeclaringType;
            var mname = method.Name;
            var rettypename = method.ReturnType.FullName;
            var paratypename = method.GetParameters().Select(p => p.ParameterType.FullName).ToArray();
            var defs = DefinitionFor(type?.Assembly);

            foreach (var def in defs)
                foreach (var module in def.Modules)
                    foreach (var func in module.GetType(type?.FullName)?.Methods ?? new Collection<MethodDefinition>())
                        if (func.Name == mname &&
                            func.ReturnType.FullName == rettypename &&
                            paratypename.SequenceEqual(func.Parameters.Select(p => p.ParameterType.FullName)))
                            return func;

            throw new KeyNotFoundException("Could not locate " + method.Name);
        }

        /// <summary>
        /// Returns the definition for a given method
        /// (Searches through the type and should therefore not be used often)
        /// </summary>
        public static MethodDefinition DefinitionFor(Delegate d) => DefinitionFor(d.Method);

        /// <summary>
        /// Creates a CSV Dump of the specified method
        /// </summary>
        public static IEnumerable<string> CsvDump(MethodInfo method)
        {
            var def = DefinitionFor(method);
            var body = def.Body;

            yield return string.Format("Method;{0};{1}", method.Name, method.DeclaringType);
            yield return "";
            yield return "Stats";
            yield return string.Format("Local Vars;{0}", body.Variables.Count);
            yield return string.Format("Instructions;{0}", body.Instructions.Count);
            yield return string.Format("Code Size;{0}", body.CodeSize);
            yield return string.Format("Max Stack Size;{0}", body.MaxStackSize);
            yield return string.Format("Init Locals;{0}", body.InitLocals);
            yield return string.Format("Exception Handlers;{0}", body.ExceptionHandlers.Count);
            if (body.HasExceptionHandlers)
            {
                yield return "";
                yield return "Exception Handlers";
                yield return "Catch Type;Filter Start;Handler Type;Handler Start;Handler End;Try Start;Try End";
                foreach (var h in body.ExceptionHandlers)
                    yield return string.Format("{0};{1};{2};{3};{4};{5};{6}", h.CatchType, h.FilterStart, h.HandlerType, h.HandlerStart, h.HandlerEnd, h.TryStart, h.TryEnd);
            }
            if (body.HasVariables)
            {
                yield return "";
                yield return "Variables";
                yield return "Index;Pinned;Name;Type";
                foreach (var v in body.Variables)
                    yield return string.Format("{0};{1};{2};{3}", v.Index, v.IsPinned, v.Name, v.VariableType);
            }

            yield return "";
            yield return "Instructions";
            yield return "Offset;Size;Code;Operand;Operand Type;Flow Control;Op1;Op2;Op Code Type;Operand Type;Size;Stack Pop;Stack Push;Value";
            foreach (var i in body.Instructions)
            {
                var o = i.OpCode;
                yield return string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13}",
                    i.Offset, i.GetSize(), o.Code, i.Operand, i.Operand?.GetType().Name, o.FlowControl, o.Op1, o.Op2, o.OpCodeType, o.OperandType,
                    o.Size, o.StackBehaviourPop, o.StackBehaviourPush, o.Value);
            }
        }

        /// <summary>
        /// Creates a CSV Dump of the specified method
        /// </summary>
        public static IEnumerable<string> CsvDump(Delegate d) => CsvDump(d.Method);
    }
}
