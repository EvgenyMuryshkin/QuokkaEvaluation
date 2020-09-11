﻿using QRV32.CPU;
using Quokka.RTL;
using System;
using System.Diagnostics;

namespace QuSoC
{
    public class QuSoCModuleInputs
    {
    }

    public class QuSoCModuleState
    {
        public uint[] BlockRAM = new uint[1024];
        public bool BlockRAMWE;

        public RTLBitArray MemReadData = 0U;
        public bool MemReady;

        public byte[] UART = new byte[4];
        public bool UART_TX;
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
        internal SoCRegisterModule CounterRegister = new SoCRegisterModule();
        internal SoCBlockRAMModule BlockRAM = new SoCBlockRAMModule(1024);

        public uint Counter => CounterRegister.ReadValue;
        public RTLBitArray CPUAddress => CPU.MemAddress;
        public bool CPUMemRead => CPU.MemRead;
        public bool CPUMemWrite => CPU.MemWrite;
        public RTLBitArray CPUMemReadData => internalMemReadData;
        public bool SOCMemReady => State.MemReady;
        public bool CPUHalted => CPU.IsHalted;
        public bool BlockRAMWE => State.BlockRAMWE;

        public QuSoCModule(uint[] instructions)
        {
            instructions.CopyTo(State.BlockRAM, 0);
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
        }

        RTLBitArray internalMemAddress => new RTLBitArray(CPU.MemAddress);
        RTLBitArray wordAddress => internalMemAddress >> 2;
        RTLBitArray byteAddress => new RTLBitArray(internalMemAddress[1, 0]) << 3;

        RTLBitArray uartReadData => new RTLBitArray(State.UART[uartAddress]).Resized(32);

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
                else
                {
                    switch ((uint)memSegment)
                    {
                        case 0:
                            result = State.MemReadData >> byteAddress;
                            break;
                        case 2:
                            result = uartReadData;
                            break;
                    }
                }

                return result;
            }
        }
        
        bool internalMemReady => State.MemReady;

        RTLBitArray mask =>
            CPU.MemAccessMode == 0
            ? (new RTLBitArray(byte.MaxValue) << byteAddress).Resized(32)
            : CPU.MemAccessMode == 1
                ? (new RTLBitArray(ushort.MaxValue) << byteAddress).Resized(32)
                : new RTLBitArray(uint.MaxValue);

        RTLBitArray blockRAMWriteData =>
            (State.MemReadData & !mask) | ((CPU.MemWriteData << byteAddress) & mask);

        RTLBitArray memSegment => wordAddress[31, 10];
        RTLBitArray blockRamAddress => wordAddress[9, 0];

        RTLBitArray uartAddress => internalMemAddress[1, 0];

        public byte UARTWriteData => State.UART[0];

        bool UARTReady => State.UART[2] != 0;

        protected override void OnStage()
        {
            if (State.BlockRAMWE)
            {
                NextState.BlockRAM[blockRamAddress] = blockRAMWriteData;
            }

            NextState.MemReadData = State.BlockRAM[blockRamAddress];
            NextState.MemReady = CPU.MemRead;

            // TODO: constants e.g. 32768U
            // TODO: State.BlockRAM.Length

            NextState.BlockRAMWE = false;
            NextState.UART_TX = false;

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
                else
                {
                    switch ((uint)memSegment)
                    {
                        case 0:
                            if (!State.BlockRAMWE)
                            {
                                // write back to block ram on next cycle
                                NextState.BlockRAMWE = true;
                                NextState.MemReady = true;
                            }
                            break;
                        case 2:
                            // TODO: inline element access
                            if (UARTReady)
                            {
                                // TODO: implicit cast is not handled in rtl transform
                                NextState.UART[0] = (byte)CPU.MemWriteData;
                                NextState.UART[2] = 0;
                                NextState.UART_TX = true;
                                NextState.MemReady = true;
                            }
                            break;
                        default:
                            Debugger.Break();
                            NextState.MemReady = true;
                            break;
                    }
                }
            }
        }
    }
}
