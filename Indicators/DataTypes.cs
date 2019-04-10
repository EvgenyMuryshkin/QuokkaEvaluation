using Drivers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Indicators
{
    public enum eIndicatorType
    {
        None,
        Left,
        Right,
        Break
    }

    public struct GameControlsState
    {
        public KeypadKeyCode keyCode;
        public ushort adcChannel1;
        public ushort adcChannel2;
        public uint msTickCounter;
    }

    public struct IndicatorsDBG
    {
        public ushort C1;
        public ushort C2;
    }
}
