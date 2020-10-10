using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;

namespace QuSoC.Tests
{
    [TestClass]
    public class QuSoCModulesTests : QuSoCModuleBaseTest
    {
        [TestMethod]
        public void RecursionTest()
        {
            var sim = PowerUp<Recursion.Recursion>();
            sim.RunToCompletion();

            Recursion.Firmware.EntryPoint();
            Assert.AreEqual(Recursion.SOC.Instance.Counter, sim.TopLevel.Counter);
        }

        [TestMethod]
        public void ArraysTest()
        {
            var sim = PowerUp<Arrays.Arrays>();
            sim.RunToCompletion();

            Arrays.Firmware.EntryPoint();
            Assert.AreEqual(Arrays.SOC.Instance.Counter, sim.TopLevel.Counter);
        }

        [TestMethod]
        public void MemBlockTest()
        {
            var sim = PowerUp<MemBlock.MemBlock>();
            sim.RunToCompletion();

            MemBlock.Firmware.EntryPoint();
            for (var idx = 0; idx < 10; idx++)
            {
                Assert.AreEqual(MemBlock.SOC.Instance.MemBlock[idx], sim.TopLevel.BlockRAM.State.BlockRAM[idx], $"Failed for {idx}");
            }
            Assert.AreEqual(MemBlock.SOC.Instance.MemBlock[1023], sim.TopLevel.BlockRAM.State.BlockRAM[1023]);
        }


        [TestMethod]
        public void FibonacciTest()
        {
            foreach (uint idx in Enumerable.Range(0, 7))
            {
                Fibonacci.SOC.Instance.Counter = idx;
                Fibonacci.Firmware.EntryPoint();

                var sim = PowerUp<Fibonacci.Fibonacci>();
                sim.TopLevel.CounterRegister.State.Value = idx;
                sim.RunToCompletion();

                Assert.AreEqual(Fibonacci.SOC.Instance.Counter, sim.TopLevel.Counter);
                Trace.WriteLine($"Fib({idx}) completed in {sim.ClockCycles} cycles");
            }
        }

        [TestMethod]
        public void CounterTest()
        {
            var sim = PowerUp<Counter.Counter>();
            sim.RunToCompletion(() => sim.TopLevel.Counter < 10);

            Assert.AreEqual(10U, sim.TopLevel.Counter);
        }
    }
}
