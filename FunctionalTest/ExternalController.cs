using Drivers;
using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    public interface IAbstractExternalPackage
    {
        void ExternalEntity(
            [ModuleInput]
            FPGA.Signal<byte> InSignal, 
            [ModuleOutput]
            FPGA.Signal<byte> OutSignal,
            [ModuleInput]
            FPGA.Signal<bool> InTrigger,
            [ModuleOutput]
            FPGA.Signal<bool> OutReady);
    }

    public interface IConcreteExternalPackage1 : Controllers.IAbstractExternalPackage { }

    public interface IConcreteExternalPackage2 : Controllers.IAbstractExternalPackage { }

    [BoardConfig(Name = "NEB")]
    public static class External_ExistingEntity
    {
        static void NestedLevel1<T>(byte dataIn, out byte dataOut) where T : IAbstractExternalPackage
        {
            FPGA.Signal<byte> externalInput = new FPGA.Signal<byte>();
            FPGA.Config.Link(dataIn, out externalInput);

            FPGA.Signal<byte> externalOutput = new FPGA.Signal<byte>();
            FPGA.Signal<bool> externalTrigger = new FPGA.Signal<bool>();
            FPGA.Signal<bool> externalReady = new FPGA.Signal<bool>();

            FPGA.Config.Entity<T>().ExternalEntity(externalInput, externalOutput, externalTrigger, externalReady);
            externalTrigger = true;

            FPGA.Runtime.WaitForAllConditions(externalReady);

            dataOut = externalOutput;
        }

        static void NestedLevel2<T>(byte dataIn, out byte dataOut) where T : IAbstractExternalPackage
        {
            NestedLevel1<T>(dataIn, out dataOut);
        }

        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD)
        {
            byte data = 0;
            FPGA.Signal<byte> externalInput = new FPGA.Signal<byte>();
            FPGA.Config.Link(data, out externalInput);

            FPGA.Signal<byte> externalOutput = new FPGA.Signal<byte>();
            FPGA.Signal<bool> externalTrigger = new FPGA.Signal<bool>();
            FPGA.Signal<bool> externalReady = new FPGA.Signal<bool>();

            Action handler = () =>
            {
                UART.Read(115200, RXD, out data);

                for(byte i = 0; i < 3; i++)
                {
                    byte result = 0;

                    switch (i)
                    {
                        case 0:
                            FPGA.Config.Entity<IConcreteExternalPackage1>().ExternalEntity(externalInput, externalOutput, externalTrigger, externalReady);

                            externalTrigger = true;

                            FPGA.Runtime.WaitForAllConditions(externalReady);

                            result = externalOutput;
                            break;
                        case 1:
                            NestedLevel1<IConcreteExternalPackage1>(data, out result);
                            break;
                        case 2:
                            NestedLevel2<IConcreteExternalPackage2>(40, out result);
                            break;
                    }

                    UART.Write(115200, result, TXD);
                }
            };

            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, handler);
        }
    }
}
