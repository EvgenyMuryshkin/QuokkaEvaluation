using Quokka.RTL;

namespace RTL.Modules
{
    public class SignalsMuxModuleInputs
    {
        public RTLBitArray Addr { get; set; } = new RTLBitArray().Resized(2);
        public byte Sig0 { get; set; }
        public byte Sig1 { get; set; }
        public byte Sig2 { get; set; }
        public byte Sig3 { get; set; }
    }

    public class SignalsMuxModule : RTLCombinationalModule<SignalsMuxModuleInputs>
    {
        byte[] signals => new byte[] 
        { 
            Inputs.Sig0, 
            Inputs.Sig1, 
            Inputs.Sig2, 
            Inputs.Sig3 
        };

        public byte Value => signals[Inputs.Addr];
    }
}
