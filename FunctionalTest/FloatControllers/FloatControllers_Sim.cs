using Drivers;
using FPGA;
using FPGA.Attributes;
using System.Threading.Tasks;

namespace FloatControllers
{
    /*[BoardConfig(Name = "NEB")]*/[BoardConfig(Name = "Quokka")]
    public static class FloatControllers_Sim
    {
        public static async Task Aggregator(
            FPGA.OutputSignal<float> OutResult,
            FPGA.OutputSignal<bool> Completed
            )
        {
            FPGA.Config.Suppress("W0007", OutResult);
            float f1 = 0, f2 = 0;
            FPGA.Signal<float> result = 0;
            byte fpuOp = 0;
            FPGA.Signal<bool> fpuTrigger = false, fpuCompleted = false;
            FPGA.Config.Entity<IFPU>().Op(fpuTrigger, fpuCompleted, f1, f2, fpuOp, result);

            FPGA.Config.Link(result, OutResult);

            Sequential handler = () =>
            {
                f1 = 20;
                f2 = 10;
                fpuTrigger = true;
                FPGA.Runtime.WaitForAllConditions(fpuCompleted);
                Completed = true;
            };

            FPGA.Config.OnStartup(handler);
        }
    }
}
