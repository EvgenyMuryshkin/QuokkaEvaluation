using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTL.Modules
{
    public class Data
    {
        public byte Op1 = 10;
        public byte Op2 = 20;
    }

    public class StructsCombinationalModuleInputs
    {
        public Data In = new Data();
    }

    public class StructsCombinationalModule : RTLCombinationalModule<StructsCombinationalModuleInputs>
    {
        Data internalDirect => Inputs.In;
        byte internalSum => (byte)(Inputs.In.Op1 + Inputs.In.Op2);
        public byte Op1 => Inputs.In.Op1;
        public byte Sum => internalSum;
        
        public Data OutDirect => Inputs.In;
        public Data OutInternal => internalDirect;
        public Data OutSwapped => new Data() { Op1 = Inputs.In.Op2, Op2 = Inputs.In.Op1 };
        public Data OutMath => new Data()
        {
            Op1 = (byte)(Inputs.In.Op1 + Inputs.In.Op2),
            Op2 = (byte)(Inputs.In.Op1 - Inputs.In.Op2)
        };

        public Data Default1 => new Data() { Op1 = 42 };
        public Data Default2 => new Data();
    }
}
