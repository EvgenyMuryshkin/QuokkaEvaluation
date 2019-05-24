using Quokka.Public.Tools;
using Quokka.VCD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QuokkaTests.Experimental
{
    public class RTLModuleTrace<TState, TInput>
    {
    }

    /// <summary>
    /// Base class for hardware state machine module, will be in toolkit
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TState"></typeparam>
    public abstract class SynchronousRTLModule<TState, TInput> : CombinationalRTLModule<TInput>, ISynchronousRTLModule
        where TInput : new()
        where TState : class, new()
    {
        public Type StateType { get; } = typeof(TState);
        public List<MemberInfo> StateProps { get; } = RTLModuleHelper.SignalProperties(typeof(TState));

        internal TState State = new TState();
        internal TState NextState = new TState();

        public SynchronousRTLModule()
        {
        }

        protected override void PopulateSelfScope(VCDScope scope)
        {
            base.PopulateSelfScope(scope);

            scope.Scopes.Add(new VCDScope()
            {
                Name = "State",
                Variables = StateProps.Select(p => new VCDVariable()
                {
                    Name = p.Name,
                    Size = 1,
                }).ToList()
            });

            scope.Scopes.Add(new VCDScope()
            {
                Name = "NextState",
                Variables = StateProps.Select(p => new VCDVariable()
                {
                    Name = p.Name,
                    Size = 1,
                }).ToList()
            });
        }

        public override void PopulateSnapshot(VCDSignalsSnapshot snapshot)
        {
            base.PopulateSnapshot(snapshot);

            var state = snapshot.Scope("State");
            foreach (var prop in StateProps)
            {
                state[prop.Name] = prop.GetValue(State);
            }

            var nextState = snapshot.Scope("NextState");
            foreach (var prop in StateProps)
            {
                nextState[prop.Name] = prop.GetValue(NextState);
            }
        }

        protected abstract void OnStage();

        public override bool Stage(int iteration)
        {
            if (!base.Stage(iteration))
                return false;

            NextState = QuokkaJson.Copy(State);
            OnStage();

            // indicated processed inputs
            return true;
        }

        public override void Commit()
        {
            base.Commit();
            State = NextState;
            NextState = null;
        }

        public void Cycle(TInput inputs)
        {
            Schedule(() => inputs);
            Stage(0);
            Commit();
        }
    }
}
