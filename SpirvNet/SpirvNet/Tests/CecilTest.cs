using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SpirvNet.DotNet;
using SpirvNet.DotNet.CFG;
using SpirvNet.DotNet.SSA;
using SpirvNet.Spirv;

namespace SpirvNet.Tests
{
    [TestFixture]
    public class CecilTest
    {
        public float SimpleAdd(float a, float b)
        {
            return a + b + 1;
        }

        [Test]
        public void SimpleAddTest()
        {
            var def = CecilLoader.DefinitionFor(this, "SimpleAdd");
            Assert.AreEqual("SimpleAdd", def.Name);

            var cfg = new ControlFlowGraph(def);
            var allocator = new IDAllocator();
            var typeBuilder = new TypeBuilder(allocator);
            var frame = new MethodFrame(cfg, typeBuilder, allocator);

            foreach (var state in frame.States)
                Assert.That(state.IsLinearFlow);
            
            //foreach (var line in CecilLoader.CsvDump(def))
            //    Console.WriteLine(line);
            File.WriteAllLines(@"C:\Temp\simpleadd.dot", cfg.DotFile);
            File.WriteAllLines(@"C:\Temp\simpleadd.csv", CecilLoader.CsvDump(def));
        }

        [Test]
        public void MethodTest()
        {
            var def0 = CecilLoader.DefinitionFor((Action)MethodTest);
            Assert.AreEqual("MethodTest", def0.Name);

            var def1 = CecilLoader.DefinitionFor((Func<double, double>)Math.Abs);
            Assert.AreEqual("Abs", def1.Name);
            Assert.AreEqual("Double", def1.ReturnType.Name);
            Assert.AreEqual(1, def1.Parameters.Count);
            Assert.AreEqual("Double", def1.Parameters[0].ParameterType.Name);

            //File.WriteAllLines(@"C:\Temp\testdump.csv", CecilLoader.CsvDump((Action)MethodTest));
        }
    }
}
