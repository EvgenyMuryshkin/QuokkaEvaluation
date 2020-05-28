using Quokka.RTL;
using System;

namespace QRV32.CPU
{
    public class InstructionDecoderInputs
    {
        public uint Instruction;
    }

    public class InstructionDecoderModule : RTLCombinationalModule<InstructionDecoderInputs>
    {
        protected RTLBitArray Bits => Inputs.Instruction;
        protected RTLBitArray ZeroU32 => 0;
        public RTLBitArray OpCode => Bits[6, 0];
        public RTLBitArray RD => Bits[11, 7];
        public RTLBitArray RS1 => Bits[19, 15];
        public RTLBitArray RS2 => Bits[24, 20];
        public RTLBitArray Funct3 => Bits[14, 12];
        public RTLBitArray Funct7 => Bits[31, 25];
        public RTLBitArray RTypeImm => new RTLBitArray(Bits[31, 20]).Signed().Resized(32);
        public RTLBitArray ITypeImm => new RTLBitArray(Bits[31, 20]).Signed().Resized(32);
        public RTLBitArray STypeImm => new RTLBitArray(Bits[31, 25], Bits[11, 7]).Signed().Resized(32);
        public RTLBitArray BTypeImm => new RTLBitArray(Bits[31], Bits[7], Bits[30, 25], Bits[11, 8], false).Signed().Resized(32);
        public RTLBitArray UTypeImm => new RTLBitArray(Bits[31, 12], ZeroU32[11, 0]).Signed().Resized(32);
        public RTLBitArray JTypeImm => new RTLBitArray(Bits[31], Bits[19, 12], Bits[20], Bits[30, 21], false).Signed().Resized(32);

        public RTLBitArray SHAMT => ITypeImm[4, 0];
        public bool SHARITH => ITypeImm[10];
    }
}