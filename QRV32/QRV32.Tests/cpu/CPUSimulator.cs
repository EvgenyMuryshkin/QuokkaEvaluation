using QRV32.CPU;
using Quokka.RTL;
using Quokka.RTL.Simulator;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace QRV32.Tests
{
    public class CPUSimulator : RTLSimulator<CPUModule, CPUModuleInputs>
    {
        public int DebuggerCalls = 0;
        public List<uint> ECalls = new List<uint>();

        public uint[] MemoryBlock = new uint[32768];
        int instructionsCount = 0;
        uint lastRequestedAddress = uint.MaxValue;

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

        public void RunAllInstructions()
        {
            while (Step()) ;
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

        bool FeedNextInstruction()
        {
            var address = (TopLevel.MemAddress >> 2);
            if (address >= MemoryBlock.Length)
                throw new IndexOutOfRangeException($"Requested address in IF was outside of memory block: {address}");

            // end state condition
            if (address >= instructionsCount)
                return false;

            if (lastRequestedAddress == address)
                return false;

            lastRequestedAddress = address;
            ClockCycle(new CPUModuleInputs() { MemReady = true, MemReadData = MemoryBlock[address] });

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
                        if (TopLevel.ID.ITypeImm[0])
                        {
                            // ebreak
                            Debugger.Break();
                            DebuggerCalls++;
                        }
                        else
                        {
                            // ecall, do something with ecall
                            ECalls.Add(TopLevel.Regs.State.x[17]);
                        }
                        ClockCycle();
                        break;
                    case CPUState.MEM:
                        var wordAddress = TopLevel.MemAddress & 0xFFFFFFFC;
                        if (wordAddress >= MemoryBlock.Length)
                            throw new IndexOutOfRangeException($"Requested address in IF was outside of memory block: {wordAddress}");

                        if (TopLevel.MemRead)
                        {
                            var byteAddress = TopLevel.MemAddress & 0x3;
                            var word = new RTLBitArray(MemoryBlock[wordAddress]);
                            var data = word >> (int)(byteAddress * 8);
                            ClockCycle(new CPUModuleInputs() { MemReady = true, MemReadData = data });
                        }
                        else if (TopLevel.MemWrite)
                        {
                            var byteAddress = (int)((TopLevel.MemAddress & 0x3) << 3);
                            var word = new RTLBitArray(MemoryBlock[wordAddress]);
                            var mask = new RTLBitArray(uint.MinValue);

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
                            var part = TopLevel.MemWriteData & mask;
                            word |= part;

                            // write data back to mem
                            MemoryBlock[wordAddress] = word;

                            ClockCycle(new CPUModuleInputs() { MemReady = true });
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
    }
}
