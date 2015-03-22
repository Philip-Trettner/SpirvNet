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
        /* http://stackoverflow.com/questions/7212255/cecil-instruction-operand-types-corresponding-to-instruction-opcode-code-value
            Instruction.OpCode.Code|Instruction.OpCode.OperandType|Instruction.Operand class
            Ldarg_S                |ShortInlineArg                |ParameterDefinition
            Ldarga_S               |ShortInlineArg                |ParameterDefinition
            Starg_S                |ShortInlineArg                |ParameterDefinition
            Ldloc_S                |ShortInlineVar                |VariableDefinition
            Ldloca_S               |ShortInlineVar                |VariableDefinition
            Stloc_S                |ShortInlineVar                |VariableDefinition
            Ldc_I4_S               |ShortInlineI                  |sbyte <===== NOTE: special case
            Ldc_I4                 |InlineI                       |int32
            Ldc_I8                 |InlineI8                      |int64
            Ldc_R4                 |ShortInlineR                  |single
            Ldc_R8                 |InlineR                       |float (64 bit)
            Jmp                    |InlineMethod                  |MethodReference
            Call                   |InlineMethod                  |MethodReference
            Calli                  |InlineSig                     |CallSite
            Br_S                   |ShortInlineBrTarget           |Instruction
            Brfalse_S              |ShortInlineBrTarget           |Instruction
            Brtrue_S               |ShortInlineBrTarget           |Instruction
            Beq_S                  |ShortInlineBrTarget           |Instruction
            Bge_S                  |ShortInlineBrTarget           |Instruction
            Bgt_S                  |ShortInlineBrTarget           |Instruction
            Ble_S                  |ShortInlineBrTarget           |Instruction
            Blt_S                  |ShortInlineBrTarget           |Instruction
            Bne_Un_S               |ShortInlineBrTarget           |Instruction
            Bge_Un_S               |ShortInlineBrTarget           |Instruction
            Bgt_Un_S               |ShortInlineBrTarget           |Instruction
            Ble_Un_S               |ShortInlineBrTarget           |Instruction
            Blt_Un_S               |ShortInlineBrTarget           |Instruction
            Br                     |InlineBrTarget                |Instruction
            Brfalse                |InlineBrTarget                |Instruction
            Brtrue                 |InlineBrTarget                |Instruction
            Beq                    |InlineBrTarget                |Instruction
            Bge                    |InlineBrTarget                |Instruction
            Bgt                    |InlineBrTarget                |Instruction
            Ble                    |InlineBrTarget                |Instruction
            Blt                    |InlineBrTarget                |Instruction
            Bne_Un                 |InlineBrTarget                |Instruction
            Bge_Un                 |InlineBrTarget                |Instruction
            Bgt_Un                 |InlineBrTarget                |Instruction
            Ble_Un                 |InlineBrTarget                |Instruction
            Blt_Un                 |InlineBrTarget                |Instruction
            Switch                 |InlineSwitch                  |Instruction array
            Callvirt               |InlineMethod                  |MethodReference
            Cpobj                  |InlineType                    |TypeReference
            Ldobj                  |InlineType                    |TypeReference
            Ldstr                  |InlineString                  |string
            Newobj                 |InlineMethod                  |MethodReference
            Castclass              |InlineType                    |TypeReference
            Isinst                 |InlineType                    |TypeReference
            Unbox                  |InlineType                    |TypeReference
            Ldfld                  |InlineField                   |FieldReference
            Ldflda                 |InlineField                   |FieldReference
            Stfld                  |InlineField                   |FieldReference
            Ldsfld                 |InlineField                   |FieldReference
            Ldsflda                |InlineField                   |FieldReference
            Stsfld                 |InlineField                   |FieldReference
            Stobj                  |InlineType                    |TypeReference
            Box                    |InlineType                    |TypeReference
            Newarr                 |InlineType                    |TypeReference
            Ldelema                |InlineType                    |TypeReference
            Ldelem_Any             |InlineType                    |TypeReference
            Stelem_Any             |InlineType                    |TypeReference
            Unbox_Any              |InlineType                    |TypeReference
            Refanyval              |InlineType                    |TypeReference
            Mkrefany               |InlineType                    |TypeReference
            Ldtoken                |InlineTok                     |IMetadataTokenProvider
            Leave                  |InlineBrTarget                |Instruction
            Leave_S                |ShortInlineBrTarget           |Instruction
            Ldftn                  |InlineMethod                  |MethodReference
            Ldvirtftn              |InlineMethod                  |MethodReference
            Ldarg                  |InlineArg                     |ParameterDefinition
            Ldarga                 |InlineArg                     |ParameterDefinition
            Starg                  |InlineArg                     |ParameterDefinition
            Ldloc                  |InlineVar                     |VariableDefinition
            Ldloca                 |InlineVar                     |VariableDefinition
            Stloc                  |InlineVar                     |VariableDefinition
            Unaligned              |ShortInlineI                  |byte
            Initobj                |InlineType                    |TypeReference
            Constrained            |InlineType                    |TypeReference
            No                     |ShortInlineI                  |byte
            Sizeof                 |InlineType                    |TypeReference
        */


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
            var rettypename = method.ReturnType.FullName.CecilFullType();
            var paratypename = method.GetParameters().Select(p => p.ParameterType.FullName.CecilFullType()).ToArray();
            var defs = DefinitionFor(type?.Assembly);
            var tfullname = type?.FullName.CecilFullType();

            foreach (var def in defs)
                foreach (var module in def.Modules)
                    foreach (var func in module.GetType(tfullname)?.Methods ?? new Collection<MethodDefinition>())
                        if (func.Name == mname &&
                            func.ReturnType.FullName == rettypename &&
                            paratypename.SequenceEqual(func.Parameters.Select(p => p.ParameterType.FullName)))
                            return func;

            throw new KeyNotFoundException("Could not locate " + method.Name);
        }

        /// <summary>
        /// Returns the type ref for a given type
        /// </summary>
        public static TypeReference TypeReferenceFor(Type type)
        {
            var defs = DefinitionFor(type.Assembly);
            var tfullname = type.FullName.CecilFullType();

            foreach (var def in defs)
                foreach (var module in def.Modules)
                {
                    var tref = module.GetType(tfullname);
                    if (tref != null)
                        return tref;
                }

            throw new KeyNotFoundException("Could not locate " + type);
        }

        /// <summary>
        /// Returns the definition for a given method
        /// (Searches through the type and should therefore not be used often)
        /// </summary>
        public static MethodDefinition DefinitionFor(Type type, string name) => DefinitionFor(type.GetMethod(name));


        /// <summary>
        /// Returns the definition for a given method
        /// (Searches through the type and should therefore not be used often)
        /// </summary>
        public static MethodDefinition DefinitionFor(object obj, string name) => DefinitionFor(obj.GetType().GetMethod(name));

        /// <summary>
        /// Returns the definition for a given method
        /// (Searches through the type and should therefore not be used often)
        /// </summary>
        public static MethodDefinition DefinitionFor(Delegate d) => DefinitionFor(d.Method);

        /// <summary>
        /// Creates a CSV Dump of the specified method
        /// </summary>
        public static IEnumerable<string> CsvDump(MethodDefinition method)
        {
            var body = method.Body;

            yield return string.Format("Method;{0};{1}", method.Name, method.DeclaringType);
            yield return "";
            yield return "Stats";
            yield return string.Format("Local Vars;{0}", body.Variables.Count);
            yield return string.Format("Instructions;{0}", body.Instructions.Count);
            yield return string.Format("Code Size;{0}", body.CodeSize);
            yield return string.Format("Max Stack Size;{0}", body.MaxStackSize);
            yield return string.Format("Init Locals;{0}", body.InitLocals);
            yield return string.Format("Exception Handlers;{0}", body.ExceptionHandlers.Count);
            yield return string.Format("This Parameter;{0}", body.ThisParameter);

            if (method.Parameters.Count > 0)
            {
                yield return "";
                yield return "Parameters";
                yield return "Index;Name;Type";
                foreach (var p in method.Parameters)
                    yield return string.Format("{0};{1};{2}", p.Index, p.Name, p.ParameterType);
            }

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
        public static IEnumerable<string> CsvDump(MethodInfo method) => CsvDump(DefinitionFor(method));
        /// <summary>
        /// Creates a CSV Dump of the specified method
        /// </summary>
        public static IEnumerable<string> CsvDump(Delegate d) => CsvDump(d.Method);
    }
}
