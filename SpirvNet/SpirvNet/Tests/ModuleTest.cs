using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SpirvNet.Spirv;
using SpirvNet.Spirv.Enums;
using SpirvNet.Spirv.Ops.ModeSetting;

namespace SpirvNet.Tests
{
    [TestFixture]
    public class ModuleTest
    {
        [Test]
        public void EmptyModule()
        {
            // create empty mod and code
            var mod = new Module();
            var code = mod.GenerateBytecode();
            Assert.AreEqual(5, code.Count);

            // create second mod from code
            var mod2 = Module.FromCode(code.ToArray());
            var code2 = mod2.GenerateBytecode();
            Assert.AreEqual(code.Count, code2.Count);

            // verify per-word
            for (var i = 0; i < code.Count; ++i)
                Assert.AreEqual(code[i], code2[i]);
        }

        [Test]
        public void EmptyModuleStream()
        {
            var stream = new MemoryStream();

            var mod = new Module();
            mod.WriteToStream(stream);
            var code = mod.GenerateBytecode();
            Assert.AreEqual(5 * sizeof(uint), stream.Length);

            // reset stream
            stream.Seek(0, SeekOrigin.Begin);

            var mod2 = Module.FromStream(stream);
            var code2 = mod2.GenerateBytecode();
            Assert.AreEqual(code.Count, code2.Count);

            for (var i = 0; i < code.Count; ++i)
                Assert.AreEqual(code[i], code2[i]);
        }

        [Test]
        public void RandomModuleStream()
        {
            var ops = Instruction.GenerateDummyInstructions();
            for (var _ = 0; _ < 1000; ++_)
            {
                var random = new Random(_ + 1);
                var stream = new MemoryStream();

                var mod = new Module();

                // gen mod
                var ic = random.Next(0, 100);
                var wc = 5;
                for (var i = 0; i < ic; ++i)
                {
                    var op = ops[(OpCode)random.Next((int)OpCode.Unknown)];
                    op.FillWithRandomValues(random);
                    var codes = op.Generate();
                    op = Instruction.Read(codes);
                    mod.Instructions.Add(op);
                    wc += codes.Length;
                }

                mod.WriteToStream(stream);
                var code = mod.GenerateBytecode();
                Assert.AreEqual(wc * sizeof(uint), stream.Length);

                // reset stream
                stream.Seek(0, SeekOrigin.Begin);

                var mod2 = Module.FromStream(stream);
                var code2 = mod2.GenerateBytecode();
                Assert.AreEqual(mod.Instructions.Count, mod2.Instructions.Count);
                Assert.AreEqual(code.Count, code2.Count);

                for (var i = 0; i < code.Count; ++i)
                    Assert.AreEqual(code[i], code2[i]);
            }
        }

        [Test]
        public void RandomSingleInstructionModuleStream()
        {
            var ops = Instruction.GenerateDummyInstructions();
            for (var _ = 0; _ < 10000; ++_)
            {
                var random = new Random(_ + 1);
                var stream = new MemoryStream();

                var mod = new Module();

                // gen mod
                var wc = 5;
                {
                    var op = ops[(OpCode)random.Next((int)OpCode.Unknown)];
                    op.FillWithRandomValues(random);
                    var codes = op.Generate();
                    op = Instruction.Read(codes);
                    mod.Instructions.Add(op);
                    wc += codes.Length;
                }

                mod.WriteToStream(stream);
                var code = mod.GenerateBytecode();
                Assert.AreEqual(wc * sizeof(uint), stream.Length);

                // reset stream
                stream.Seek(0, SeekOrigin.Begin);

                var mod2 = Module.FromStream(stream);
                var code2 = mod2.GenerateBytecode();
                Assert.AreEqual(mod.Instructions.Count, mod2.Instructions.Count);
                Assert.AreEqual(code.Count, code2.Count);

                for (var i = 0; i < code.Count; ++i)
                    Assert.AreEqual(code[i], code2[i]);
            }
        }

        [Test]
        public void CompilerFlagMod()
        {
            var mod = new Module();
            var op = new OpCompileFlag { Flag = { Value = "123456789" } };
            mod.Instructions.Add(op);

            var code = mod.GenerateBytecode();
            var mod2 = Module.FromCode(code.ToArray());

            var code2 = mod2.GenerateBytecode();

            Assert.AreEqual(code.Count, code2.Count);
            for (var i = 0; i < code.Count; ++i)
                Assert.AreEqual(code[i], code2[i]);
        }
    }
}
