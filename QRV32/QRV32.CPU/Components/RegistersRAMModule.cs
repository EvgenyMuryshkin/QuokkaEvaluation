using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRV32.CPU
{
    public class RegistersRAMModuleState : RegistersModuleState
    {
        public uint ReadData;

        public byte Mode;
        public uint RS1;
        public uint RS2;
        public bool Ready;
    }

    public class RegistersRAMModule : RegistersModule<RegistersRAMModuleState>
    {
        public override RTLBitArray RS1 => State.RS1;
        public override RTLBitArray RS2 => State.RS2;
        public override bool Ready => State.Ready;

        // TODO: support local declarations in stage
        RTLBitArray ReadAddress => State.Mode == 0 ? Inputs.RS1Addr : Inputs.RS2Addr;

        protected override void OnStage()
        {
            var we = Inputs.WE && Inputs.RD != 0;

            if (we)
                NextState.x[Inputs.RD] = Inputs.WriteData;

            NextState.ReadData = State.x[ReadAddress];

            NextState.Ready = false;

            if (Inputs.Read && State.Mode == 0)
            {
                NextState.Mode = 1;
            }

            if (State.Mode == 1)
            {
                NextState.Mode = 2;
                NextState.RS1 = State.ReadData;
            }

            if (State.Mode == 2)
            {
                NextState.Ready = true;
                NextState.Mode = 0;
                NextState.RS2 = State.ReadData;
            }
        }
    }
}
