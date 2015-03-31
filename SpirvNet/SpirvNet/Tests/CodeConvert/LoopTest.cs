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
    public class LoopTest
    {
        public int Loop(int k)
        {
            int res = 0;
            for (var i = 0; i < k; ++i)
            {
                res += k * i;
                for (var j = i + 1; j < k; ++j)
                {
                    res += j;
                    if (res % 7 == 0)
                        break;
                    if (res % 9 == 0)
                        continue;
                    res += 1;
                }

                if (res % 11 == 0)
                    break;

                if (res % 13 == 0)
                    res += k * (res % 3);
                else
                    while (res % 7 != 0)
                        res -= 3;
            }

            return res;
        }

        public int Loop0(int k)
        {
            int res = 0;
            for (var i = 0; i < k; ++i)
            {
                res += k * i;

                while (res % 7 != 0)
                    res -= 3;
            }

            return res;
        }

        public float SimplestFor(int k)
        {
            float res = 0;

            for (var i = 0; i < k; ++i)
            {
                res += 1;
            }

            return res;
        }

        [Test]
        public void ComplexLoopTest()
        {
            var def = CecilLoader.DefinitionFor(this, "Loop");
            Assert.AreEqual("Loop", def.Name);

            var cfg = new ControlFlowGraph(def);

            var modbuilder = new ModuleBuilder();
            var fbuilder = modbuilder.CreateFunction(def);
            var mod = modbuilder.CreateModule();

            mod.SetBoundAutomatically();
            var vmod = mod.Validate();

            //DebugHelper.CreatePage(def, cfg, fbuilder.Frame, mod, vmod).WriteToTempAndOpen();
        }

        [Test]
        public void Loop0Test()
        {
            var def = CecilLoader.DefinitionFor(this, "Loop0");
            Assert.AreEqual("Loop0", def.Name);

            var cfg = new ControlFlowGraph(def);

            var modbuilder = new ModuleBuilder();
            var fbuilder = modbuilder.CreateFunction(def);
            var mod = modbuilder.CreateModule();

            mod.SetBoundAutomatically();
            var vmod = mod.Validate();

            //DebugHelper.CreatePage(def, cfg, fbuilder.Frame, mod, vmod).WriteToTempAndOpen();
        }

        [Test]
        public void SimplestForTest()
        {
            var def = CecilLoader.DefinitionFor(this, "SimplestFor");
            Assert.AreEqual("SimplestFor", def.Name);

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
