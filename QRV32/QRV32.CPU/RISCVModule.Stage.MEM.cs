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

        void CheckMemAddressMisalign()
        {
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

        RTLBitArray LWData => Inputs.MemReadData;
        RTLBitArray LHData => Inputs.MemReadData[15, 0].Signed().Resized(32);
        RTLBitArray LHUData => Inputs.MemReadData[15, 0].Unsigned().Resized(32);
        RTLBitArray LBData => Inputs.MemReadData[7, 0].Signed().Resized(32);
        RTLBitArray LBUData => Inputs.MemReadData[7, 0].Unsigned().Resized(32);

        // TODO: inlined RTLBitArray operations
        void MemStage()
        {
            if (Inputs.MemReady)
            {
                NextState.State = CPUState.WB;
                if (IsLoadOp)
                {
                    NextState.WBDataReady = true;

                    switch (ID.LoadTypeCode)
                    {
                        case LoadTypeCodes.LW:
                            NextState.WBData = LWData;
                            break;
                        case LoadTypeCodes.LH:
                            NextState.WBData = LHData;
                            break;
                        case LoadTypeCodes.LHU:
                            NextState.WBData = LHUData;
                            break;
                        case LoadTypeCodes.LB:
                            NextState.WBData = LBData;
                            break;
                        case LoadTypeCodes.LBU:
                            NextState.WBData = LBUData;
                            break;
                    }
                }
            }
        }
    }
}
