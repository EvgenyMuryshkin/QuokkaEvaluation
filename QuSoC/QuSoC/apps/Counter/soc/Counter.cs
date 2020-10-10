using QuSoC;
using System;
using System.Collections.Generic;
using System.Text;

namespace Counter
{
    public class Counter : QuSoCModule
    {
        public Counter() : base(AppLocation()) { }
    }
}
