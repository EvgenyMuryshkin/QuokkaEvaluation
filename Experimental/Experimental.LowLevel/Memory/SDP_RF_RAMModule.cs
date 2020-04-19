using System;
using System.Linq;
using Quokka.RTL;

namespace QuokkaTests.Experimental
{
    public class SDP_RF_RAMModule_Inputs
    {
        public byte ReadAddress;
        public byte WriteAddress;
        public byte WriteData;
        public bool WE;
    }

    public class SDP_RF_RAMModule_State
    {
        public byte ReadData;
        public byte[] Buff = new byte[256];
    }

    public class SDP_RF_RAMModule : RTLSynchronousModule<SDP_RF_RAMModule_Inputs, SDP_RF_RAMModule_State>
    {
        public SDP_RF_RAMModule()
        {
        }

        public byte Data => State.ReadData;

        protected override void OnStage()
        {
            if (Inputs.WE)
                NextState.Buff[Inputs.WriteAddress] = Inputs.WriteData; 

            NextState.ReadData = State.Buff[Inputs.ReadAddress];
        }
    }
}
