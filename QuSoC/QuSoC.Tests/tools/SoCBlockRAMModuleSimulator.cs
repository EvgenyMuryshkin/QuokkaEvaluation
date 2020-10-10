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
}
