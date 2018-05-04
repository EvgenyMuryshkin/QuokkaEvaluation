using Drivers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeGame
{
    public enum eGameMode : byte
    {
        Setup,
        Play,
        Failed,
        Completed
    }

    public enum eDirectionType : byte
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    public enum eCellType : byte
    {
        None,
        NextPart,
        SnakeHead,
        SnakeUp,
        SnakeDown,
        SnakeLeft,
        SnakeRight,
        RedCross,
        GreenCross,
    }

    public struct Position
    {
        public byte row;
        public byte col;
    }

    public struct GameControlsState
    {
        public KeypadKeyCode keyCode;
        public ushort adcChannel1;
        public ushort adcChannel2;
    }

    public struct SnakeDBG
    {
        public ushort C1;
        public ushort C2;
        public eDirectionType KD;
        public eDirectionType JD;
        public eDirectionType ND;
    }
}
