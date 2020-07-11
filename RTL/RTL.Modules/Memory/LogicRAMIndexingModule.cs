using System;
using System.Collections.Generic;
using System.Linq;
using Quokka.RTL;

namespace RTL.Modules
{
    public class LogicRAMIndexingModuleInputs
    {
        public bool WE;
        public RTLBitArray WriteAddr = new RTLBitArray().Resized(2);
        public byte WriteData;

        public RTLBitArray ReadAddr = new RTLBitArray().Resized(2);
        public byte OpData;

    }

    public class LogicRAMIndexingModuleState
    {
        public byte[] Buff = new byte[4];
    }

    public class LogicRAMIndexingModule : RTLSynchronousModule<LogicRAMIndexingModuleInputs, LogicRAMIndexingModuleState>
    {
        public byte MemLhsRhs => (byte)(State.Buff[1] + State.Buff[Inputs.ReadAddr]);
        public byte MathMemLhs => (byte)(State.Buff[Inputs.ReadAddr] - Inputs.OpData);
        public byte MathMemRhs => (byte)(Inputs.OpData + State.Buff[Inputs.ReadAddr]);

        public byte LogicMemLhs => (byte)(State.Buff[Inputs.ReadAddr] | Inputs.OpData);
        public byte LogicMemRhs => (byte)(Inputs.OpData & State.Buff[Inputs.ReadAddr]);

        public bool CmpMemLhs => State.Buff[Inputs.ReadAddr] > Inputs.OpData;
        public bool CmpMemRhs => Inputs.OpData > State.Buff[Inputs.ReadAddr];

        protected override void OnStage()
        {
            if (Inputs.WE)
                NextState.Buff[Inputs.WriteAddr] = Inputs.WriteData;
        }
    }
}
