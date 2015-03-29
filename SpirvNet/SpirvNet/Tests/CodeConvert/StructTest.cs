using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SpirvNet.DotNet;
using SpirvNet.DotNet.CFG;
using SpirvNet.Helper;
using SpirvNet.Spirv;

namespace SpirvNet.Tests.CodeConvert
{

    [TestFixture]
    public class StructTest
    {
        public struct Bar
        {
            public int a, b;
        }

        public struct Foo
        {
            public double x;
            public float y;
            public Bar b;
        }

        public Bar Func(Foo f, int i)
        {
            return new Bar
            {
                a = (int)(i + f.x),
                b = f.b.a * f.b.b + (int)f.y
            };
        }

        [Test]
        public void FuncTest()
        {
            var def = CecilLoader.DefinitionFor(this, "Func");
            Assert.AreEqual("Func", def.Name);

            var cfg = new ControlFlowGraph(def);

            var modbuilder = new ModuleBuilder();
            var fbuilder = modbuilder.CreateFunction(def);
            var mod = modbuilder.CreateModule();

            mod.SetBoundAutomatically();
            var vmod = mod.Validate();

            //DebugHelper.CreatePage(def, cfg, fbuilder.Frame, mod, vmod).WriteToTempAndOpen();
        }
    }
}
