using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime;
using System.Text;

namespace AES
{
    public class SBoxTestModuleInputs
    {
        public uint LSBData;
        public bool LSBDataReady;
    }

    public enum SBoxTestModuleFSM
    {
        Reset,
        Reading,
        Transforming,
        Writing
    }

    public class SBoxTestModuleState
    {
        public byte Couner;
        public RTLBitArray Value = new RTLBitArray().Resized(128);
        public RTLBitArray Result = new RTLBitArray().Resized(128);
        public SBoxTestModuleFSM FSM = SBoxTestModuleFSM.Reset;
    }

    public class SBoxTestModule : RTLSynchronousModule<SBoxTestModuleInputs, SBoxTestModuleState>
    {
        SBoxModule sbox = new SBoxModule();

        protected override void OnSchedule(Func<SBoxTestModuleInputs> inputsFactory)
        {
            base.OnSchedule(inputsFactory);

            sbox.Schedule(() => new SBoxModuleInputs() { Value = State.Value });
        }

        protected override void OnStage()
        {
            switch (State.FSM)
            {
                case SBoxTestModuleFSM.Reset:
                    NextState.Couner = 0;
                    NextState.FSM = SBoxTestModuleFSM.Reading;
                    break;
                case SBoxTestModuleFSM.Reading:
                    if (Inputs.LSBDataReady)
                    {
                        NextState.Value = ((State.Value >> 32) | (new RTLBitArray(Inputs.LSBData) << 96)).Resized(128);
                        NextState.Couner = (byte)(State.Couner + 1);
                        if (State.Couner == 3)
                        {
                            NextState.FSM = SBoxTestModuleFSM.Transforming;
                        }
                    }
                    break;
                case SBoxTestModuleFSM.Transforming:
                    NextState.Result = sbox.Result;
                    NextState.Couner = 0;
                    NextState.FSM = SBoxTestModuleFSM.Writing;
                    break;
                case SBoxTestModuleFSM.Writing:
                    NextState.Result = (State.Result >> 32).Resized(128);
                    NextState.Couner = (byte)(State.Couner + 1);
                    if (State.Couner == 3)
                    {
                        NextState.FSM = SBoxTestModuleFSM.Reading;
                    }
                    break;
            }
        }

        public uint LSBResult => State.Result[31, 0];
        public bool LBSResultReady => State.FSM == SBoxTestModuleFSM.Writing;
    }
}
