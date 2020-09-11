using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.RTL.Simulator;

namespace QuSoC.Tests
{
    class SoCBlockRAMModuleSimulator : RTLInstanceSimulator<SoCBlockRAMModule, SoCBlockRAMModuleInputs>
    {
        public SoCBlockRAMModuleSimulator() : base(new SoCBlockRAMModule(1024))
        {

        }

        public void U32Write(uint addr, uint value)
        {
            ClockCycle(new SoCBlockRAMModuleInputs()
            {
                Common = new SoCComponentModuleCommon()
                {
                    Address = addr,
                    RE = false,
                    WE = true,
                    WriteValue = value,
                },
                DeviceAddress = 0,
                MemAccessMode = 2
            });
        }

        public uint U32Read(uint addr)
        {
            ClockCycle(new SoCBlockRAMModuleInputs()
            {
                Common = new SoCComponentModuleCommon()
                {
                    Address = addr,
                    RE = true,
                    WE = false,
                    WriteValue = 0,
                },
                DeviceAddress = 0,
                MemAccessMode = 2
            });

            return TopLevel.ReadValue;
        }

        public void U16Write(uint addr, uint value)
        {
            ClockCycle(new SoCBlockRAMModuleInputs()
            {
                Common = new SoCComponentModuleCommon()
                {
                    Address = addr,
                    RE = false,
                    WE = true,
                    WriteValue = value,
                },
                DeviceAddress = 0,
                MemAccessMode = 1
            });
        }

        public uint U16Read(uint addr)
        {
            ClockCycle(new SoCBlockRAMModuleInputs()
            {
                Common = new SoCComponentModuleCommon()
                {
                    Address = addr,
                    RE = true,
                    WE = false,
                    WriteValue = 0,
                },
                DeviceAddress = 0,
                MemAccessMode = 1
            });

            return TopLevel.ReadValue;
        }

        public void U8Write(uint addr, uint value)
        {
            ClockCycle(new SoCBlockRAMModuleInputs()
            {
                Common = new SoCComponentModuleCommon()
                {
                    Address = addr,
                    RE = false,
                    WE = true,
                    WriteValue = value,
                },
                DeviceAddress = 0,
                MemAccessMode = 0
            });
        }

        public uint U8Read(uint addr)
        {
            ClockCycle(new SoCBlockRAMModuleInputs()
            {
                Common = new SoCComponentModuleCommon()
                {
                    Address = addr,
                    RE = true,
                    WE = false,
                    WriteValue = 0,
                },
                DeviceAddress = 0,
                MemAccessMode = 0
            });

            return TopLevel.ReadValue;
        }

        public void U8WriteComlete(uint addr, uint value)
        {
            U8Write(addr, value);
            U8Write(addr, value);
        }

        public void U16WriteComlete(uint addr, uint value)
        {
            U16Write(addr, value);
            U16Write(addr, value);
        }

        public void U32WriteComlete(uint addr, uint value)
        {
            U32Write(addr, value);
        }
    }

    [TestClass]
    public class SoCBlockRAMModuleTests
    {
        SoCBlockRAMModuleSimulator Sim()
        {
            return new SoCBlockRAMModuleSimulator();
        }

        [TestMethod]
        public void NotActiveTest()
        {
            var sim = Sim();
            // nothing is requested from module
            sim.ClockCycle(new SoCBlockRAMModuleInputs()
            {
                Common = new SoCComponentModuleCommon()
                {
                    Address = 0,
                    RE = false,
                    WE = false,
                    WriteValue = 0,
                },
                DeviceAddress = 0
            });
            Assert.IsFalse(sim.TopLevel.IsActive);

            // read address is outside of memory span
            sim.ClockCycle(new SoCBlockRAMModuleInputs()
            {
                Common = new SoCComponentModuleCommon()
                {
                    Address = 0,
                    RE = true,
                    WE = false,
                    WriteValue = 0,
                },
                DeviceAddress = 0x00010000
            });
            Assert.IsFalse(sim.TopLevel.IsActive);

            // read address is outside of memory span
            sim.ClockCycle(new SoCBlockRAMModuleInputs()
            {
                Common = new SoCComponentModuleCommon()
                {
                    Address = 0,
                    RE = false,
                    WE = true,
                    WriteValue = 0,
                },
                DeviceAddress = 0x00010000
            });
            Assert.IsFalse(sim.TopLevel.IsActive);
        }

        [TestMethod]
        public void U16WriteReadTest()
        {
            var sim = Sim();

            // write halves, two-cycle operations
            // low half
            sim.U16Write(0x14, 0x8001);
            Assert.IsTrue(sim.TopLevel.IsActive);
            Assert.IsFalse(sim.TopLevel.IsReady);

            // second cycle should write back
            sim.U16Write(0x14, 0x8001);
            Assert.IsTrue(sim.TopLevel.IsActive);
            Assert.IsTrue(sim.TopLevel.IsReady);
            Assert.AreEqual(0x8001U, sim.TopLevel.State.BlockRAM[5]);

            // high half
            sim.U16Write(0x16, 0xB003);
            Assert.IsTrue(sim.TopLevel.IsActive);
            Assert.IsFalse(sim.TopLevel.IsReady);

            sim.U16Write(0x16, 0xB003);
            Assert.IsTrue(sim.TopLevel.IsActive);
            Assert.IsTrue(sim.TopLevel.IsReady);
            Assert.AreEqual(0xB0038001U, sim.TopLevel.State.BlockRAM[5]);

            sim.U16Read(0x14);
            Assert.AreEqual(0x8001U, sim.TopLevel.ReadValue);

            sim.U16Read(0x16);
            Assert.AreEqual(0xB003U, sim.TopLevel.ReadValue);

            sim.U32Read(0x14);
            Assert.AreEqual(0xB0038001U, sim.TopLevel.ReadValue);
        }

        [TestMethod]
        public void U32WriteReadTest()
        {
            var sim = Sim();

            // write word
            sim.U32Write(0x10, 0x80000042);
            Assert.IsTrue(sim.TopLevel.IsActive);
            Assert.IsTrue(sim.TopLevel.IsReady);
            Assert.AreEqual(0x80000042, sim.TopLevel.State.BlockRAM[4]);

            // read from block
            sim.U32Read(0x10);
            Assert.IsTrue(sim.TopLevel.IsActive);
            Assert.IsTrue(sim.TopLevel.IsReady);
            Assert.AreEqual(0x80000042, sim.TopLevel.ReadValue);
        }

        [TestMethod]
        public void U8WriteReadTest()
        {
            var sim = Sim();
            sim.U8WriteComlete(0x10, 0x81);
            sim.U8WriteComlete(0x11, 0x93);
            sim.U8WriteComlete(0x12, 0xa5);
            sim.U8WriteComlete(0x13, 0xb7);

            Assert.AreEqual(0xb7a59381, sim.TopLevel.State.BlockRAM[4]);
            sim.U32Read(0x10);
            Assert.AreEqual(0xb7a59381, sim.TopLevel.ReadValue);
            Assert.AreEqual(0x81U, sim.U8Read(0x10));
            Assert.AreEqual(0x93U, sim.U8Read(0x11));
            Assert.AreEqual(0xa5U, sim.U8Read(0x12));
            Assert.AreEqual(0xb7U, sim.U8Read(0x13));
        }
    }
}
