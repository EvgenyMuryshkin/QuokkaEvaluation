using Quokka.RTL;
using System;

namespace RTL.Modules
{
    public class SignedCastModuleInputs
    {
        public short ShortValue;
    }

    public class SignedCastModule : RTLCombinationalModule<SignedCastModuleInputs>
    {
        public byte ByteValue => (byte)Inputs.ShortValue;
        public sbyte SByteValue => (sbyte)Inputs.ShortValue;
        public ushort UShortValue => (ushort)Inputs.ShortValue;
        public int IntValue => Inputs.ShortValue;
        public uint UIntValue => (uint)Inputs.ShortValue;
    }
}
