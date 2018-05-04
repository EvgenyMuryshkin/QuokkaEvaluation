using System;
using System.Collections.Generic;
using System.Text;

namespace Drivers
{
    public static class SevenSegment
    {
        public static void LinkLEDs(
            byte value,
            FPGA.Signal<bool> SEG_A,
            FPGA.Signal<bool> SEG_B,
            FPGA.Signal<bool> SEG_C,
            FPGA.Signal<bool> SEG_D,
            FPGA.Signal<bool> SEG_E,
            FPGA.Signal<bool> SEG_F,
            FPGA.Signal<bool> SEG_G,
            FPGA.Signal<bool> SEG_P
            )
        {
            Func<bool> bit0 = () => FPGA.Config.Bit(value, 0);
            Func<bool> bit1 = () => FPGA.Config.Bit(value, 1);
            Func<bool> bit2 = () => FPGA.Config.Bit(value, 2);
            Func<bool> bit3 = () => FPGA.Config.Bit(value, 3);
            Func<bool> bit4 = () => FPGA.Config.Bit(value, 4);
            Func<bool> bit5 = () => FPGA.Config.Bit(value, 5);
            Func<bool> bit6 = () => FPGA.Config.Bit(value, 6);
            Func<bool> bit7 = () => FPGA.Config.Bit(value, 7);

            FPGA.Config.Link(bit7, SEG_A);
            FPGA.Config.Link(bit6, SEG_B);
            FPGA.Config.Link(bit5, SEG_C);
            FPGA.Config.Link(bit4, SEG_D);
            FPGA.Config.Link(bit3, SEG_E);
            FPGA.Config.Link(bit2, SEG_F);
            FPGA.Config.Link(bit1, SEG_G);
            FPGA.Config.Link(bit0, SEG_P);
        }

        public static void EncodeValue(byte value, out byte encoded)
        {
            FPGA.Collections.ReadOnlyDictionary<byte, byte> lookup = new FPGA.Collections.ReadOnlyDictionary<byte, byte>()
            {
                { 0, 0xFC },
                { 1, 0x60 },
                { 2, 0xDA },
                { 3, 0xF2 },
                { 4, 0x66 },
                { 5, 0xB6 },
                { 6, 0xBE },
                { 7, 0xE0 },
                { 8, 0xFE },
                { 9, 0xF6 },
            };

            encoded = lookup[value];
        }
    }
}
