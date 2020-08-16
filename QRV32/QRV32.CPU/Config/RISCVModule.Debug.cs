using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QRV32.CPU
{
    public partial class RISCVModule
    {
        public bool IsHalted => State.State == CPUState.Halt;

        public override string ToString()
        {
            // this is non-synthesizable method, anything can be done here
            var dump = new StringBuilder();
            var disasm = new Disassembler();

            dump.AppendLine($"RISC-V Dump:");
            dump.AppendLine($"PC: 0x{State.PC}");
            dump.AppendLine($"State: {State.State}");
            dump.AppendLine($"Instruction: 0x{State.Instruction:X8}");
            dump.AppendLine($"Disassembled: {disasm.Single(State.PC, State.Instruction)}");
            dump.AppendLine($"OpCode: {ID.OpTypeCode}");
            dump.AppendLine($"=== REGS ===");
            Regs
                .State
                .x
                .Select((r, idx) => $"x{idx}".PadRight(3) + $": 0x{r:X8}")
                .ForEach(l => dump.AppendLine(l));

            return dump.ToString();
        }
    }
}
