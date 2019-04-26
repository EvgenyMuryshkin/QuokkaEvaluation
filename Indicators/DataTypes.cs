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

    public enum eIndicatorMode
    {
        Solid,
        Blinking,
        Sliding
    }

    public class IndicatorsControlsState
    {
        public KeypadKeyCode keyCode;
        public uint counterMs;
        public uint dim = 1;
        public uint flashSpeedMs = 500;

        // indicator data
        public eIndicatorMode mode = eIndicatorMode.Blinking;
        public eIndicatorType nextIndicator;
        public uint nextIndicatorKeyEventTimeStamp;

        public eIndicatorType lastIndicator;
        public uint lastIndicatorTimeStamp;

        public uint flashIndicatorTimeStamp;
        public bool isIndicatorActive;

        public int slideValue = 0;

        // auto indicator
        public uint autoIndicatorTimeStamp;
    }

    public struct IndicatorsDBG
    {
        public ushort C1;
        public ushort C2;
    }
}
