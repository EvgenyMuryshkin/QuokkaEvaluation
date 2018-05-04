using System;
using System.Collections.Generic;
using System.Text;

namespace Drivers
{
    public enum KeypadKeyCode : byte
    {
        None = 0,
        D0 = 48,
        D1 = 49,
        D2 = 50,
        D3 = 51,
        D4 = 52,
        D5 = 53,
        D6 = 54,
        D7 = 55,
        D8 = 56,
        D9 = 57,
        ENT = 13,
        ESC = 27,
        STOP = 3, // ETX
        GO = 2, // STX
        LOCK = 6, // ACK
        PWR = 24, // Cancel code
    }

    /// <summary>
    /// Connecting keypad:
    /// Connect pull-down resistors to K0-K3
    /// </summary>
    public static class Keypad4x4
    {
        public static void ReadCode(
            FPGA.OutputSignal<bool> K7,
            FPGA.OutputSignal<bool> K6,
            FPGA.OutputSignal<bool> K5,
            FPGA.OutputSignal<bool> K4,
            FPGA.InputSignal<bool> K3,
            FPGA.InputSignal<bool> K2,
            FPGA.InputSignal<bool> K1,
            FPGA.InputSignal<bool> K0,
            out byte code
            )
        {
            byte colCode = 1 << 4, row = 0, col = 0, rowSnapshot = 0;
                        
            Func<bool> k7bit = () => FPGA.Config.Bit(colCode, 3);
            Func<bool> k6bit = () => FPGA.Config.Bit(colCode, 2);
            Func<bool> k5bit = () => FPGA.Config.Bit(colCode, 1);
            Func<bool> k4bit = () => FPGA.Config.Bit(colCode, 0);

            Func<byte> rowCode = () => (byte)( (K3 << 3) | (K2 << 2) | (K1 << 1) | K0);

            FPGA.Config.Link(k7bit, K7);
            FPGA.Config.Link(k6bit, K6);
            FPGA.Config.Link(k5bit, K5);
            FPGA.Config.Link(k4bit, K4);

            FPGA.Collections.ReadOnlyDictionary<byte, byte> colLookup =
                new FPGA.Collections.ReadOnlyDictionary<byte, byte>()
                {
                    { 8, 1 },
                    { 4, 2 },
                    { 2, 3 },
                    { 1, 4 }
                };

            FPGA.Collections.ReadOnlyDictionary<byte, byte> rowLookup =
                new FPGA.Collections.ReadOnlyDictionary<byte, byte>()
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 4, 3 },
                    { 8, 4 }
                };

            while (colCode > 0)
            {
                colCode = (byte)(colCode >> 1);
                FPGA.Runtime.Delay(TimeSpan.FromMilliseconds(1));
                rowSnapshot = rowCode();
                if (rowSnapshot != 0)
                    break;
            }

            col = colLookup[colCode];

            row = rowLookup[rowSnapshot];

            if (col == 0 || row == 0)
                code = 0;
            else
                code = (byte)(col + (row-1) * 4);

            colCode = 0;
        }

        public static void ReadASCIICode(
            FPGA.OutputSignal<bool> K7,
            FPGA.OutputSignal<bool> K6,
            FPGA.OutputSignal<bool> K5,
            FPGA.OutputSignal<bool> K4,
            FPGA.InputSignal<bool> K3,
            FPGA.InputSignal<bool> K2,
            FPGA.InputSignal<bool> K1,
            FPGA.InputSignal<bool> K0,
            out KeypadKeyCode code
            )
        {
            byte rawCode = 0;
            ReadCode(K7, K6, K5, K4, K3, K2, K1, K0, out rawCode);

            FPGA.Collections.ReadOnlyDictionary<byte, Drivers.KeypadKeyCode> asciiLookup = new FPGA.Collections.ReadOnlyDictionary<byte, Drivers.KeypadKeyCode>()
            {
                { 0, Drivers.KeypadKeyCode.None },
                { 1, Drivers.KeypadKeyCode.D1 },
                { 2, Drivers.KeypadKeyCode.D2 },
                { 3, Drivers.KeypadKeyCode.D3 },
                { 4, Drivers.KeypadKeyCode.STOP },
                { 5, Drivers.KeypadKeyCode.D4 },
                { 6, Drivers.KeypadKeyCode.D5 },
                { 7, Drivers.KeypadKeyCode.D6 },
                { 8, Drivers.KeypadKeyCode.GO },
                { 9, Drivers.KeypadKeyCode.D7 },
                { 10, Drivers.KeypadKeyCode.D8 },
                { 11, Drivers.KeypadKeyCode.D9 },
                { 12, Drivers.KeypadKeyCode.LOCK },
                { 13, Drivers.KeypadKeyCode.ENT },
                { 14, Drivers.KeypadKeyCode.D0 },
                { 15, Drivers.KeypadKeyCode.ESC },
                { 16, Drivers.KeypadKeyCode.PWR },
            };

            code = asciiLookup[rawCode];
        }
    }
}
