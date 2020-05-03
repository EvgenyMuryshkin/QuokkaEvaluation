using System;
using System.Collections.Generic;
using System.Linq;
using Quokka.RTL;

namespace RTL.Modules
{
    public class LogicRAMModuleInputs
    {
        public byte Value;
    }

    public class LogicRAMModuleState
    {
        public RTLBitArray Index = new RTLBitArray().Resized(2);
        public byte[] Buff = new byte[4];
    }

    public class LogicRAMModule : RTLSynchronousModule<LogicRAMModuleInputs, LogicRAMModuleState>
    {
        public LogicRAMModule()
        {
        }

        public byte Avg => (byte)((State.Buff[0] + State.Buff[1] + State.Buff[2] + State.Buff[3]) >> 2);

        protected override void OnStage()
        {
            NextState.Buff[State.Index] = Inputs.Value;
            NextState.Index = (State.Index + 1)[1, 0];
        }
    }
}
