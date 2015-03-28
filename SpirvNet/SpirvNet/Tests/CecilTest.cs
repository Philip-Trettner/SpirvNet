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
using SpirvNet.Helper;
using SpirvNet.Spirv;
using SpirvNet.Validation;

namespace SpirvNet.Tests
{
    [TestFixture]
    public class CecilTest
    {
        public float SimpleAdd(float a, float b)
        {
            return a + b + 1;
        }
        public float SimpleBranch(float a, float b)
        {
            if (a < b)
                return a + 1;
            else
                return b - a;
        }
        public float SimpleMod(float a, float b)
        {
            int i = (int)a;
            uint u = (uint)b;
            i = i % 5;
            u = u % 17;
            a = b % a;
            return i + a + u;
        }

        public float SimpleLoop(int k)
        {
            float f = 3;
            for (int i = 0; i < k; ++i)
                f += i;
            return f;
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

            var modbuilder = new ModuleBuilder();
            modbuilder.CreateFunction(def);
            var mod = modbuilder.CreateModule();

            Assert.Greater(mod.Instructions.Count, 10);

            mod.SetBoundAutomatically();
            ValidatedModule vmod = null;
            Assert.DoesNotThrow(() => vmod = mod.Validate());
            Assert.AreEqual(1, vmod.Functions.Count);

            //foreach (var line in CecilLoader.CsvDump(def))
            //    Console.WriteLine(line);
            //File.WriteAllLines(@"C:\Temp\simpleadd.dot", cfg.DotFile);
            //File.WriteAllLines(@"C:\Temp\simpleadd.csv", CecilLoader.CsvDump(def));
            //File.WriteAllLines(@"C:\Temp\simpleadd.spirv.csv", mod.CSVDump());
        }

        [Test]
        public void SimpleBranchTest()
        {
            var def = CecilLoader.DefinitionFor(this, "SimpleBranch");
            Assert.AreEqual("SimpleBranch", def.Name);
            
            var cfg = new ControlFlowGraph(def);

            var modbuilder = new ModuleBuilder();
            modbuilder.CreateFunction(def);
            var mod = modbuilder.CreateModule();

            var allocator = new IDAllocator();
            var typeBuilder = new TypeBuilder(allocator);
            var frame = new MethodFrame(cfg, typeBuilder, allocator);

            DebugHelper.CreatePage(def, cfg, frame, mod).WriteToTempAndOpen();
            //DotHelper.Execute(@"C:\Temp\simplebranch.dot", cfg.DotFile);
            //File.WriteAllLines(@"C:\Temp\simplebranch.dot", cfg.DotFile);
            //File.WriteAllLines(@"C:\Temp\simplebranch.frame.dot", frame.DotFile);
            //File.WriteAllLines(@"C:\Temp\simplebranch.csv", CecilLoader.CsvDump(def));
            //File.WriteAllLines(@"C:\Temp\simplebranch.spirv.csv", mod.CSVDump());
        }

        [Test]
        public void SimpleModTest()
        {
            var def = CecilLoader.DefinitionFor(this, "SimpleMod");
            Assert.AreEqual("SimpleMod", def.Name);

            var cfg = new ControlFlowGraph(def);

            //File.WriteAllLines(@"C:\Temp\SimpleMod.dot", cfg.DotFile);
            //File.WriteAllLines(@"C:\Temp\SimpleMod.csv", CecilLoader.CsvDump(def));
        }

        [Test]
        public void SimpleLoopTest()
        {
            var def = CecilLoader.DefinitionFor(this, "SimpleLoop");
            Assert.AreEqual("SimpleLoop", def.Name);

            var cfg = new ControlFlowGraph(def);

            var modbuilder = new ModuleBuilder();
            modbuilder.CreateFunction(def);
            var mod = modbuilder.CreateModule();

            var allocator = new IDAllocator();
            var typeBuilder = new TypeBuilder(allocator);
            var frame = new MethodFrame(cfg, typeBuilder, allocator);

            //File.WriteAllLines(@"C:\Temp\SimpleLoop.dot", cfg.DotFile);
            //File.WriteAllLines(@"C:\Temp\SimpleLoop.csv", CecilLoader.CsvDump(def));
            //File.WriteAllLines(@"C:\Temp\SimpleLoop.frame.dot", frame.DotFile);
            //File.WriteAllLines(@"C:\Temp\SimpleLoop.spirv.csv", mod.CSVDump());
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
