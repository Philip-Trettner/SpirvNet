using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SpirvNet.DotNet;
using SpirvNet.DotNet.CFG;
using SpirvNet.DotNet.SSA;
using SpirvNet.Interpreter;
using SpirvNet.Spirv;
using SpirvNet.Validation;

namespace SpirvNet.Tests
{
    [TestFixture]
    public class IntegrationTest
    {
        public float SimpleAdd(float x, float y)
        {
            return x + y + 3;
        }

        [Test]
        public void SimpleAddTest()
        {
            var def = CecilLoader.DefinitionFor(this, "SimpleAdd");
            Assert.AreEqual("SimpleAdd", def.Name);

            var modbuilder = new ModuleBuilder();
            modbuilder.CreateFunction(def);
            var mod = modbuilder.CreateModule();

            mod.SetBoundAutomatically();
            Assert.That(mod.CheckValidity());

            var vmod = mod.Validate();
            Assert.AreEqual(1, vmod.Functions.Count);

            var machine = new Machine(vmod);
            Assert.AreEqual(vmod.Bound, machine.Module.Bound);

            Assert.AreEqual(1, vmod.Functions.Count);

            var func = vmod.Functions.First();
            Assert.That(func.ReturnType.IsFloating);
            Assert.AreEqual(32, func.ReturnType.BitWidth);
            Assert.AreEqual(2, func.ParameterTypes.Count);
            Assert.That(func.ParameterTypes[0].IsFloating);
            Assert.That(func.ParameterTypes[1].IsFloating);
            Assert.AreEqual(32, func.ParameterTypes[0].BitWidth);
            Assert.AreEqual(32, func.ParameterTypes[1].BitWidth);

            var res = machine.Execute(func, 1f, 2f);
            Assert.AreEqual(1f + 2f + 3, (float)res);
            Assert.AreEqual(SimpleAdd(1f, 2f), (float)res);

            var random = new Random(123);
            for (var i = 0; i < 100; ++i)
            {
                var a = (float)random.NextDouble() * 100 - 50;
                var b = (float)random.NextDouble() * 100 - 50;

                res = machine.Execute(func, a, b);
                Assert.AreEqual(a + b + 3, (float)res);
                Assert.AreEqual(SimpleAdd(a, b), (float)res);
            }
        }
    }
}
