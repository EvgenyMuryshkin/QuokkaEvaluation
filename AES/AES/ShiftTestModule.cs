using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime;
using System.Text;

namespace AES
{
    public class ShiftTestModuleInputs
    {
        public uint LSBData;
        public bool LSBDataReady;
    }

    public enum ShiftTestModuleFSM
    {
        Reset,
        Reading,
        Transforming,
        Writing
    }

    public class ShiftTestModuleState
    {
        public byte Couner;
        public RTLBitArray Value = new RTLBitArray().Resized(128);
        public RTLBitArray Result = new RTLBitArray().Resized(128);
        public ShiftTestModuleFSM FSM = ShiftTestModuleFSM.Reset;
    }

    public class ShiftTestModule : RTLSynchronousModule<ShiftTestModuleInputs, ShiftTestModuleState>
    {
        ShiftModule shift = new ShiftModule();

        protected override void OnSchedule(Func<ShiftTestModuleInputs> inputsFactory)
        {
            base.OnSchedule(inputsFactory);

            shift.Schedule(() => new ShiftModuleInputs() { Value = State.Value });
        }

        protected override void OnStage()
        {
            switch (State.FSM)
            {
                case ShiftTestModuleFSM.Reset:
                    NextState.Couner = 0;
                    NextState.FSM = ShiftTestModuleFSM.Reading;
                    break;
                case ShiftTestModuleFSM.Reading:
                    if (Inputs.LSBDataReady)
                    {
                        NextState.Value = ((State.Value >> 32) | (new RTLBitArray(Inputs.LSBData) << 96)).Resized(128);
                        NextState.Couner = (byte)(State.Couner + 1);
                        if (State.Couner == 3)
                        {
                            NextState.FSM = ShiftTestModuleFSM.Transforming;
                        }
                    }
                    break;
                case ShiftTestModuleFSM.Transforming:
                    NextState.Result = shift.Result;
                    NextState.Couner = 0;
                    NextState.FSM = ShiftTestModuleFSM.Writing;
                    break;
                case ShiftTestModuleFSM.Writing:
                    NextState.Result = (State.Result >> 32).Resized(128);
                    NextState.Couner = (byte)(State.Couner + 1);
                    if (State.Couner == 3)
                    {
                        NextState.FSM = ShiftTestModuleFSM.Reading;
                    }
                    break;
            }
        }

        public uint LSBResult => State.Result[31, 0];
        public bool LBSResultReady => State.FSM == ShiftTestModuleFSM.Writing;
    }
}
