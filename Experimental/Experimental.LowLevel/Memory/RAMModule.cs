using System.Linq;
using Quokka.RTL;

namespace QuokkaTests.Experimental
{
    public class RAMModuleInputs
    {
        public RTLBitArray iAddress = new RTLBitArray().Resized(8);
        public byte iData;
        public bool iWE;
    }

    public class RAMModuleState
    {
        public byte ReadData;
        public byte[] Buff = new byte[256];
    }

    public class RAMModule : RTLSynchronousModule<RAMModuleInputs, RAMModuleState>
    {
        public byte oData => State.ReadData;
        protected override void OnStage()
        {
            NextState.ReadData = State.Buff[Inputs.iAddress];

            if (Inputs.iWE)
                NextState.Buff[Inputs.iAddress] = Inputs.iData;
        }
    }
}
