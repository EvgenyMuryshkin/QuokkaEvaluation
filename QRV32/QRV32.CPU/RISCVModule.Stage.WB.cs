using System;
using System.Collections.Generic;
using System.Text;

namespace QRV32.CPU
{
    public partial class RISCVModule
    {
        void DisableInterrupts()
        {
            NextState.CSR[(byte)CSRAddr.mstatus] = State.CSR[(byte)CSRAddr.mstatus] & 0xFFFFFFF7;
        }

        void EnableInterrupts()
        {
            NextState.CSR[(byte)CSRAddr.mstatus] = State.CSR[(byte)CSRAddr.mstatus] | 0x8;
        }

        void WriteBackStage()
        {
            if (PCMisaligned)
            {
                if (MIE && HasMTVEC)
                {
                    NextState.State = CPUState.IF;

                    // address misalign caused trap, store current address
                    DisableInterrupts();
                    NextState.CSR[(byte)CSRAddr.mepc] = State.PC;
                    NextState.CSR[(byte)CSRAddr.mtval] = internalNextPC;
                    NextState.CSR[(byte)CSRAddr.mcause] = (uint)MCAUSE.InstAddrMisalign;
                    NextState.PC = State.CSR[(byte)CSRAddr.mtvec];
                }
                else
                {
                    Halt();
                }
            }
            else
            {
                NextState.State = CPUState.IF;

                if (State.pendingMCause != 0)
                {
                    if (MIE && HasMTVEC)
                    {
                        // in instruction caused trap, store current address
                        DisableInterrupts();
                        NextState.CSR[(byte)CSRAddr.mepc] = State.PC;
                        NextState.CSR[(byte)CSRAddr.mcause] = (uint)NextState.pendingMCause;
                        NextState.PC = State.CSR[(byte)CSRAddr.mtvec];
                        NextState.pendingMCause = 0;
                    }
                    else
                    {
                        // no trap handler, halt execution
                        Halt();
                    }
                }
                else if (MIE && Inputs.ExtIRQ)
                {
                    // if external cause trap, store next instruction, as current once completed successfully
                    DisableInterrupts();
                    NextState.CSR[(byte)CSRAddr.mepc] = internalNextPC;
                    NextState.CSR[(byte)CSRAddr.mcause] = (uint)MCAUSE.MExternalIRQ;
                    NextState.PC = State.CSR[(byte)CSRAddr.mtvec];
                }
                else if (MRET)
                {
                    NextState.PC = State.CSR[(byte)CSRAddr.mepc];
                    EnableInterrupts();
                }
                else
                {
                    NextState.PC = internalNextPC;
                }
            }
        }
    }
}
