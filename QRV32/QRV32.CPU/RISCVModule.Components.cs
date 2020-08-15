using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QRV32.CPU
{
    public partial class RISCVModule
    {
        internal InstructionDecoderModule ID = new InstructionDecoderModule();
        internal IRegistersModule Regs = null;
        internal ALUModule ALU = new ALUModule();
        internal CompareModule CMP = new CompareModule();

        public RISCVModule()
        {
            var ramRegs = true;
            if (ramRegs)
            {
                Regs = new RegistersRAMModule();
            }
            else
            {
                Regs = new RegistersBlockModule();
            }
        }

        protected override void OnSchedule(Func<RISCVModuleInputs> inputsFactory)
        {
            base.OnSchedule(inputsFactory);

            ID.Schedule(() => new InstructionDecoderInputs()
            {
                Instruction = State.Instruction
            });

            Regs.Schedule(() => new RegistersModuleInput()
            {
                RS1Addr = ID.RS1,
                RS2Addr = ID.RS2,
                RD = ID.RD,
                Read = RegsRead,
                WriteData = State.WBData,
                WE = RegsWE
            });

            ALU.Schedule(() => new ALUModuleInputs()
            {
                Op1 = ALUOp1,
                Op2 = ALUOp2,
                SHAMT = ALUSHAMT
            });

            CMP.Schedule(() => new CompareModuleInputs()
            {
                Lhs = CMPLhs,
                Rhs = CMPRhs
            });
        }

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
