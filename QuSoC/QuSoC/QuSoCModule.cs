using QRV32.CPU;
using Quokka.RTL;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace QuSoC
{
    public class QuSoCModuleInputs
    {
    }

    public class QuSoCModuleState
    {
        public bool MemReady;
    }

    // TODO: inheritance not supportted yet
    /*
    public class QuSoCBlinkerModule : QuSoCModule
    {
        public QuSoCBlinkerModule()
        {
        }

        protected override void OnSchedule(Func<QuSoCModuleInputs> inputsFactory)
        {
            base.OnSchedule(inputsFactory);
        }
    }
    */

    public class QuSoCModule : RTLSynchronousModule<QuSoCModuleInputs, QuSoCModuleState>
    {
        internal RISCVModule CPU = new RISCVModule();
        internal SoCBlockRAMModule InstructionsRAM = new SoCBlockRAMModule(1024);
        internal SoCRegisterModule CounterRegister = new SoCRegisterModule();
        internal SoCBlockRAMModule BlockRAM = new SoCBlockRAMModule(1024);
        internal SoCUARTSimModule UARTSim = new SoCUARTSimModule();

        ISoCComponentModule[] AllModules => new ISoCComponentModule[]
        {
            InstructionsRAM,
            CounterRegister,
            BlockRAM,
            UARTSim
        };

        public uint Counter => CounterRegister.ReadValue;

        public QuSoCModule(uint[] instructions)
        {
            instructions.CopyTo(InstructionsRAM.State.BlockRAM, 0);
        }

        SoCComponentModuleCommon ModuleCommon => new SoCComponentModuleCommon()
        {
            WriteValue = CPU.MemWriteData,
            WE = CPU.MemWrite,
            Address = CPU.MemAddress,
            RE = CPU.MemRead
        };

        protected override void OnSchedule(Func<QuSoCModuleInputs> inputsFactory)
        {
            base.OnSchedule(inputsFactory);

            CPU.Schedule(() => new RISCVModuleInputs()
            {
                BaseAddress = 0U,
                MemReadData = internalModuleReadData,
                MemReady = internalMemReady
            });

            InstructionsRAM.Schedule(() => new SoCBlockRAMModuleInputs()
            {
                Common = ModuleCommon,
                DeviceAddress = 0x00000000,
                MemAccessMode = CPU.MemAccessMode[1, 0]
            });

            CounterRegister.Schedule(() => new SoCRegisterModuleInputs()
            {
                Common = ModuleCommon,
                DeviceAddress = 0x80000000,
            });

            BlockRAM.Schedule(() => new SoCBlockRAMModuleInputs()
            {
                Common = ModuleCommon,
                DeviceAddress = 0x80100000,
                MemAccessMode = CPU.MemAccessMode[1,0]
            });

            UARTSim.Schedule(() => new SoCUARTSimModuleInputs()
            {
                Common = ModuleCommon,
                DeviceAddress = 0x80200000,
            });
        }

        // TODO: RTLBitArray variable declaration, support for bit array methods e.g. Resize
        (byte, bool) BusCS
        {
            get
            {
                bool hasActive = true;
                byte address = 0;

                // TODO: prioritized encored
                if (InstructionsRAM.IsActive)
                {
                    address = 0;
                }
                else if (CounterRegister.IsActive)
                {
                    address = 1;
                }
                else if (BlockRAM.IsActive)
                {
                    address = 2;
                }
                else if (UARTSim.IsActive)
                {
                    address = 3;
                }
                else
                {
                    hasActive = false;
                }

                return (address, hasActive);
            }
        }

        byte ModuleIndex => BusCS.Item1;
        bool HasActiveModule => BusCS.Item2;

        uint internalModuleReadData => AllModules[ModuleIndex].ReadValue;
        bool internalModuleIsReady => AllModules[ModuleIndex].IsReady;

        bool internalMemReady => State.MemReady;

        protected override void OnStage()
        {
            NextState.MemReady = CPU.MemRead;

            // TODO: constants e.g. 32768U
            // TODO: State.BlockRAM.Length

            if (CPU.MemWrite)
            {
                if (HasActiveModule)
                {
                    NextState.MemReady = internalModuleIsReady;
                }
                else
                {
                    // TODO: Halt state or something
                    Debugger.Break();
                    NextState.MemReady = true;
                }
            }
        }
    }
}
