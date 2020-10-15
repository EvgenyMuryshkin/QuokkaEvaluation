using QuSoC;
using System;
using System.Collections.Generic;
using System.Text;

namespace Increment
{
    public partial class Increment : QuSoCModule
    {
        public uint Counter => CounterModule.ReadValue;
    }
}
