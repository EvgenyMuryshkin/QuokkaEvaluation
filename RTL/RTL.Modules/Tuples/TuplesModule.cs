using Quokka.RTL;

namespace RTL.Modules
{
    public class TuplesModuleInputs
    {
        public bool Value1 { get; set; }
        public bool Value2 { get; set; }
    }

    public class TuplesModule : RTLCombinationalModule<TuplesModuleInputs>
    {
        (bool, bool) Logic
        {
            get
            {
                var same = Inputs.Value1 == Inputs.Value2;
                var diff = Inputs.Value1 != Inputs.Value2;

                return (same, diff);
            }
        }

        public bool Same => Logic.Item1;
        public bool Diff => Logic.Item2;
    }
}

