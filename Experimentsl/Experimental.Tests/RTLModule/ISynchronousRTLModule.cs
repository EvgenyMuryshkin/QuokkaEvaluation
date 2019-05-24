using System;
using System.Collections.Generic;
using System.Reflection;

namespace QuokkaTests.Experimental
{
    public interface ISynchronousRTLModule : ICombinationalRTLModule
    {
        Type StateType { get; }
        List<MemberInfo> StateProps { get; }
    }
}
