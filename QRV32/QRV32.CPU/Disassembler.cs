using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QRV32.CPU
{
    public class Disassembler
    {
        public string Single(uint address, uint i)
        {
            var id = new InstructionDecoderModule();
            id.Setup();

            id.Cycle(new InstructionDecoderInputs() { Instruction = i });
            var decoded = id.OpTypeCode.ToString();

            switch (id.OpTypeCode)
            {
                case OpTypeCodes.JALR:
                    decoded += $" x{(uint)id.RD}, x{(uint)id.RS1}, {(uint)id.ITypeImm} (0x{id.ITypeImm})";
                    break;
                case OpTypeCodes.LUI:
                    decoded += $" x{(uint)id.RD}, {(uint)id.UTypeImm} (0x{id.UTypeImm})";
                    break;
                case OpTypeCodes.AUIPC:
                    decoded += $" x{(uint)id.RD}, {(uint)id.UTypeImm} (0x{id.UTypeImm})";
                    break;
                case OpTypeCodes.JAL:
                    if (id.RD == 0)
                    {
                        decoded = $"J 0x{(uint)(address + id.JTypeImm):X8}";
                    }
                    else
                    {
                        decoded += $" x{(uint)id.RD}, 0x{(uint)(address + id.JTypeImm):X8} (0x{id.JTypeImm})";
                    }
                    break;
                case OpTypeCodes.OPIMM:
                    decoded = $"{id.OPIMMCode} x{(uint)id.RD}, x{(uint)id.RS1}, {(int)id.ITypeImm} (0x{id.ITypeImm})";
                    break;
                case OpTypeCodes.OP:
                    var op = id.OPCode.ToString();
                    switch (id.OPCode)
                    {
                        case OPCodes.ADD_SUB:
                        {
                            if (id.SUB)
                                op = "SUB";
                            else
                                op = "ADD";
                        }
                        break;
                        case OPCodes.SRL_SRA:
                        {
                            if (id.SHARITH)
                                op = "SRA";
                            else
                                op = "SRL";
                        }
                        break;
                    }

                    decoded = $"{op} x{(uint)id.RD}, x{(uint)id.RS1}, {(uint)id.RS2}";
                    break;
                case OpTypeCodes.STORE:
                    switch ((int)id.Funct3)
                    {
                        case 0:
                            decoded = $"SB x{(uint)id.RS2}, {(int)id.STypeImm}(x{(uint)id.RS1})";
                            break;
                        case 1:
                            decoded = $"SH x{(uint)id.RS2}, {(int)id.STypeImm}(x{(uint)id.RS1})";
                            break;
                        case 2:
                            decoded = $"SW x{(uint)id.RS2}, {(int)id.STypeImm}(x{(uint)id.RS1})";
                            break;
                    }
                    break;
                case OpTypeCodes.LOAD:
                    switch ((int)id.Funct3)
                    {
                        case 0:
                            decoded = $"LB x{(uint)id.RD}, {(int)id.STypeImm}(x{(uint)id.RS1})";
                            break;
                        case 1:
                            decoded = $"LH x{(uint)id.RD}, {(int)id.STypeImm}(x{(uint)id.RS1})";
                            break;
                        case 2:
                            decoded = $"LW x{(uint)id.RD}, {(int)id.STypeImm}(x{(uint)id.RS1})";
                            break;
                        case 4:
                            decoded = $"LBU x{(uint)id.RD}, {(int)id.STypeImm}(x{(uint)id.RS1})";
                            break;
                        case 5:
                            decoded = $"LHU x{(uint)id.RD}, {(int)id.STypeImm}(x{(uint)id.RS1})";
                            break;
                    }
                    break;
                case OpTypeCodes.SYSTEM:
                    switch (id.SystemCode)
                    {
                        case SystemCodes.E:
                            switch (id.SysTypeCode)
                            {
                                case SysTypeCodes.CALL:
                                    decoded = "ECALL";
                                    break;
                                case SysTypeCodes.BREAK:
                                    decoded = "EBREAK";
                                    break;
                                case SysTypeCodes.IRQ:
                                    decoded = "IRQ";
                                    break;
                                case SysTypeCodes.TRAP:
                                    switch(id.RetTypeCode)
                                    {
                                        case RetTypeCodes.MRET:
                                            decoded = $"{id.RetTypeCode}";
                                            break;
                                        default:
                                            decoded = $"TRAP ({id.RetTypeCode})";
                                            break;
                                    }
                                    break;
                            }
                            break;
                        default:
                            if (id.SystemCode >= SystemCodes.CSRRW && id.SystemCode <= SystemCodes.CSRRCI)
                            {
                                decoded = $"{id.SystemCode} x{(uint)id.RD}, {id.CSRAddress}, x{(uint)id.RS1}";
                            }
                            else
                            {
                                decoded += $" {id.SystemCode}";
                            }
                            break;
                    }
                    break;
            }

            return $"[{address:X8}]: {decoded,-32} // 0x{i:X8}";
        }

        public string Disassemble(uint[] instructions)
        {
            var asm = new StringBuilder();

            var lines = instructions.Select((i, idx) =>
            {
                var address = (uint)(idx << 2);
                return Single(address, i);
            });

            lines.ForEach(l => asm.AppendLine(l));

            return asm.ToString();
        }
    }
}
