using Quokka.Public.Tools;
using Quokka.VCD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Quokka.RTL
{
    public abstract class RTLCombinationalModule<TInput> : IRTLCombinationalModule
        where TInput : new()
    {
        public Type InputsType { get; } = typeof(TInput);
        public List<MemberInfo> InputProps { get; }
        public List<MemberInfo> OutputProps { get; }
        public List<MemberInfo> ModuleProps { get; }
        public List<IRTLCombinationalModule> Modules { get; }

        public RTLCombinationalModule()
        {
            InputProps = RTLModuleHelper.SignalProperties(InputsType);
            OutputProps = RTLModuleHelper.SignalProperties(GetType());
            ModuleProps = RTLModuleHelper.ModuleProperties(GetType());
            Modules = ModuleProps.Select(m => (IRTLCombinationalModule)m.GetValue(this)).ToList();
        }

        public void Setup()
        {
            Schedule(() => new TInput());
        }

        internal TInput Inputs = new TInput();
        internal Func<TInput> InputsFactory;

        public virtual void Schedule(Func<TInput> inputsFactory)
        {
            InputsFactory = inputsFactory;
        }

        protected virtual bool ShouldStage(TInput nextInputs)
        {
            // check if given set of inputs was already processed on previous iteration
            foreach (var prop in InputProps)
            {
                var currentValue = prop.GetValue(Inputs);
                var nextVaue = prop.GetValue(nextInputs);

                if (!currentValue.Equals(nextVaue))
                    return true;
            }

            return false;
        }

        public virtual bool Stage(int iteration)
        {
            var nextInputs = InputsFactory();

            bool selfModified = iteration == 0 || ShouldStage(nextInputs);
            bool childrenModified = false;

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

        protected virtual int SizeOf(object value)
        {
            switch (value)
            {
                case Enum v:
                    return RTLModuleHelper.SizeOfEnum(value.GetType());
                default:
                    return VCDTools.SizeOf(value);
            }
        }

        protected virtual IEnumerable<VCDVariable> ToVCDVariables(MemberInfo memberInfo, object value)
        {
            switch(value)
            {
                case Enum v:
                    return new[]
                    {
                        new VCDVariable($"{memberInfo.Name}ToString", value.ToString(), SizeOf("")),
                        new VCDVariable(memberInfo.Name, value, SizeOf(value))
                    };
                default:
                    return new[]
                    {
                        new VCDVariable(memberInfo.Name, value, SizeOf(value))
                    };
            }
        }

        public virtual void PopulateSnapshot(VCDSignalsSnapshot snapshot)
        {
            var inputs = snapshot.Scope("Inputs");
            foreach (var prop in InputProps)
            {
                var value = prop.GetValue(Inputs);
                inputs.SetVariables(ToVCDVariables(prop, value));
            }

            var outputs = snapshot.Scope("Outputs");
            foreach (var prop in OutputProps)
            {
                var value = prop.GetValue(this);
                outputs.SetVariables(ToVCDVariables(prop, value));
            }

            foreach (var m in ModuleProps)
            {
                var module = (IRTLCombinationalModule)m.GetValue(this);
                var moduleScope = snapshot.Scope(m.Name);

                module.PopulateSnapshot(moduleScope);
            }
        }
    }
}
