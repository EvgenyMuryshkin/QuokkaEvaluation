using Drivers;
using FPGA;
using FPGA.Attributes;
using System.Threading.Tasks;

namespace FloatControllers
{
    /*[BoardConfig(Name = "NEB")]*/[BoardConfig(Name = "Quokka")]
    public static class FloatControllers_InferredFPU_Sim
    {
        public static async Task Aggregator(
            FPGA.OutputSignal<byte> OutWithDefault,
            FPGA.OutputSignal<byte> OutOperation,
            FPGA.OutputSignal<bool> OutTrigger,
            FPGA.OutputSignal<float> OutResult,
            FPGA.OutputSignal<bool> OutCompleted)
        {
            byte value = 100;
            FPGA.Config.Link(value, OutWithDefault);

            Sequential handler = () =>
            {
                FPU.FPUScope();

                float op1 = 1.23f, op2 = 10f, res = 0;
                FPGA.Config.Link(res, OutResult);

                for(byte op = 0; op < 10; op++)
                {
                    FPGA.Config.Link(op, OutOperation);
                    OutTrigger = true;
                    FloatControllersOps.TestHandler(op1, op2, op, out res);
                    OutCompleted = true;
                }
            };

            FPGA.Config.OnStartup(handler);
        }
    }
}
