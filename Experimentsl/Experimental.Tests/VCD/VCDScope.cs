using System.Collections.Generic;

namespace Quokka.VCD
{
    public class VCDScope
    {
        public string Name { get; set; } = "TOP";
        public List<VCDVariable> Variables { get; set; } = new List<VCDVariable>();
        public List<VCDScope> Scopes = new List<VCDScope>();

        public override string ToString()
        {
            return $"{Name}, {Variables.Count} Vars, {Scopes.Count} Scopes";
        }
    }
}
