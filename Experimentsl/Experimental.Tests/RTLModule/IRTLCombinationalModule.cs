using Quokka.VCD;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Quokka.RTL
{
    public interface IRTLCombinationalModule
    {
        Type InputsType { get; }

        List<MemberInfo> InputProps { get; }
        List<MemberInfo> OutputProps { get; }
        List<MemberInfo> ModuleProps { get; }
        List<IRTLCombinationalModule> Modules { get; }

        bool Stage(int iteration);
        void Commit();

        void PopulateSnapshot(VCDSignalsSnapshot snapshot);
        void Setup();
    }
}
