using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRV32.CPU
{
    public class PCModuleInputs
    {
        public bool WE;
        public bool Overwrite;
        public uint Offset;
    }

    public class PCModuleState
    {
        public uint PC;
    }

    public class PCModule : RTLSynchronousModule<PCModuleInputs, PCModuleState>
    {
        public uint PC => State.PC;
        protected override void OnStage()
        {
            if (Inputs.WE)
            {
                NextState.PC = Inputs.Overwrite ? Inputs.Offset : State.PC + Inputs.Offset;
            }
        }
    }
}
