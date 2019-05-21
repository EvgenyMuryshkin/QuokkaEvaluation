using System;
using System.Collections.Generic;
using System.Reflection;

namespace QuokkaTests.Experimental
{
    public interface ICombinationalRTLModule
    {
        Type InputsType { get; }

        List<MemberInfo> InputProps { get; }
        List<MemberInfo> OutputProps { get; }
        List<MemberInfo> ModuleProps { get; }
        List<ICombinationalRTLModule> Modules { get; }

        bool IsTraceEnabled { get; set; }

        bool Stage(int iteration);
        void Commit();

        void PopulateSnapshot(string prefix, Dictionary<string, object> snapshot);
    }

    public interface IRTLModule : ICombinationalRTLModule
    {
        Type StateType { get; }
        List<MemberInfo> StateProps { get; }
    }
}
