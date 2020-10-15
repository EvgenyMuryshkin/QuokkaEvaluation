using QuSoC;
using System;
using System.Collections.Generic;
using System.Text;

namespace MemBlock
{
    public partial class MemBlock : QuSoCModule
    {
        public uint Counter => CounterModule.ReadValue;
    }
}
