using System;
using System.Collections.Generic;
using System.Reflection;

namespace Quokka.RTL
{
    public interface IRTLSynchronousModule : IRTLCombinationalModule
    {
        Type StateType { get; }
        List<MemberInfo> StateProps { get; }
    }
}
