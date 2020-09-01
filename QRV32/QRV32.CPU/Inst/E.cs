using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRV32.CPU
{
    public partial class RISCVModule
    {
        void OnE()
        {
            NextState.State = CPUState.WB;
            switch (ID.SysTypeCode)
            {
                case SysTypeCodes.CALL:
                    if (HasMTVEC)
                    {
                        // in instruction caused trap, store current address
                        SwitchToTrapHandler(State.PC, 0, MCAUSE.MECall);
                    }
                    else
                    {
                        NextState.State = CPUState.E;
                    }
                    break;
                case SysTypeCodes.BREAK:
                    if (HasMTVEC)
                    {
                        // in instruction caused trap, store current address
                        SwitchToTrapHandler(State.PC, 0, MCAUSE.Breakpoint);
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
                            Halt(HaltCode.RetTypeCode);
                            break;
                    }
                    break;
                case SysTypeCodes.IRQ:
                    if (ID.IRQTypeCode != IRQTypeCodes.WFI)
                    {
                        Halt(HaltCode.IRQTypeCode);
                    }
                    break;
                default:
                    Halt(HaltCode.SysTypeCode);
                    break;
            }
        }
    }
}
