using Drivers;
using FPGA;
using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    public enum eShiftCommand
    {
        RightLogic,
        RightArith,
        LeftLogic,
        LeftArith,
        Rol
    }

    [BoardConfig(Name = "NEB")]
    public static class Math_Shifter
    {
        public static void ValueForCommand(eShiftCommand cmd, out byte value)
        {
            byte us = 255;
            sbyte s = -1;
            switch (cmd)
            {
                case Controllers.eShiftCommand.LeftArith:
                    {
                        value = (byte)(s << 4);
                    }
                    break;
                case Controllers.eShiftCommand.LeftLogic:
                    {
                        value = (byte)(us << 4);
                    }
                    break;
                case Controllers.eShiftCommand.RightArith:
                    {
                        value = (byte)(s >> 4);
                    }
                    break;
                case Controllers.eShiftCommand.RightLogic:
                    {
                        value = (byte)(us >> 4);
                    }
                    break;
                case Controllers.eShiftCommand.Rol:
                    {
                        value = FPGA.Runtime.Rol(4, us);
                    }
                    break;
                default:
                    value = 0;
                    break;
            }
        }

        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            Controllers.eShiftCommand cmd = 0;

            Sequential readHandler = () =>
            {
                byte data = 0;
                UART.Read(115200, RXD, out data);
                cmd = (eShiftCommand)data;
            };
            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, readHandler);

            byte write = 0;
            FPGA.Signal<bool> dataWritten = false;

            Sequential writeHandler = () =>
            {
                UART.Write(115200, write, TXD);
                dataWritten = true;
            };
            FPGA.Config.OnRegisterWritten(write, writeHandler);

            Sequential cmdHandler = () =>
            {
                byte result = 0;
                ValueForCommand(cmd, out result);

                write = result;
                FPGA.Runtime.WaitForAllConditions(dataWritten);
            };
            FPGA.Config.OnRegisterWritten(cmd, cmdHandler);

        }
    }
}
