using Drivers;
using FPGA.Attributes;
using FPGA.Optimizers;
using System.Reflection;
using System.Threading.Tasks;

namespace FPGA.OrbitalCalc.Controllers
{
    [BoardConfig(Name = "Quokka")]
    public static class VEscController
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
    )
        {
            Sequential handler = () =>
            {
                FPU.FPUScopeNoSync();

                while (true)
                {
                    FPGA.Optimizations.AddOptimizer<DefaultOptimizer>();

                    const uint baud = 115200;
                    UART.ReadFloat(baud, RXD, out float mass);
                    UART.ReadFloat(baud, RXD, out float radius);

                    var vEsc = FPGAOrbitalCalc.VEsc(mass, radius);

                    UART.WriteFloat(baud, vEsc, TXD);
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }

    [BoardConfig(Name = "Quokka")]
    public static class VOrbitController
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
        )
        {
            Sequential handler = () =>
            {
                FPU.FPUScopeNoSync();

                while (true)
                {
                    FPGA.Optimizations.AddOptimizer<DefaultOptimizer>();

                    const uint baud = 115200;
                    UART.ReadFloat(baud, RXD, out float mass);
                    UART.ReadFloat(baud, RXD, out float radius);

                    var vOrbit = FPGAOrbitalCalc.VOrbit(mass, radius);

                    UART.WriteFloat(baud, vOrbit, TXD);
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }

    [BoardConfig(Name = "Quokka")]
    public static class TOrbitController
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
        )
        {
            Sequential handler = () =>
            {
                FPU.FPUScopeNoSync();

                while (true)
                {
                    FPGA.Optimizations.AddOptimizer<DefaultOptimizer>();

                    const uint baud = 115200;
                    UART.ReadFloat(baud, RXD, out float mass);
                    UART.ReadFloat(baud, RXD, out float radius);

                    var vOrbit = FPGAOrbitalCalc.TOrbit(mass, radius);

                    UART.WriteFloat(baud, vOrbit, TXD);
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }

    [BoardConfig(Name = "Quokka")]
    public static class DeltaVInnerOrbitController
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
        )
        {
            Sequential handler = () =>
            {
                FPU.FPUScopeNoSync();

                while (true)
                {
                    FPGA.Optimizations.AddOptimizer<DefaultOptimizer>();

                    const uint baud = 115200;
                    UART.ReadFloat(baud, RXD, out float mass);
                    UART.ReadFloat(baud, RXD, out float innerRadius);
                    UART.ReadFloat(baud, RXD, out float outerRadius);

                    var deltaV = FPGAOrbitalCalc.DeltaVInnerOrbit(mass, innerRadius, outerRadius);

                    UART.WriteFloat(baud, deltaV, TXD);
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }

    [BoardConfig(Name = "Quokka")]
    public static class DeltaVOuterOrbitController
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
        )
        {
            Sequential handler = () =>
            {
                FPU.FPUScopeNoSync();

                while (true)
                {
                    FPGA.Optimizations.AddOptimizer<DefaultOptimizer>();

                    const uint baud = 115200;
                    UART.ReadFloat(baud, RXD, out float mass);
                    UART.ReadFloat(baud, RXD, out float innerRadius);
                    UART.ReadFloat(baud, RXD, out float outerRadius);

                    var deltaV = FPGAOrbitalCalc.DeltaVOuterOrbit(mass, innerRadius, outerRadius);

                    UART.WriteFloat(baud, deltaV, TXD);
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }

    [BoardConfig(Name = "Quokka")]
    public static class DeltaVInclinationOrbitOptimizedController
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
        )
        {
            Sequential handler = () =>
            {
                FPU.FPUScopeNoSync();

                while (true)
                {
                    FPGA.Optimizations.AddOptimizer<DefaultOptimizer>();
                    const uint baud = 115200;
                    float[] buff = new float[4];
                    for (byte i = 0; i < 4; i++)
                    {
                        UART.ReadFloat(baud, RXD, out float tmp);
                        buff[i] = tmp;
                    }

                    var deltaV = FPGAOrbitalCalc.DeltaVInclinationOrbitOptimized(
                        buff[0], 
                        buff[1], 
                        buff[2],
                        buff[3]
                        );

                    UART.WriteFloat(baud, deltaV, TXD);
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }

    [BoardConfig(Name = "Quokka")]
    public static class DeltaVInclinationOrbitController
    {
        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
        )
        {
            Sequential handler = () =>
            {
                FPU.FPUScopeNoSync();

                while (true)
                {
                    FPGA.Optimizations.AddOptimizer<DefaultOptimizer>();
                    const uint baud = 115200;
                    float[] buff = new float[4];
                    for (byte i = 0; i < 4; i++)
                    {
                        UART.ReadFloat(baud, RXD, out float tmp);
                        buff[i] = tmp;
                    }

                    var deltaV = FPGAOrbitalCalc.DeltaVInclinationOrbit(
                        buff[0],
                        buff[1],
                        buff[2],
                        buff[3]
                        );

                    UART.WriteFloat(baud, deltaV, TXD);
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }
}
