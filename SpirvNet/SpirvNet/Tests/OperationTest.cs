using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SpirvNet.DotNet;
using SpirvNet.Spirv;
using SpirvNet.Interpreter;

namespace SpirvNet.Tests
{
    [TestFixture]
    public class OperationTest
    {
        public int OpAddInt32(int a, int b) => a + b;
        public long OpAddInt64(long a, long b) => a + b;
        public uint OpAddUInt32(uint a, uint b) => a + b;
        public ulong OpAddUInt64(ulong a, ulong b) => a + b;
        public double OpAddFloat64(double a, double b) => a + b;
        public float OpAddFloat32(float a, float b) => a + b;

        public float OpSub(float a, float b) => a - b;
        public float OpDiv(float a, float b) => a / b;
        public float OpMul(float a, float b) => a * b;

        public float OpMixed(float a, float b) => (a + b * 7) * (b - a / 1000);

        [Test]
        public void SimpleArithmetics()
        {
            RandomTest<float>("OpAddFloat32", (a, b) => a + b);
            RandomTest<double>("OpAddFloat64", (a, b) => a + b);
            RandomTest<int>("OpAddInt32", (a, b) => a + b);
            RandomTest<long>("OpAddInt64", (a, b) => a + b);
            RandomTest<uint>("OpAddUInt32", (a, b) => a + b);
            RandomTest<ulong>("OpAddUInt64", (a, b) => a + b);

            RandomTest<float>("OpSub", (a, b) => a - b);
            RandomTest<float>("OpMul", (a, b) => a * b);
            RandomTest<float>("OpDiv", (a, b) => a / b);

            RandomTest<float>("OpMixed", (a, b) => (a + b * 7) * (b - a / 1000));
        }

        private void RandomTest<T>(string funcname, Func<T, T, T> op)
        {
            var cfunc = GetType().GetMethod(funcname);
            var random = new Random(funcname.GetHashCode());
            var machine = MachineFor(funcname);
            var func = machine.Module.Functions.First();

            for (var i = 0; i < 200; ++i)
            {
                var ra = random.Next(101, 200);
                var rb = random.Next(1, 100);

                var a = TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(ra.ToString());
                var b = TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(rb.ToString());

                var r0 = op((T)a, (T)b);
                var r1 = cfunc.Invoke(this, new[] { a, b });
                var r2 = machine.Execute(func, a, b);

                Assert.AreEqual(r0, r1);
                Assert.AreEqual(r1, r2);
            }
        }

        private Machine MachineFor(string func)
        {
            var def = CecilLoader.DefinitionFor(this, func);
            Assert.AreEqual(func, def.Name);

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

            return machine;
        }
    }
}
