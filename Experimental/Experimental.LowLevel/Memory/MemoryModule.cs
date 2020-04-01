using Quokka.RTL;

namespace QuokkaTests.Experimental
{
    public class MemoryInputs
    {
        public byte ReadAddress;
        public byte WriteAddress;
        public ushort WriteData; 
    }

    public class MemoryState
    {

    }

    public class MemoryModule : RTLSynchronousModule<MemoryInputs, MemoryState>
    {
        protected override void OnStage()
        {
            
        }
    }
}
