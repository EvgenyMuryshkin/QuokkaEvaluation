using Drivers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeGame
{
    public static class Diagnostics
    {
        public static void ReportState(
            GameControlsState controlsState,
            eDirectionType nextDirectionFromKeypad,
            eDirectionType nextDirectionFromJoystick,
            eDirectionType nextDirection,
            FPGA.Signal<bool> TXD
            )
        {
            SnakeDBG dbg = new SnakeDBG();
            dbg.C1 = controlsState.adcChannel1;
            dbg.C2 = controlsState.adcChannel2;

            dbg.KD = nextDirectionFromKeypad;
            dbg.JD = nextDirectionFromJoystick;
            dbg.ND = nextDirection;

            JSON.SerializeToUART(dbg, TXD);
        }
    }
}
