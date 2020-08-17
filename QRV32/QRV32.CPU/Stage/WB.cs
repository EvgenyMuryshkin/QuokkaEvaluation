using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace QRV32.CPU
{
    public partial class RISCVModule
    {
        bool HasMTVEC => State.CSR[(byte)SupportedCSRAddr.mtvec] != 0;

        RTLBitArray InstructionOffset => new RTLBitArray(4).Unsigned();

        RTLBitArray internalNextPC => ID.OpTypeCode == OpTypeCodes.JALR ? State.PCOffset : State.PC + State.PCOffset;

        void DisableInterrupts()
        {
            NextState.CSR[(byte)SupportedCSRAddr.mstatus] = State.CSR[(byte)SupportedCSRAddr.mstatus] & 0xFFFFFFF7;
        }

        void EnableInterrupts()
        {
            NextState.CSR[(byte)SupportedCSRAddr.mstatus] = State.CSR[(byte)SupportedCSRAddr.mstatus] | 0x8;
        }

        void SwitchToTrapHandler(uint mepc, uint mtval, MCAUSE mcause)
        {
            RTLBitArray MSTATUS = State.CSR[(byte)SupportedCSRAddr.mstatus];
            var isMIE = MSTATUS[3];

            if (!isMIE)
            {
                Halt(HaltCode.NoMIE);
            }
            else if(!HasMTVEC)
            {
                Halt(HaltCode.NoTrapHandler);
            }
            else
            {
                NextState.State = CPUState.IF;
                DisableInterrupts();

                NextState.CSR[(byte)SupportedCSRAddr.mepc] = mepc;
                NextState.CSR[(byte)SupportedCSRAddr.mtval] = mtval;
                NextState.CSR[(byte)SupportedCSRAddr.mcause] = (uint)mcause;
                NextState.PC = State.CSR[(byte)SupportedCSRAddr.mtvec];
            }
        }

        void WriteBackStage()
        {
            var pcMisaligned = new RTLBitArray(internalNextPC[1, 0]) != 0;
            var isMRET = ID.SystemCode == SystemCodes.E && ID.SysTypeCode == SysTypeCodes.TRAP && ID.RetTypeCode == RetTypeCodes.MRET;
            RTLBitArray MSTATUS = State.CSR[(byte)SupportedCSRAddr.mstatus];
            var isMIE = MSTATUS[3];

            if (pcMisaligned)
            {
                // address misalign caused trap, store current address
                SwitchToTrapHandler(State.PC, internalNextPC, MCAUSE.InstAddrMisalign);
            }
            else
            {
                NextState.State = CPUState.IF;

                if (isMIE && Inputs.ExtIRQ)
                {
                    // if external cause trap, store next instruction, as current once completed successfully
                    SwitchToTrapHandler(internalNextPC, 0, MCAUSE.MExternalIRQ);
                }
                else if (isMRET)
                {
                    NextState.PC = State.CSR[(byte)SupportedCSRAddr.mepc];
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
