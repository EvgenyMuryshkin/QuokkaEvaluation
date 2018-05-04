/*
 * Copyright: Evgeny Muryshkin evmuryshkin@gmail.com
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace Drivers
{
    public static class MCP49X2
    {
        public static void Write(
            ushort ChannelA,
            ushort ChannelB,
            FPGA.OutputSignal<bool> NCS,
            FPGA.OutputSignal<bool> SCK,
            FPGA.OutputSignal<bool> SDI
            )
        {
            ushort[] buff = new ushort[2];
            ushort[] words = new ushort[2];

            buff[0] = ChannelA;
            buff[1] = ChannelB;

            for (int i = 0; i < 2; i++)
            {
                byte cmd = (byte)((i << 3) | 3); // channel | unbuffered | no gain | no shutdown
                ushort val = 0;
                val = buff[i];
                ushort channelWord = (ushort)((cmd << 12) | (val >> 4));
                words[i] = channelWord;
            }

            for (int j = 0; j < 2; j++)
            {
                ushort channelData = 0;
                channelData = buff[j];
                SPI.Write<ushort>(channelData, NCS, SCK, SDI);
            }
        }
    }
}
