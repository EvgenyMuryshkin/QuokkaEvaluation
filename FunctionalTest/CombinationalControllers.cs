using FPGA.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    /*[BoardConfig(Name = "NEB")]*/[BoardConfig(Name = "Quokka")]
    public static class CombinationalControllers
    {
        public static ulong Aggregator(uint a, uint b)
        {
            BitArray arr;
            return a * b;
        }
    }
}
