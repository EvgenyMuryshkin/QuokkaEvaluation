using Quokka.RTL;
using System;

namespace QRV32.CPU
{
    public class InstructionDecoderInputs
    {
        public uint Instruction;
    }

    public abstract class TypeDecoderModule : RTLCombinationalModule<InstructionDecoderInputs>
    {
        protected RTLBitArray Bits => Inputs.Instruction;
        protected RTLBitArray ZeroU32 => 0;
        public RTLBitArray OpCode => Bits[6, 0];
    }

    public class RTypeDecoderModule : TypeDecoderModule
    {
        public RTLBitArray RD => Bits[11, 7];
        public RTLBitArray Funct3 => Bits[14, 12];
        public RTLBitArray RS1 => Bits[19, 15];
        public RTLBitArray RS2 => Bits[24, 20];
        public RTLBitArray Funct7 => Bits[31, 25];
    }

    public class ITypeDecoderModule : TypeDecoderModule
    {
        public RTLBitArray RD => Bits[11, 7];
        public RTLBitArray Funct3 => Bits[14, 12];
        public RTLBitArray RS1 => Bits[19, 15];
        public RTLBitArray Imm => new RTLBitArray(Bits[31, 20]).Signed().Resized(32);
    }

    public class STypeDecoderModule : TypeDecoderModule
    {
        public RTLBitArray Funct3 => Bits[14, 12];
        public RTLBitArray RS1 => Bits[19, 15];
        public RTLBitArray RS2 => Bits[24, 20];
        public RTLBitArray Imm => new RTLBitArray(Bits[31, 25], Bits[11, 7]).Signed().Resized(32);
    }

    public class BTypeDecoderModule : TypeDecoderModule
    {
        public RTLBitArray Funct3 => Bits[14, 12];
        public RTLBitArray RS1 => Bits[19, 15];
        public RTLBitArray RS2 => Bits[24, 20];
        public RTLBitArray Imm => new RTLBitArray(Bits[31], Bits[7], Bits[30, 25], Bits[11, 8], false).Signed().Resized(32);
    }

    public class UTypeDecoderModule : TypeDecoderModule
    {
        public RTLBitArray RD => Bits[11, 7];
        public RTLBitArray Imm => new RTLBitArray(Bits[31, 12], ZeroU32[11, 0]).Signed().Resized(32);
    }  

    public class JTypeDecoderModule : TypeDecoderModule
    {
        public RTLBitArray RD => Bits[11, 7];
        public RTLBitArray Imm => new RTLBitArray(Bits[31], Bits[19, 12], Bits[20], Bits[30, 21], false).Signed().Resized(32);
    }            
}