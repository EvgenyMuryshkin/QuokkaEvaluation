using Quokka.Public.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QuokkaTests.Experimental
{
    public abstract class CombinationalRTLModule<TInput> : ICombinationalRTLModule
    {
        public bool IsTraceEnabled { get; set; }
        public Type InputsType => typeof(TInput);
        public List<MemberInfo> InputProps => RTLModuleHelper.SignalProperties(InputsType);
        public List<MemberInfo> OutputProps => RTLModuleHelper.SignalProperties(GetType());
        public List<MemberInfo> ModuleProps => RTLModuleHelper.ModuleProperties(GetType());

        public List<ICombinationalRTLModule> Modules => ModuleProps.Select(m => (ICombinationalRTLModule) m.GetValue(this)).ToList();

        internal TInput Inputs;
        internal Func<TInput> InputsFactory;

        public virtual void Schedule(Func<TInput> inputsFactory)
        {
            InputsFactory = inputsFactory;
        }

        public virtual bool Stage(int iteration)
        {
            bool selfModified = true, childrenModified = false;
            var nextInputs = InputsFactory();
            if (iteration != 0)
            {
                // TODO: faster inputs comparison
                var prevJson = QuokkaJson.SerializeObject(Inputs);
                var nextJson = QuokkaJson.SerializeObject(nextInputs);

                // check if given set of inputs was already processed on previous iteration
                if (prevJson == nextJson)
                    return selfModified = false;
            }

            Inputs = nextInputs;

            foreach (var child in Modules)
            {
                childrenModified |= child.Stage(iteration);
            }

            return selfModified | childrenModified;
        }

        public virtual void Commit()
        {
            foreach (var child in Modules)
            {
                child.Commit();
            }
        }

        public virtual void PopulateSnapshot(string prefix, Dictionary<string, object> snapshot)
        {
            foreach (var prop in OutputProps)
            {
                var propName = $"{prefix}_Outputs_{prop.Name}";
                snapshot[propName] = prop.GetValue(this);
            }

            foreach (var prop in InputProps)
            {
                var propName = $"{prefix}_Inputs_{prop.Name}";
                snapshot[propName] = prop.GetValue(Inputs);
            }

            foreach (var m in ModuleProps)
            {
                var module = (ICombinationalRTLModule)m.GetValue(this);
                module.PopulateSnapshot($"{prefix}_{m.Name}", snapshot);
            }
        }
    }
}
