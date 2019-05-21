using Quokka.Public.Tools;
using System;
using System.Collections.Generic;
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
    public abstract class SynchronizedRTLModule<TState, TInput> : CombinationalRTLModule<TInput>, IRTLModule
        where TState : class, new()
    {
        public Type StateType => typeof(TState);
        public List<MemberInfo> StateProps => RTLModuleHelper.SignalProperties(StateType);

        internal TState State = new TState();
        internal TState NextState;

        public SynchronizedRTLModule()
        {
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
