using Quokka.RTL;

namespace RTL.Modules
{
    public class StateMembersInputs
    {
        public bool Toggle;
    }

    public class StateMembersState
    {
        public bool BoolValue;
    }

    public class StateMembersModule : RTLSynchronousModule<StateMembersInputs, StateMembersState>
    {
        // public data points
        public bool BoolValue => State.BoolValue;

        protected override void OnStage()
        {
            if (Inputs.Toggle)
            {
                NextState.BoolValue = !State.BoolValue;
            }
        }
    }
}
