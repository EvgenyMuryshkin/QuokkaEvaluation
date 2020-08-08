using QRV32.CPU;
using Quokka.RTL;
using Quokka.RTL.Simulator;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace QRV32.Tests
{
    public class CPUSimulator : RTLSimulator<RISCVModule, RISCVModuleInputs>
    {
        public int DebuggerCalls = 0;
        public List<uint> ECalls = new List<uint>();

        public uint[] MemoryBlock = new uint[32768];
        int instructionsCount = 0;

        public Action<RISCVModuleInputs, RTLSimulatorCallback<RISCVModule>> inputsModifier = null;

        public CPUSimulator()
        {

        }

        public void SetInstructions(uint[] instructions)
        {
            for (instructionsCount = 0; instructionsCount < instructions.Length; instructionsCount++)
            {
                MemoryBlock[instructionsCount] = instructions[instructionsCount];
            }
        }

        public void RunAll(params uint[] instructions)
        {
            SetInstructions(instructions);
            RunAllInstructions();
        }

        public void RunAllInstructions(uint maxInstructions = 1000)
        {
            while (--maxInstructions != 0)
            {
                if (!Step())
                    return;
            }

            throw new Exception("CPU seems to hang");
        }

        public bool Step()
        {
            if (!RunTillInstructionFetch())
                return false;

            if (!FeedNextInstruction())
                return false;

            if (!RunTillInstructionFetch())
                return false;

            return true;
        }

        RTLBitArray wordAddress => TopLevel.MemAddress >> 2;
        byte byteAddress => (byte)((TopLevel.MemAddress & 0x3) << 3);

        protected virtual void TraceLine(string value)
        {
            if (Debugger.IsAttached)
            {
                Trace.WriteLine(value);
            }
        }

        bool FeedNextInstruction()
        {
            if (wordAddress >= MemoryBlock.Length)
                throw new IndexOutOfRangeException($"Requested address in IF was outside of memory block: {wordAddress}");

            // end state condition
            if (wordAddress >= instructionsCount)
            {
                TraceLine($"Simulation finished, instruction read outside of program memory [{TopLevel.MemAddress.ToString("X6")}]");
                return false;
            }

            var instruction = MemoryBlock[wordAddress];
            var id = new InstructionDecoderModule();
            id.Setup();
            id.Cycle(new InstructionDecoderInputs() { Instruction = instruction });

            TraceLine($"[{TopLevel.MemAddress.ToString("X6")}]: {id.OpTypeCode}");

            if (instruction == 111)
            {
                TraceLine($"Simulation finished, detected infinite loop at address [{TopLevel.MemAddress.ToString("X6")}]");
                return false;
            }

            ClockCycle(new RISCVModuleInputs() { MemReady = true, MemReadData = MemoryBlock[wordAddress] });

            return true;
        }

        public bool RunTillInstructionFetch()
        {
            int counter = 0;

            while (counter++ < 1000)
            {
                switch (TopLevel.State.State)
                {
                    case CPUState.Halt:
                        throw new Exception($"Halted");
                    case CPUState.E:
                        switch (TopLevel.ID.SysTypeCode)
                        {
                            case SysTypeCodes.CALL:
                            {
                                // ecall, do something with ecall
                                ECalls.Add(TopLevel.Regs.State.x[17]);
                            }
                            break;
                            case SysTypeCodes.BREAK:
                            {
                                // ebreak
                                Debugger.Break();
                                DebuggerCalls++;
                            }
                            break;
                        }
                        ClockCycle();
                        break;
                    case CPUState.MEM:
                        if (wordAddress >= MemoryBlock.Length)
                            throw new IndexOutOfRangeException($"Requested address in IF was outside of memory block: {wordAddress}");

                        if (TopLevel.MemRead)
                        {
                            var word = new RTLBitArray(MemoryBlock[wordAddress]);
                            var data = word >> byteAddress;
                            ClockCycle(new RISCVModuleInputs() { MemReady = true, MemReadData = data });
                        }
                        else if (TopLevel.MemWrite)
                        {
                            var word = new RTLBitArray(MemoryBlock[wordAddress]);
                            var mask = new RTLBitArray(uint.MinValue);
                            var value = (TopLevel.MemWriteData << byteAddress).Resized(32);

                            switch ((byte)TopLevel.MemWriteMode)
                            {
                                case 0:
                                    mask = (new RTLBitArray(byte.MaxValue) << byteAddress).Resized(32);
                                    break;
                                case 1:
                                    mask = (new RTLBitArray(ushort.MaxValue) << byteAddress).Resized(32);
                                    break;
                                case 2:
                                    mask = new RTLBitArray(uint.MaxValue);
                                    break;
                                default:
                                    throw new Exception($"Unsupported mem write mode: {TopLevel.MemWriteMode}");
                            }

                            word &= !mask;
                            var part = value & mask;
                            word |= part;

                            // write data back to mem
                            MemoryBlock[wordAddress] = word;

                            ClockCycle(new RISCVModuleInputs() { MemReady = true });
                        }
                        else
                        {
                            throw new Exception($"No operation in mem stage");
                        }
                        break;
                    case CPUState.IF:
                        return true;
                    default:
                        ClockCycle();
                        break;
                }
            }

            throw new Exception("CPU seems to hang");
        }

        public override void ClockCycle(RISCVModuleInputs inputs)
        {
            inputsModifier?.Invoke(inputs, CallbackData);
            base.ClockCycle(inputs);
        }
    }
}
