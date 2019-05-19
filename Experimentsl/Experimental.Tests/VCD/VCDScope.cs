using System.Collections.Generic;

namespace Experimental.Tests
{
    public class VCDVariable
    {
        public string Name { get; set; }
        public int Size { get; set; }
    }

    public class VCDScope
    {
        public string Name { get; set; } = "TOP";
        public List<VCDVariable> Variables { get; set; } = new List<VCDVariable>();
        public List<VCDScope> Scopes = new List<VCDScope>();
    }
}
