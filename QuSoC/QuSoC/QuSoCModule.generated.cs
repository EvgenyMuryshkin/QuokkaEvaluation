namespace QuSoC
{
    public partial class QuSoCModule
    {
        internal SoCRegisterModule CounterRegister = new SoCRegisterModule();
        internal SoCBlockRAMModule BlockRAM = new SoCBlockRAMModule(1024);

        ISoCComponentModule[] GeneratedModules => new ISoCComponentModule[]
        {
            CounterRegister,
            BlockRAM
        };

        protected virtual void CreateGeneratedModules()
        {
            CounterRegister = new SoCRegisterModule();
            BlockRAM = new SoCBlockRAMModule(1024);
        }

        protected virtual void ScheduleGeneratedModules()
        {
            CounterRegister.Schedule(() => new SoCRegisterModuleInputs()
            {
                Common = ModuleCommon,
                DeviceAddress = 0x80000000,
            });

            BlockRAM.Schedule(() => new SoCBlockRAMModuleInputs()
            {
                Common = ModuleCommon,
                DeviceAddress = 0x80100000,
            });
        }
    }
}
