using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SpirvNet.DotNet;

namespace SpirvNet.Tests
{
    [TestFixture]
    public class CecilTest
    {
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
