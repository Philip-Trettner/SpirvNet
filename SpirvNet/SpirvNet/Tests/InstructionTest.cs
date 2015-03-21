using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SpirvNet.Spirv;
using SpirvNet.Spirv.Enums;
using SpirvNet.Spirv.Ops;
using SpirvNet.Spirv.Ops.Debug;
using SpirvNet.Spirv.Ops.Misc;

namespace SpirvNet.Tests
{
    [TestFixture]
    public class InstructionTest
    {
        [Test]
        public void SimpleGenerate()
        {
            var op = new OpUndef
            {
                ResultType = new ID(10),
                Result = new ID(20)
            };
            var code = new List<uint>();
            op.Generate(code);

            Assert.AreEqual(3, op.WordCount);
            Assert.AreEqual(3, code.Count);
            Assert.AreEqual(code[0], (uint)OpCode.Undef + (3u << 16));
            Assert.AreEqual(code[1], 10u);
            Assert.AreEqual(code[2], 20u);
        }

        [Test]
        public void UnknownGenerate()
        {
            var op = new OpUnknown(OpCode.MemoryModel);
            op.Args.Add(9);
            op.Args.Add(8);
            op.Args.Add(7);
            op.Args.Add(6);
            var code = new List<uint>();
            op.Generate(code);

            Assert.AreEqual(5, op.WordCount);
            Assert.AreEqual(5, code.Count);
            Assert.AreEqual(code[0], (uint)OpCode.MemoryModel + (5u << 16));
            Assert.AreEqual(code[1], 9u);
            Assert.AreEqual(code[2], 8u);
            Assert.AreEqual(code[3], 7u);
            Assert.AreEqual(code[4], 6u);
        }

        [Test]
        public void StringGenerate()
        {
            var op = new OpSourceExtension();
            var code = new List<uint>();
            op.Generate(code);

            Assert.AreEqual(2, op.WordCount);
            Assert.AreEqual(2, code.Count);
            Assert.AreEqual(code[0], (uint)OpCode.SourceExtension + (2u << 16));
            Assert.AreEqual(code[1], 0u);

            op = new OpSourceExtension { Extension = { Value = "abc" } };
            op.Generate(code);
            Assert.AreEqual(2, op.WordCount);
            Assert.AreEqual(2 + 2, code.Count);

            op = new OpSourceExtension { Extension = { Value = "abcdef" } };
            op.Generate(code);
            Assert.AreEqual(3, op.WordCount);
            Assert.AreEqual(2 + 2 + 3, code.Count);
        }

        [Test]
        public void CoveredOpCodes()
        {
            var op2type = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(Instruction))).ToDictionary(type => type.Name);

            foreach (var value in Enum.GetValues(typeof(OpCode)))
                Assert.That(op2type.ContainsKey("Op" + value));
        }

        [Test]
        public void ZeroSerializeDeserialize()
        {
            var ops = Instruction.GenerateDummyInstructions();
            foreach (var op in ops.Values)
            {
                var code = new List<uint>();
                op.Generate(code);
                Assert.AreEqual(code.Count, op.WordCount);

                var op2 = Instruction.Read(code.ToArray());
                var code2 = new List<uint>();
                op2.Generate(code2);

                Assert.AreEqual(code.Count, code2.Count);
                for (var i = 0; i < code.Count; ++i)
                    Assert.AreEqual(code[i], code2[i]);
            }
        }

        [Test]
        public void RandomSerializeDeserialize()
        {
            var ops = Instruction.GenerateDummyInstructions();
            foreach (var op in ops.Values)
            {
                var random = new Random(op.GetType().GetHashCode());
                for (var _ = 0; _ < 20; ++_)
                {
                    op.FillWithRandomValues(random);

                    var code = new List<uint>();
                    op.Generate(code);
                    Assert.AreEqual(code.Count, op.WordCount);

                    var op2 = Instruction.Read(code.ToArray());
                    var code2 = new List<uint>();
                    op2.Generate(code2);

                    Assert.AreEqual(code.Count, code2.Count);
                    for (var i = 0; i < code.Count; ++i)
                        Assert.AreEqual(code[i], code2[i]);

                    //Console.WriteLine("{0}: {1}", op, code.Select(u => u.ToString("X8")).Aggregate((s1, s2) => s1 + " " + s2));
                }
            }
        }
    }
}
