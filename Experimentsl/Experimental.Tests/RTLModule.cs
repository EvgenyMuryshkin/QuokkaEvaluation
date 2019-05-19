using Quokka.Public.Tools;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QuokkaTests.Experimental
{
    public interface IRTLModule
    {
        void Stage();
        void Commit();
    }

    /// <summary>
    /// Base class for hardware state machine module, will be in toolkit
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TState"></typeparam>
    public abstract class RTLModule<TState, TInput> : IRTLModule
        where TState : class, new()
    {
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

        public void Stage()
        {
            NextState = QuokkaJson.Copy(State);
            Inputs = InputsFactory();
            OnStage();
        }

        public void Commit()
        {
            State = NextState;
            NextState = null;
        }

        public void Cycle(TInput inputs)
        {
            Schedule(() => inputs);
            Stage();
            Commit();
        }
    }
}
