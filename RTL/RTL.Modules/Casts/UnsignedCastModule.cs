using Quokka.RTL;
using System;

namespace RTL.Modules
{
    public class UnsignedCastModuleInputs
    {
        public ushort UShortValue;
    }

    public class UnsignedCastModule : RTLCombinationalModule<UnsignedCastModuleInputs>
    {
        public byte ByteValue => (byte)Inputs.UShortValue;
        public sbyte SByteValue => (sbyte)Inputs.UShortValue;
        public short ShortValue => (short)Inputs.UShortValue;
        public int IntValue => Inputs.UShortValue;
        public uint UIntValue => (uint)Inputs.UShortValue;
    }
}
