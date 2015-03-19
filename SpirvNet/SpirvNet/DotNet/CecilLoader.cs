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
    }
}
