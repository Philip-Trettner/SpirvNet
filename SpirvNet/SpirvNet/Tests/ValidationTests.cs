using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SpirvNet.Spirv;
using SpirvNet.Spirv.Ops.FlowControl;
using SpirvNet.Spirv.Ops.Function;
using SpirvNet.Spirv.Ops.ModeSetting;
using SpirvNet.Spirv.Ops.TypeDeclaration;
using SpirvNet.Validation;

namespace SpirvNet.Tests
{
    [TestFixture]
    public class ValidationTests
    {
        [Test]
        public void SimplestValidModule()
        {
            var mod = new Module();
            mod.Instructions.Add(new OpMemoryModel());
            mod.Instructions.Add(new OpEntryPoint { EntryPoint = new ID(4) });
            mod.Instructions.Add(new OpTypeVoid { Result = new ID(2) });
            mod.Instructions.Add(new OpTypeFunction { Result = new ID(3), ReturnType = new ID(2) });
            mod.Instructions.Add(new OpFunction { Result = new ID(4), ResultType = new ID(2), FunctionType = new ID(3) });
            mod.Instructions.Add(new OpLabel { Result = new ID(5) });
            mod.Instructions.Add(new OpReturn());
            mod.Instructions.Add(new OpFunctionEnd());
            mod.SetBoundManually(6);

            ValidatedModule vmod = null;
            Assert.DoesNotThrow(() => vmod = mod.Validate());

            Assert.AreEqual(1, vmod.EntryPoints.Count);
            Assert.AreEqual(1, vmod.Functions.Count);

            for (var i = 0; i < mod.Instructions.Count; ++i)
            {
                var mod2 = mod.Clone();
                if (mod2.Instructions[i] is OpEntryPoint)
                    continue; // no entry point is more or less ok
                mod2.Instructions.RemoveAt(i);

                Assert.Throws<ValidationException>(() => mod2.Validate());
            }
        }
    }
}
