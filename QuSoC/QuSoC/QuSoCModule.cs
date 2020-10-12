using QRV32.CPU;
using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace QuSoC
{
    public class QuSoCModuleInputs
    {
    }

    public class QuSoCModuleState
    {
        public bool MemReady;
    }

    public partial class QuSoCModule : RTLSynchronousModule<QuSoCModuleInputs, QuSoCModuleState>
    {
        internal RISCVModule CPU = new RISCVModule();
        internal SoCBlockRAMModule InstructionsRAM = new SoCBlockRAMModule(1024);
        internal SoCUARTSimModule UARTSim = new SoCUARTSimModule();

        protected virtual ISoCComponentModule[] QuSoCModules => new ISoCComponentModule[]
        {
            InstructionsRAM
        };

        protected virtual ISoCComponentModule[] ManualModules => new ISoCComponentModule[]
        {
            UARTSim
        };

        protected virtual ISoCComponentModule[] AllModules => new []
        {
            QuSoCModules,
            ManualModules,
            GeneratedModules
        }
        .SelectMany(m => m)
        .ToArray();

        // NOTE: reverse is needed because RTLBitArray constructor is MSB ordered
        // Please get in touch if you are interested in rationale (dirty hacks) behind this.
        RTLBitArray CombinedModuleIsActive => new RTLBitArray(AllModules.Select(g => g.IsActive)).Reversed();
        RTLBitArray internalMemAccessMode => CPU.MemAccessMode[1, 0];
        public uint Counter => CounterRegister.ReadValue;
         
        protected QuSoCModule()
        {
            FromInstructions(FirmwareTools.FromApp(GetType().Name));
        }
        
        public QuSoCModule(uint[] instructions)
        {
            FromInstructions(instructions);
        }

        void FromInstructions(uint[] instructions)
        {
            instructions.CopyTo(InstructionsRAM.State.BlockRAM, 0);
            CreateGeneratedModules();
        }

        SoCComponentModuleCommon ModuleCommon => new SoCComponentModuleCommon()
        {
            WriteValue = CPU.MemWriteData,
            WE = CPU.MemWrite,
            Address = CPU.MemAddress,
            RE = CPU.MemRead,
            MemAccessMode = internalMemAccessMode
        };

        void ScheduleQuSoCModules()
        {
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
            });
        }

        protected virtual void ScheduleManualModules()
        {
            UARTSim.Schedule(() => new SoCUARTSimModuleInputs()
            {
                Common = ModuleCommon,
                DeviceAddress = 0x90000000,
            });
        }

        protected override void OnSchedule(Func<QuSoCModuleInputs> inputsFactory)
        {
            base.OnSchedule(inputsFactory);

            ScheduleQuSoCModules();
            ScheduleManualModules();
            ScheduleGeneratedModules();
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
            // assumption for now is that module is immediately ready
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
