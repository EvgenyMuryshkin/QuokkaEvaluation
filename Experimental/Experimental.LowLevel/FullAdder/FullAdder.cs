using Quokka.RTL;

namespace QuokkaTests.Experimental
{
    public class FullAdderInputs
    {
        public bool A = false;
        public bool B = false;
        public bool CIn = false;
    }
    
    public class FullAdderModule : RTLCombinationalModule<FullAdderInputs>
    {
        private bool P => Inputs.A ^ Inputs.B;

        public bool O => P ^ Inputs.CIn;
        public bool COut => (Inputs.A & Inputs.B) || (P & Inputs.CIn);
    }
}
