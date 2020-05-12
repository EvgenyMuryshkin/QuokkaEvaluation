﻿using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Text;

namespace QRV32.CPU
{
    public class RegistersModuleInput
    {
        public bool WE;
        public RTLBitArray WriteAddress = new RTLBitArray().Resized(5);
        public uint WriteData;

        public RTLBitArray ReadAddress = new RTLBitArray().Resized(5);
    }

    public class RegistersModuleState
    {
        public uint[] x = new uint[32];
        public uint ReadData;
    }

    public class RegistersModule : RTLSynchronousModule<RegistersModuleInput, RegistersModuleState>
    {
        public uint ReadData => State.ReadData;
        protected override void OnStage()
        {
            var we = Inputs.WE && Inputs.WriteAddress != 0;

            if (we)
                NextState.x[Inputs.WriteAddress] = Inputs.WriteData;

            NextState.ReadData = State.x[Inputs.ReadAddress];
        }
    }
}
