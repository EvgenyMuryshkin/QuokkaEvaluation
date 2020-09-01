﻿using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRV32.CPU
{
    public partial class RISCVModule
    {
        RTLBitArray CSRAddress => CSRData.Item1;
        bool CSRWriteFault => CSRData.Item2;

        (RTLBitArray, bool) CSRData
        {
            get
            {
                SupportedCSRAddr address = SupportedCSRAddr.mvendorid;
                bool CSRWriteFault = false;
                switch (ID.CSRAddress)
                {
                    case CSRCodes.mvendorid: address = SupportedCSRAddr.mvendorid; break;
                    case CSRCodes.marchid: address = SupportedCSRAddr.marchid; break;
                    case CSRCodes.mimpid: address = SupportedCSRAddr.mimpid; break;
                    case CSRCodes.mhartid: address = SupportedCSRAddr.mhartid; break;
                    case CSRCodes.mstatus: address = SupportedCSRAddr.mstatus; break;
                    case CSRCodes.misa: address = SupportedCSRAddr.misa; break;
                    case CSRCodes.mie: address = SupportedCSRAddr.mie; break;
                    case CSRCodes.mtvec: address = SupportedCSRAddr.mtvec; break;
                    case CSRCodes.mepc: address = SupportedCSRAddr.mepc; break;
                    case CSRCodes.mcause: address = SupportedCSRAddr.mcause; break;
                    case CSRCodes.mtval: address = SupportedCSRAddr.mtval; break;
                    case CSRCodes.mip: address = SupportedCSRAddr.mip; break;
                    case CSRCodes.mscratch: address = SupportedCSRAddr.mscratch; break;
                    default:
                        CSRWriteFault = true;
                        break;
                }

                return (new RTLBitArray((byte)address)[3, 0], CSRWriteFault);
            }
        }

        uint CSRWriteData
        {
            get
            {
                var CSRI = ID.RS1.Unsigned().Resized(32);

                uint result = 0;
                switch (ID.SystemCode)
                {
                    case SystemCodes.CSRRW:
                        result = Regs.RS1;
                        break;
                    case SystemCodes.CSRRWI:
                        result = CSRI;
                        break;
                    case SystemCodes.CSRRS:
                        result = State.CSR[CSRAddress] | Regs.RS1;
                        break;
                    case SystemCodes.CSRRSI:
                        result = State.CSR[CSRAddress] | CSRI;
                        break;
                    case SystemCodes.CSRRC:
                        result = State.CSR[CSRAddress] & !Regs.RS1;
                        break;
                    case SystemCodes.CSRRCI:
                        result = State.CSR[CSRAddress] & !CSRI;
                        break;
                }

                return result;
            }
        }

        void OnCSR()
        {
            NextState.State = CPUState.WB;
            NextState.WBData = State.CSR[CSRAddress];
            NextState.WBDataReady = ID.RD != 0;

            if (ID.CSRWE)
            {
                if (CSRWriteFault)
                {
                    Halt(HaltCode.CSRWriteFault);
                }
                else
                {
                    NextState.CSR[CSRAddress] = CSRWriteData;
                }
            }
        }
    }
}
