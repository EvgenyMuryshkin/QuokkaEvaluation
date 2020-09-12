using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuSoC
{
    public class SoCUARTSimModuleInputs : SoCComponentModuleInputs
    {
    }

    public class SoCUARTSimModuleState
    {
        public byte[] UART = new byte[4] { 0, 0, 2, 0 };
        public bool UART_TX;
        public byte txSimCounter;
    }

    public class SoCUARTSimModule : SoCComponentModule<SoCUARTSimModuleInputs, SoCUARTSimModuleState>
    {
        public SoCUARTSimModule() : base(4)
        {

        }

        bool internalIsActive => addressMatch;
        bool internalIsReady => State.UART[2] != 0;

        public override bool IsActive => internalIsActive;
        public override bool IsReady => internalIsReady;
        public override uint ReadValue => new RTLBitArray(
            State.UART[3], State.UART[2], State.UART[1], State.UART[0]) >> internalByteAddress;

        protected override void OnStage()
        {
            NextState.UART_TX = false;

            if (internalIsReady && internalIsActive && Inputs.Common.WE)
            {
                // TODO: implicit cast is not handled in rtl transform
                NextState.UART[0] = (byte)Inputs.Common.WriteValue;
                NextState.UART[2] = 0;
                NextState.UART_TX = true;

                // simulate 100 cycles transmission
                NextState.txSimCounter = 100;
            }

            if (!internalIsReady)
            {
                if (State.txSimCounter == 0)
                    NextState.UART[2] = 2;
                else
                    NextState.txSimCounter = (byte)(State.txSimCounter - 1);
            }
        }
    }
}
