using Quokka.Public.Tools;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QuokkaTests.Experimental
{
    public class MaxStageIterationReachedException : Exception
    {
    }

    public interface IRTLModule
    {
        Type StateType { get; }
        Type InputsType { get; }

        List<MemberInfo> StateProps { get; }
        List<MemberInfo> InputProps { get; }
        List<MemberInfo> OutputProps { get; }

        bool IsTraceEnabled { get; set; }
        bool Stage(int iteration);
        void Commit();
    }

    public class RTLModuleTrace<TState, TInput>
    {
    }

    /// <summary>
    /// Base class for hardware state machine module, will be in toolkit
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TState"></typeparam>
    public abstract class RTLModule<TState, TInput> : IRTLModule
        where TState : class, new()
    {
        public bool IsTraceEnabled { get; set; }

        public Type StateType => typeof(TState);
        public Type InputsType => typeof(TInput);

        public List<MemberInfo> StateProps => RTLModuleHelper.SignalProperties(StateType);
        public List<MemberInfo> InputProps => RTLModuleHelper.SignalProperties(InputsType);
        public List<MemberInfo> OutputProps => RTLModuleHelper.SignalProperties(GetType());

        internal TState State = new TState();
        internal TState NextState;
        internal TInput Inputs;
        internal Func<TInput> InputsFactory;

        public RTLModule()
        {
        }

        protected abstract void OnStage();

        public void Schedule(Func<TInput> inputsFactory)
        {
            InputsFactory = inputsFactory;
        }

        protected TInput previousStageInputs;

        public bool Stage(int iteration)
        {
            if (iteration > 0)
                previousStageInputs = Inputs;

            NextState = QuokkaJson.Copy(State);

            Inputs = InputsFactory();
            /*
            var prevJson = QuokkaJson.SerializeObject(previousStageInputs);
            var nextJson = QuokkaJson.SerializeObject(Inputs);

            // check if given set of inputs was already processed on prevoous iteration
            if (prevJson == nextJson)
                return false;
            */
            //NextState = QuokkaJson.Copy(State);
            OnStage();
            // inducated processed inputs
            return true;
        }

        public void Commit()
        {
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
