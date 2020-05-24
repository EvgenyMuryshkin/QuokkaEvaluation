using QRV32.CPU;
using Quokka.RTL.Simulator;
using System;

namespace QRV32.Tests
{
    public class CPUSimulator : RTLSimulator<CPUModule, CPUModuleInputs>
    {
        public CPUSimulator()
        {

        }

        public void RunInstruction(uint instruction)
        {
            ClockCycle(new CPUModuleInputs() { MemReady = true, MemReadValue = instruction });
            RunTillInstructionFetch();
        }

        public void RunTillInstructionFetch()
        {
            int counter = 0;

            while (!TopLevel.MemRead)
            {
                if (++counter > 100)
                    throw new Exception("CPU seems to hang");

                switch (TopLevel.State.State)
                {
                    case CPUState.Halt:
                        throw new Exception($"Halted");
                    case CPUState.MEM:
                        // TODO: implement memory operation
                        break;
                    case CPUState.IF:
                        return;
                }

                ClockCycle();
            }
        }
    }
}
