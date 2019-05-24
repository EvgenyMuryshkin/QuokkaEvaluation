using Quokka.VCD;
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

        VCDScope CreateScope(string prefix);
        void PopulateSnapshot(VCDSignalsSnapshot snapshot);
    }
}
