using Quokka.Public.Tools;
using Quokka.VCD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QuokkaTests.Experimental
{
    public abstract class CombinationalRTLModule<TInput> : ICombinationalRTLModule
        where TInput : new()
    {
        public bool IsTraceEnabled { get; set; }
        public Type InputsType { get; } = typeof(TInput);
        public List<MemberInfo> InputProps { get; }
        public List<MemberInfo> OutputProps { get; }
        public List<MemberInfo> ModuleProps { get; }
        public List<ICombinationalRTLModule> Modules { get; }

        public CombinationalRTLModule()
        {
            InputProps = RTLModuleHelper.SignalProperties(InputsType);
            OutputProps = RTLModuleHelper.SignalProperties(GetType());
            ModuleProps = RTLModuleHelper.ModuleProperties(GetType());
            Modules = ModuleProps.Select(m => (ICombinationalRTLModule)m.GetValue(this)).ToList();
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

        protected virtual void PopulateSelfScope(VCDScope scope)
        {
            var inputsScope = new VCDScope()
            {
                Name = "Inputs",
                Variables = InputProps.Select(p => new VCDVariable()
                {
                    Name = p.Name,
                    Size = 1,
                }).ToList()
            };
            var outputsScope = new VCDScope()
            {
                Name = "Outputs",
                Variables = OutputProps.Select(p => new VCDVariable()
                {
                    Name = p.Name,
                    Size = 1,
                }).ToList()
            };

            scope.Scopes.AddRange(new[] { inputsScope, outputsScope });
        }

        protected virtual void PopulateChildrenScopes(VCDScope scope)
        {
            scope.Scopes.AddRange(ModuleProps.Select(child => ((ICombinationalRTLModule)child.GetValue(this)).CreateScope(child.Name)));
        }

        public virtual VCDScope CreateScope(string prefix)
        {
            var scope = new VCDScope()
            {
                Name = prefix,
            };

            PopulateSelfScope(scope);
            PopulateChildrenScopes(scope);

            return scope;
        }

        public virtual void PopulateSnapshot(VCDSignalsSnapshot snapshot)
        {
            var inputs = snapshot.Scope("Inputs");
            foreach (var prop in InputProps)
            {
                inputs[prop.Name] = prop.GetValue(Inputs);
            }

            var outputs = snapshot.Scope("Outputs");
            foreach (var prop in OutputProps)
            {
                outputs[prop.Name] = prop.GetValue(this);
            }

            foreach (var m in ModuleProps)
            {
                var module = (ICombinationalRTLModule)m.GetValue(this);
                var moduleScope = snapshot.Scope(m.Name);

                module.PopulateSnapshot(moduleScope);
            }
        }
    }
}
