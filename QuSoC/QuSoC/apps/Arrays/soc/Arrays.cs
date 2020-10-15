using QuSoC;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arrays
{
    public partial class Arrays : QuSoCModule
    {
        public uint Counter => CounterModule.ReadValue;
    }
}
