using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Text;

namespace RTL.Modules
{
    public abstract class RTLCombinationalModule<TInputs> : DefaultRTLCombinationalModule<TInputs> 
        where TInputs : new()
    {

    }

    public abstract class RTLSynchronousModule<TInputs, TState> : DefaultRTLSynchronousModule<TInputs, TState> 
        where TInputs : new() 
        where TState : new()
    {

    }
}
