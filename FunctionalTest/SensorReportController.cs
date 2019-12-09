using Drivers;
using FPGA;
using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    public struct SRDTO
    {
        // channel 1
        public ushort C1;

        // channel 2
        public ushort C2;
    }

    /*[BoardConfig(Name = "NEB")]*/[BoardConfig(Name = "Quokka")]
    public static class Misc_SensorReportController
    {
        public static async Task Aggregator(
            // UART signals
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD,

            // ADC signals
            FPGA.OutputSignal<bool> ADC1NCS,
            FPGA.OutputSignal<bool> ADC1SLCK,
            FPGA.OutputSignal<bool> ADC1DIN,
            FPGA.InputSignal<bool> ADC1DOUT
            )
        {
            Sequential handler = () =>
            {
                ushort adcChannel1Value = 0, adcChannel2Value = 0;
                ADC102S021.Read(
                    out adcChannel1Value, 
                    out adcChannel2Value, 
                    ADC1NCS, 
                    ADC1SLCK, 
                    ADC1DIN, 
                    ADC1DOUT);

                Controllers.SRDTO response = new Controllers.SRDTO();
                response.C1 = adcChannel1Value;
                response.C2 = adcChannel2Value;

                Drivers.JSON.SerializeToUART<Controllers.SRDTO>(ref response, TXD);
            };

            FPGA.Config.OnTimer(TimeSpan.FromSeconds(1), handler);
        }
    }
}
