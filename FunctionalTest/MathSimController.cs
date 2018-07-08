using Drivers;
using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    [BoardConfig(Name = "NEB")]
    public static class Math_SimController
    {
        public static async Task Aggregator(
            [PassThrough]
            FPGA.InputSignal<byte> InUnsigned,
            [PassThrough]
            FPGA.InputSignal<sbyte> InSigned,
            FPGA.OutputSignal<bool> OutEqual,
            FPGA.OutputSignal<bool> OutNotEqual,
            FPGA.OutputSignal<bool> OutGreater,
            FPGA.OutputSignal<bool> OutLess,
            FPGA.OutputSignal<ushort> OutAddUnsigned,
            FPGA.OutputSignal<short> OutAddSigned,
            FPGA.OutputSignal<ushort> OutSubUnsigned,
            FPGA.OutputSignal<short> OutSubSigned,
            FPGA.OutputSignal<ushort> OutMltUnsigned,
            FPGA.OutputSignal<short> OutMltSigned
            )
        {
            // TODO direct combinational logic in non-sequential methods
            OutEqual = FPGA.Config.Compare(InUnsigned, FPGA.CompareType.Equal, InSigned);
            OutNotEqual = FPGA.Config.Compare(InUnsigned, FPGA.CompareType.NotEqual, InSigned);
            OutGreater = FPGA.Config.Compare(InUnsigned, FPGA.CompareType.Greater, InSigned);
            OutLess = FPGA.Config.Compare(InUnsigned, FPGA.CompareType.Less, InSigned);
            OutAddUnsigned = (ushort)(InUnsigned + InSigned);
            OutAddSigned = (short)(InUnsigned + InSigned);
            OutSubUnsigned = (ushort)(InUnsigned - InSigned);
            OutSubSigned = (short)(InUnsigned - InSigned);
            OutMltUnsigned = (ushort)(InUnsigned * InSigned);
            OutMltSigned = (short)(InUnsigned * InSigned);
        }
    }
}
