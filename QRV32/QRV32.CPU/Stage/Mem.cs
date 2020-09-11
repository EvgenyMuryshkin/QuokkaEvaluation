using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRV32.CPU
{
    public partial class RISCVModule
    {
        public RTLBitArray MemWriteData => Regs.RS2;
        public RTLBitArray MemAccessMode => ID.Funct3;

        internal bool IsLoadOp => ID.OpTypeCode == OpTypeCodes.LOAD;
        internal bool IsStoreOp => ID.OpTypeCode == OpTypeCodes.STORE;

        public bool MemRead => State.State == CPUState.IF || (State.State == CPUState.MEM && IsLoadOp);
        public bool MemWrite => State.State == CPUState.MEM && IsStoreOp;
        uint internalMemAddress
        {
            get
            {
                uint address = 0;
                if (State.State == CPUState.IF)
                {
                    address = State.PC;
                }
                else if (IsLoadOp)
                {
                    address = Regs.RS1 + ID.ITypeImm;
                }
                else if (IsStoreOp)
                {
                    address = Regs.RS1 + ID.STypeImm;
                }

                return address;
            }
        }
        public uint MemAddress => internalMemAddress;

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
                            NextState.WBData = Inputs.MemReadData;
                            break;
                        case LoadTypeCodes.LH:
                            NextState.WBData = Inputs.MemReadData[15, 0].Signed().Resized(32);
                            break;
                        case LoadTypeCodes.LHU:
                            NextState.WBData = Inputs.MemReadData[15, 0].Unsigned().Resized(32);
                            break;
                        case LoadTypeCodes.LB:
                            NextState.WBData = Inputs.MemReadData[7, 0].Signed().Resized(32);
                            break;
                        case LoadTypeCodes.LBU:
                            NextState.WBData = Inputs.MemReadData[7, 0].Unsigned().Resized(32);
                            break;
                    }
                }
            }
        }
    }
}
