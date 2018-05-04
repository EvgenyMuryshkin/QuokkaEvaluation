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
        Sqrt
    }

    public interface IFPU
    {
        void Op(FPGA.Signal<bool> Trigger, FPGA.Signal<bool> Completed, float Lhs, float Rhs, byte Op, FPGA.Signal<float> result);
    }

    public static class FPU
    {
        [Inlined]
        public static void FPUScope()
        {
            float fpuOp1 = 0, fpuOp2 = 0;
            FPGA.Signal<float> fpuResult = 0;
            byte fpuOp = 0;
            FPGA.Signal<bool> fpuTrigger = false, fpuCompleted = false;
            FPGA.Config.Entity<IFPU>().Op(fpuTrigger, fpuCompleted, fpuOp1, fpuOp2, fpuOp, fpuResult);
            object fpuLock = new object();
        }
    }
}
