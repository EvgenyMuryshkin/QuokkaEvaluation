using Quokka.RTL;
using System;
using System.Linq;

namespace RTL.Modules
{
    public class ModuleSignalsMuxModuleInputs
    {
        public RTLBitArray Addr { get; set; } = new RTLBitArray().Resized(2);
        public bool I1 { get; set; }
        public bool I2 { get; set; }
    }

    public class ModuleSignalsMuxModule : RTLCombinationalModule<ModuleSignalsMuxModuleInputs>
    {
        AndGateModule AndGate = new AndGateModule();
        OrGateModule OrGate = new OrGateModule();
        XorGateModule XorGate = new XorGateModule();

        ILogicGate[] gates => new ILogicGate[]
        {
            AndGate,
            OrGate,
            XorGate
        };

        public bool O => gates[Inputs.Addr].O;

        public RTLBitArray CombinedO => new RTLBitArray(gates.Select(g => g.O));

        protected override void OnSchedule(Func<ModuleSignalsMuxModuleInputs> inputsFactory)
        {
            base.OnSchedule(inputsFactory);

            AndGate.Schedule(() => new GateInputs() { I1 = Inputs.I1, I2 = Inputs.I2 });
            OrGate.Schedule(() => new GateInputs() { I1 = Inputs.I1, I2 = Inputs.I2 });
            XorGate.Schedule(() => new GateInputs() { I1 = Inputs.I1, I2 = Inputs.I2 });
        }
    }
}
