using Drivers;
using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    [BoardConfig(Name = "NEB")]
    public static class Misc_SEG7Controller
    {
        public static async Task Aggregator(
            FPGA.OutputSignal<bool> Bank2,
            FPGA.OutputSignal<bool> SEG_A,
            FPGA.OutputSignal<bool> SEG_B,
            FPGA.OutputSignal<bool> SEG_C,
            FPGA.OutputSignal<bool> SEG_D,
            FPGA.OutputSignal<bool> SEG_E,
            FPGA.OutputSignal<bool> SEG_F,
            FPGA.OutputSignal<bool> SEG_G,
            FPGA.OutputSignal<bool> SEG_P)
        {
            QuokkaBoard.OutputBank(Bank2);

            byte counter = 0;
            byte segmentValue = 0;

            SevenSegment.LinkLEDs(
                segmentValue,
                SEG_A,
                SEG_B,
                SEG_C,
                SEG_D,
                SEG_E,
                SEG_F,
                SEG_G,
                SEG_P);

            BlastOff(out segmentValue);
        }

        public static void BlastOff(out byte segmentData)
        {
            byte currentaData = 0;
            FPGA.Config.Link(currentaData, out segmentData);

            byte counter = 0;
            byte[] data = new byte[]
            {
                0xF2,// '3'
                0xDA,// '2'
                0x60,// '1'
                0x3E,// 'b'
                0x3C,// 'l'
                0xEE,// 'a'
                0xB6,// 's'
                0x1E,// 't'
                0x00,// ' '
                0xFC,// 'o'
                0x8E,// 'f'
                0x8E,// 'f'
                0x00,// ' '
            };

            Action segmentHandler = () =>
            {
                currentaData = data[counter];
                counter = (byte)(counter < data.Length - 1 ? counter + 1 : 0);

                FPGA.Runtime.Delay(TimeSpan.FromMilliseconds(800));
                currentaData = 0;
            };

            FPGA.Config.OnTimer(TimeSpan.FromSeconds(1), segmentHandler);
        }
    }
}
