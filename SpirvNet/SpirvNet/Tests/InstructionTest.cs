using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SpirvNet.Spirv;
using SpirvNet.Spirv.Enums;
using SpirvNet.Spirv.Ops;
using SpirvNet.Spirv.Ops.Misc;

namespace SpirvNet.Tests
{
    [TestFixture]
    public class InstructionTest
    {
        [Test]
        public void SimpleGenerate()
        {
            var op = new OpUndef()
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
    }
}
