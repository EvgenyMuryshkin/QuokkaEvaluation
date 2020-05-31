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
            ClockCycle(new CPUModuleInputs() { MemReady = true, MemReadValue = instruction });
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
                        var wordAddress = TopLevel.MemAddress & 0xFFFFFFFC;
                        var byteAddress = TopLevel.MemAddress & 0x3;
                        var word = new RTLBitArray(MemoryBlock[wordAddress]);
                        var data = word >> (int)(byteAddress * 8);
                        ClockCycle(new CPUModuleInputs() { MemReady = true, MemReadValue = data });
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
