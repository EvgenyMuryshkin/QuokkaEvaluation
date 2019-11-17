using Quokka.RTL;

namespace QuokkaTests.Experimental
{
    // Example module

    /// <summary>
    /// Inputs declarations, names, types and sizes;
    /// </summary>
    public class TransmitterInputs
    {
        public RTLBitArray Trigger = false;
        public RTLBitArray Ack = false;
        public RTLBitArray Data = byte.MinValue;
    }
}
