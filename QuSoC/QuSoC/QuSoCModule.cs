using QRV32.CPU;
using Quokka.RTL;
using System;
using System.ComponentModel;
using System.Diagnostics;

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
        internal SoCBlockRAMModule InstructionsRAM = new SoCBlockRAMModule(1024);
        internal RISCVModule CPU = new RISCVModule();
        internal SoCRegisterModule CounterRegister = new SoCRegisterModule();
        internal SoCBlockRAMModule BlockRAM = new SoCBlockRAMModule(1024);
        internal SoCUARTSimModule UARTSim = new SoCUARTSimModule();

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
                MemReadData = internalMemReadData,
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
        uint internalMemReadData
        {
            get
            {
                uint result = 0;

                if (CounterRegister.IsActive)
                {
                    result = CounterRegister.ReadValue;
                }
                else if (BlockRAM.IsActive)
                {
                    result = BlockRAM.ReadValue;
                }
                else if (InstructionsRAM.IsActive)
                {
                    result = InstructionsRAM.ReadValue;
                }    
                else if (UARTSim.IsActive)
                {
                    result = UARTSim.ReadValue;
                }

                return result;
            }
        }
        
        bool internalMemReady => State.MemReady;

        protected override void OnStage()
        {
            NextState.MemReady = CPU.MemRead;

            // TODO: constants e.g. 32768U
            // TODO: State.BlockRAM.Length

            if (CPU.MemWrite)
            {
                if (CounterRegister.IsActive)
                {
                    NextState.MemReady = CounterRegister.IsReady;
                }
                else if (BlockRAM.IsActive)
                {
                    NextState.MemReady = BlockRAM.IsReady;
                }
                else if (InstructionsRAM.IsActive)
                {
                    NextState.MemReady = InstructionsRAM.IsReady;
                }
                else if (UARTSim.IsActive)
                {
                    NextState.MemReady = UARTSim.IsReady;
                }
                else
                {
                    Debugger.Break();
                    NextState.MemReady = true;
                }
            }
        }
    }
}
