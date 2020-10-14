namespace QuSoC
{
    public class IntegrationTestModule : QuSoCModule
    {
        internal SoCUARTSimModule UARTSim = new SoCUARTSimModule();
        internal SoCRegisterModule CounterModule = new SoCRegisterModule();
        protected override ISoCComponentModule[] ManualModules => new ISoCComponentModule[] { CounterModule, UARTSim };
        
        public uint Counter => CounterModule.ReadValue;

        public IntegrationTestModule(uint[] instructions) : base(instructions)
        {

        }

        protected override void ScheduleManualModules()
        {
            CounterModule.Schedule(() => new SoCRegisterModuleInputs() 
            { 
                Common = ModuleCommon, 
                DeviceAddress = 0x80000000
            });

            UARTSim.Schedule(() => new SoCUARTSimModuleInputs()
            {
                Common = ModuleCommon,
                DeviceAddress = 0x90000000,
            });

        } // ScheduleGeneratedModules
    }
}
