using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRV32.CPU
{
    public partial class RISCVModule
    {
        bool halfMisaliged => (new RTLBitArray(internalMemAddress))[0] != false;
        bool wordMisaliged => (new RTLBitArray(internalMemAddress))[1, 0] != 0;

        bool MemAddressMisaligned
        {
            get
            {
                bool result = false;
                switch (ID.LoadTypeCode)
                {
                    case LoadTypeCodes.LW:
                        result = wordMisaliged;
                        break;
                    case LoadTypeCodes.LH:
                        result = halfMisaliged;
                        break;
                    case LoadTypeCodes.LHU:
                        result = halfMisaliged;
                        break;
                }

                return result;
            }
        }

        void OnLoadStore()
        {
            NextState.State = CPUState.MEM;

            if (MemAddressMisaligned)
            {
                NextState.State = CPUState.IF;

                // address misalign caused trap, store current address
                DisableInterrupts();
                NextState.CSR[(byte)CSRAddr.mepc] = State.PC;
                NextState.CSR[(byte)CSRAddr.mtval] = internalMemAddress;
                NextState.PC = State.CSR[(byte)CSRAddr.mtvec];

                switch (ID.OpTypeCode)
                {
                    case OpTypeCodes.LOAD:
                        NextState.CSR[(byte)CSRAddr.mcause] = (uint)MCAUSE.LoadAddrMisalign;
                        break;
                    case OpTypeCodes.STORE:
                        NextState.CSR[(byte)CSRAddr.mcause] = (uint)MCAUSE.StoreAddrMisalign;
                        break;
                }
            }
        }
    }
}
