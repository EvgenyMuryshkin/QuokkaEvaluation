using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRV32.CPU
{
    public class RegistersBlockModuleState : RegistersModuleState
    {
        public uint ReadData;
    }

    public class RegistersBlockModule : RegistersModule<RegistersBlockModuleState>
    {
        public override RTLBitArray RS1 => State.x[Inputs.RS1Addr];
        public override RTLBitArray RS2 => State.x[Inputs.RS2Addr];
        public override bool Ready => Inputs.Read;

        protected override void OnStage()
        {
            if (Inputs.WE && Inputs.RD != 0)
                NextState.x[Inputs.RD] = Inputs.WriteData;
        }
    }
}
