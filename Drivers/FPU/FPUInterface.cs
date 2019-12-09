using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drivers
{
    public enum eFPUOp : byte
    {
        Add,
        Subtract,
        Multiply,
        Divide,
        // Sqrt // not supported by verilog FPU 
    }

    public interface IFPU
    {
        void Op(
            [ModuleInput]
            FPGA.Signal<bool> Trigger,
            [ModuleOutput]
            FPGA.Signal<bool> Completed,
            [ModuleInput]
            float Lhs,
            [ModuleInput]
            float Rhs,
            [ModuleInput]
            byte Op,
            [ModuleOutput]
            FPGA.Signal<float> Result
            );
    }

    public interface IFloatToIntCast
    {
        void Op(
            [ModuleInput]   FPGA.Signal<bool> Trigger,
            [ModuleOutput]  FPGA.Signal<bool> Completed,
            [ModuleInput]   float Source,
            [ModuleOutput]  FPGA.Signal<long> Result
            );
    }

    public interface IIntToFloatCast
    {
        void Op(
            [ModuleInput]   FPGA.Signal<bool> Trigger,
            [ModuleInput]   FPGA.Signal<bool> IsSigned,
            [ModuleOutput]  FPGA.Signal<bool> Completed,
            [ModuleInput]   ulong Source,
            [ModuleOutput]  FPGA.Signal<float> Result
            );
    }
}
