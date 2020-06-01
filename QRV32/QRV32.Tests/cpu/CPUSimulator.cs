using QRV32.CPU;
using Quokka.RTL;
using Quokka.RTL.Simulator;
using System;

namespace QRV32.Tests
{
    public class CPUSimulator : RTLSimulator<CPUModule, CPUModuleInputs>
    {
        public uint[] MemoryBlock = new uint[32768];

        public CPUSimulator()
        {

        }

        public void RunAll(uint[] instructions)
        {
            foreach (var inst in instructions)
            {
                RunInstruction(inst);
            }
        }

        public void RunInstruction(uint instruction)
        {
            ClockCycle(new CPUModuleInputs() { MemReady = true, MemReadData = instruction });
            RunTillInstructionFetch();
        }

        public void RunTillInstructionFetch()
        {
            int counter = 0;

            while (counter++ < 100)
            {
                switch (TopLevel.State.State)
                {
                    case CPUState.Halt:
                        throw new Exception($"Halted");
                    case CPUState.MEM:
                        if (TopLevel.MemRead)
                        {
                            var wordAddress = TopLevel.MemAddress & 0xFFFFFFFC;
                            var byteAddress = TopLevel.MemAddress & 0x3;
                            var word = new RTLBitArray(MemoryBlock[wordAddress]);
                            var data = word >> (int)(byteAddress * 8);
                            ClockCycle(new CPUModuleInputs() { MemReady = true, MemReadData = data });
                        }
                        else if (TopLevel.MemWrite)
                        {
                            var wordAddress = TopLevel.MemAddress & 0xFFFFFFFC;
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
                        return;
                    default:
                        ClockCycle();
                        break;
                }
            }

            throw new Exception("CPU seems to hang");
        }
    }
}
