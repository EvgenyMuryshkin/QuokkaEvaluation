using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Text;
using TestTools;

namespace FPGA.Trigonometry.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        [TestMethod]
        public void Normalize()
        {
            using (var port = new QuokkaPort())
            {
                var rnd = new Random(Environment.TickCount);

                foreach (var idx in Enumerable.Range(0, 10000))
                {
                    var value = (float)(rnd.NextDouble() * idx);
                    port.WriteFloat(value);

                    var expected = FPGATrigonometryTools.Normalize(value);
                    var actualBytes = port.Read(4, true, port.DefaultTimeout);
                    var actual = TestConverters.FloatFromByteArray(actualBytes);

                    Assert.AreEqual(expected, actual, $"Failed for {value}");
                }
            }
        }

        [TestMethod]
        public void Project()
        {
            using (var port = new QuokkaPort())
            {
                var rnd = new Random(Environment.TickCount);

                foreach (var idx in Enumerable.Range(0, 10000))
                {
                    var value = (float)(rnd.NextDouble() * FPGATrigonometryConstants.TwoPI);
                    port.WriteFloat(value);

                    var expected = FPGATrigonometryTools.Q1Project(value);
                    var actualBytes = port.Read(4, true, port.DefaultTimeout);
                    var actual = TestConverters.FloatFromByteArray(actualBytes);

                    Assert.AreEqual(expected, actual, $"Failed for {value}");
                }
            }
        }

        [TestMethod]
        public void Pow()
        {
            using (var port = new QuokkaPort())
            {
                var rnd = new Random(Environment.TickCount);

                foreach (var idx in Enumerable.Range(0, 10000))
                {
                    var value = (float)(rnd.NextDouble());
                    port.WriteFloat(value);

                    var expected = FPGATrigonometryTools.Pow(value, 5);
                    var actualBytes = port.Read(4, true, port.DefaultTimeout);
                    var actual = TestConverters.FloatFromByteArray(actualBytes);

                    Assert.AreEqual(expected, actual, $"Failed for {value}");
                }
            }
        }

        [TestMethod]
        public void TaylorSin()
        {
            using (var port = new QuokkaPort())
            {
                var rnd = new Random(Environment.TickCount);

                foreach (var idx in Enumerable.Range(0, 10000))
                {
                    var value = (float)(rnd.NextDouble() * FPGATrigonometryConstants.HalfPI);
                    port.WriteFloat(value);

                    var expected = FPGATrigonometryTools.TaylorSin(value);
                    var actualBytes = port.Read(4, true, port.DefaultTimeout);
                    var actual = TestConverters.FloatFromByteArray(actualBytes);

                    Assert.AreEqual(expected, actual, $"Failed for {value}");
                }
            }
        }

        [TestMethod]
        public void TaylorCos()
        {
            using (var port = new QuokkaPort())
            {
                var rnd = new Random(Environment.TickCount);

                foreach (var idx in Enumerable.Range(0, 10000))
                {
                    var value = (float)(rnd.NextDouble() * FPGATrigonometryConstants.HalfPI);
                    port.WriteFloat(value);

                    var expected = FPGATrigonometryTools.TaylorCos(value);
                    var actualBytes = port.Read(4, true, port.DefaultTimeout);
                    var actual = TestConverters.FloatFromByteArray(actualBytes);

                    Assert.AreEqual(expected, actual, $"Failed for {value}");
                }
            }
        }

        [TestMethod]
        public void Sin()
        {
            using (var port = new QuokkaPort())
            {
                var rnd = new Random(Environment.TickCount);

                foreach (var idx in Enumerable.Range(-5000, 10000))
                {
                    var value = (float)(idx * rnd.NextDouble() * FPGATrigonometryConstants.TwoPI);
                    port.WriteFloat(value);

                    var expected = FPGATrigonometry.Sin(value);
                    var actualBytes = port.Read(4, true, port.DefaultTimeout);
                    var actual = TestConverters.FloatFromByteArray(actualBytes);

                    Assert.AreEqual(expected, actual, $"Failed for {value}");
                }
            }
        }


        [TestMethod]
        public void Cos()
        {
            using (var port = new QuokkaPort())
            {
                var rnd = new Random(Environment.TickCount);

                foreach (var idx in Enumerable.Range(-5000, 10000))
                {
                    var value = (float)(idx * rnd.NextDouble() * FPGATrigonometryConstants.TwoPI);
                    port.WriteFloat(value);

                    var expected = FPGATrigonometry.Cos(value);
                    var actualBytes = port.Read(4, true, port.DefaultTimeout);
                    var actual = TestConverters.FloatFromByteArray(actualBytes);

                    Assert.AreEqual(expected, actual, $"Failed for {value}");
                }
            }
        }
    }
}
