using QuSoC;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fibonacci
{
    public partial class Fibonacci : QuSoCModule
    {
        public Fibonacci() : base(AppLocation()) { }
    }
}
