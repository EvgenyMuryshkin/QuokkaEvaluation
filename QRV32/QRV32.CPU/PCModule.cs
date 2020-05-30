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
        public RTLBitArray Offset = new RTLBitArray(uint.MinValue);
    }

    public class PCModuleState
    {
        public RTLBitArray PC = new RTLBitArray(uint.MinValue);
    }

    public class PCModule : RTLSynchronousModule<PCModuleInputs, PCModuleState>
    {
        public bool PCMisaligned => new RTLBitArray(State.PC[1, 0]) != 0;

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
