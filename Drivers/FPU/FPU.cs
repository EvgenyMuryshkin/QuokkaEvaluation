using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drivers
{
    public static class FPU
    {
        #region FPU entry points

        [Inlined]
        public static void FPUScopeNoSync()
        {
            internalFPUOp();
            internalFPUCast();
        }

        [Inlined]
        public static void FPUScope()
        {
            internalFPUCast();
            internalFPUCastSync();

            internalFPUOp();
            internalFPUOpSync();
        }

        [Inlined]
        public static void FPUCast()
        {
            internalFPUCast();
            internalFPUCastSync();
        }

        [Inlined]
        public static void FPUCastNoSync()
        {
            internalFPUCast();
        }
        #endregion

        #region FPU mapping
        [Inlined]
        static void internalFloatToIntCast()
        {
            FPGA.Signal<bool> floatToIntCastTrigger = false, floatToIntCastCompleted = false;
            float floatToIntCastSource = 0;
            FPGA.Signal<long> floatToIntCastResult = 0;

            FPGA.Config.Entity<IFloatToIntCast>().Op(
                floatToIntCastTrigger,
                floatToIntCastCompleted,
                floatToIntCastSource,
                floatToIntCastResult);
        }

        [Inlined]
        static void internalIntToFloatCast()
        {
            bool intToFloatIsSigned = false;
            FPGA.Signal<bool> intToFloatCastTrigger = false, intToFloatCastCompleted = false;
            ulong intToFloatCastSource = 0;
            FPGA.Signal<float> intToFloatCastResult = 0;

            FPGA.Config.Entity<IIntToFloatCast>().Op(
                intToFloatCastTrigger,
                intToFloatIsSigned,
                intToFloatCastCompleted,
                intToFloatCastSource,
                intToFloatCastResult);
        }

        [Inlined]
        static void internalFPUCast()
        {
            internalFloatToIntCast();
            internalIntToFloatCast();
        }

        [Inlined]
        static void internalFPUOp()
        {
            float fpuOp1 = 0, fpuOp2 = 0;
            FPGA.Signal<float> fpuResult = 0;
            byte fpuOp = 0;
            FPGA.Signal<bool> fpuTrigger = false, fpuCompleted = false;
            FPGA.Config.Entity<IFPU>().Op(fpuTrigger, fpuCompleted, fpuOp1, fpuOp2, fpuOp, fpuResult);
        }
        #endregion

        #region FPU Sync
        [Inlined]
        static void internalFPUOpSync()
        {
            object fpuLock = new object();
            FPGA.Const<bool> fpuSync = true;
        }

        [Inlined]
        static void internalFPUCastSync()
        {
            FPGA.Const<bool> floatToIntCastSync = true;
            object floatToIntCastLock = new object();

            FPGA.Const<bool> intToFloatCastSync = true;
            object intToFloatCastLock = new object();
        }
        #endregion
    }
}
