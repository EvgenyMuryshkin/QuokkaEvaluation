using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRV32.CPU
{
    public partial class RISCVModule
    {
        RTLBitArray CSRAddress => new RTLBitArray((byte)CSRLookup())[3, 0];

        CSRAddr CSRLookup()
        {
            CSRAddr address = CSRAddr.mvendorid;

            switch (ID.CSRAddress)
            {
                case CSRCodes.mvendorid: address = CSRAddr.mvendorid; break;
                case CSRCodes.marchid: address = CSRAddr.marchid; break;
                case CSRCodes.mimpid: address = CSRAddr.mimpid; break;
                case CSRCodes.mhartid: address = CSRAddr.mhartid; break;
                case CSRCodes.mstatus: address = CSRAddr.mstatus; break;
                case CSRCodes.misa: address = CSRAddr.misa; break;
                case CSRCodes.mie: address = CSRAddr.mie; break;
                case CSRCodes.mtvec: address = CSRAddr.mtvec; break;
                case CSRCodes.mepc: address = CSRAddr.mepc; break;
                case CSRCodes.mcause: address = CSRAddr.mcause; break;
                case CSRCodes.mtval: address = CSRAddr.mtval; break;
                case CSRCodes.mip: address = CSRAddr.mip; break;
            }

            return address;
        }

        bool IsCSR => ID.SystemCode >= SystemCodes.CSRRW && ID.SystemCode <= SystemCodes.CSRRCI;
        RTLBitArray CSRI => ID.RS1.Unsigned().Resized(32);
        bool CSRWE => ID.RS1 != 0 && CSRAddress != 0;

        void OnE()
        {
            switch (ID.SysTypeCode)
            {
                case SysTypeCodes.CALL:
                    if (HasMTVEC)
                    {
                        // go into trap handler
                        NextState.pendingMCause = MCAUSE.MECall;
                        NextState.State = CPUState.WB;
                    }
                    else
                    {
                        NextState.State = CPUState.E;
                    }
                    break;
                case SysTypeCodes.BREAK:
                    if (HasMTVEC)
                    {
                        // go into trap handler
                        NextState.pendingMCause = MCAUSE.Breakpoint;
                        NextState.State = CPUState.WB;
                    }
                    else
                    {
                        NextState.State = CPUState.E;
                    }
                    break;
                case SysTypeCodes.TRAP:
                    switch (ID.RetTypeCode)
                    {
                        case RetTypeCodes.MRET:
                            // handled in WB
                            break;
                        default:
                            Halt();
                            break;
                    }
                    break;
                case SysTypeCodes.IRQ:
                    switch (ID.IRQTypeCode)
                    {
                        case IRQTypeCodes.WFI:
                            NextState.State = CPUState.WB;
                            break;
                        default:
                            Halt();
                            break;
                    }
                    break;
                default:
                    Halt();
                    break;
            }
        }

        void OnCSR()
        {
            NextState.State = CPUState.WB;
            NextState.WBData = State.CSR[CSRAddress];
            NextState.WBDataReady = ID.RD != 0;

            if (CSRWE)
            {
                switch (ID.SystemCode)
                {
                    case SystemCodes.CSRRW:
                        NextState.CSR[CSRAddress] = Regs.RS1;
                        break;
                    case SystemCodes.CSRRWI:
                        NextState.CSR[CSRAddress] = CSRI;
                        break;
                    case SystemCodes.CSRRS:
                        NextState.CSR[CSRAddress] = State.CSR[CSRAddress] | Regs.RS1;
                        break;
                    case SystemCodes.CSRRSI:
                        NextState.CSR[CSRAddress] = State.CSR[CSRAddress] | CSRI;
                        break;
                    case SystemCodes.CSRRC:
                        NextState.CSR[CSRAddress] = State.CSR[CSRAddress] & !Regs.RS1;
                        break;
                    case SystemCodes.CSRRCI:
                        NextState.CSR[CSRAddress] = State.CSR[CSRAddress] & !CSRI;
                        break;
                    default:
                        Halt();
                        break;
                }
            }
        }

        void OnSystem()
        {
            if (ID.SystemCode == SystemCodes.E)
            {
                OnE();
            }
            else if (IsCSR)
            {
                OnCSR();
            }
            else
            {
                Halt();
            }
        }
    }
}
