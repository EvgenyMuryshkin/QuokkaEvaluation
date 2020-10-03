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

    public partial class QuSoCModule : RTLSynchronousModule<QuSoCModuleInputs, QuSoCModuleState>
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

        // NOTE: reverse is needed because RTLBitArray constructor is MSB ordered
        // Please get in touch if you are interested in rationale (dirty hacks) behind this.
        RTLBitArray CombinedModuleIsActive => new RTLBitArray(AllModules.Select(g => g.IsActive)).Reversed();
        RTLBitArray internalMemAccessMode => CPU.MemAccessMode[1, 0];
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
                MemAccessMode = internalMemAccessMode
            });

            OnScheduleGenerated();
        }

        //RTLBitArray IsActive
        // TODO: RTLBitArray variable declaration, support for bit array methods e.g. Resize
        (byte, bool) BusCS
        {
            get
            {
                bool hasActive = false;
                byte address = 0;

                for (byte idx = 0; idx < CombinedModuleIsActive.Size; idx++)
                {
                    hasActive = hasActive | CombinedModuleIsActive[idx];

                    if (CombinedModuleIsActive[idx])
                        address = idx;
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
