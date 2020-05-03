using System;
using System.Linq;
using Quokka.RTL;

namespace RTL.Modules
{
    public class SDP_WF_RAMModule_Inputs
    {
        public byte ReadAddress;
        public byte WriteAddress;
        public byte WriteData;
        public bool WE;
    }

    public class SDP_WF_RAMModule_State
    {
        public byte ReadData;
        public byte[] Buff = new byte[256];
    }

    public class SDP_WF_RAMModule : RTLSynchronousModule<SDP_WF_RAMModule_Inputs, SDP_WF_RAMModule_State>
    {
        public SDP_WF_RAMModule()
        {
        }

        public byte Data => State.ReadData;

        protected override void OnStage()
        {
            if (Inputs.WE)
                NextState.Buff[Inputs.WriteAddress] = Inputs.WriteData; 

            NextState.ReadData = NextState.Buff[Inputs.ReadAddress];
        }
    }
}
