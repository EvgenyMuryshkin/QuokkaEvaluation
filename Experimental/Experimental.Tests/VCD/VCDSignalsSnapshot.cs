using System.Collections;
using System.Collections.Generic;

namespace Quokka.VCD
{
    public class VCDSignalsSnapshot : IEnumerable<KeyValuePair<string, VCDVariable>>
    {
        public string Name { get; set; }
        Dictionary<string, VCDVariable> _mapSignals = new Dictionary<string, VCDVariable>();
        Dictionary<string, VCDSignalsSnapshot> _childScopes = new Dictionary<string, VCDSignalsSnapshot>();

        public IEnumerable<VCDSignalsSnapshot> Scopes => _childScopes.Values;
        public IEnumerable<VCDVariable> Variables => _mapSignals.Values;

        public VCDSignalsSnapshot(string name)
        {
            Name = name;
        }

        public VCDSignalsSnapshot(IEnumerable<KeyValuePair<string, VCDVariable>> collection)
        {
            _mapSignals = new Dictionary<string, VCDVariable>(collection);
        }

        public VCDVariable Add(VCDVariable value)
        {
            _mapSignals[value.Name] = value;
            return value;
        }

        public IEnumerator<KeyValuePair<string, VCDVariable>> GetEnumerator()
        {
            return _mapSignals.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _mapSignals.GetEnumerator();
        }

        public VCDSignalsSnapshot Scope(string name)
        {
            if (!_childScopes.ContainsKey(name))
                _childScopes[name] = new VCDSignalsSnapshot(name);

            return _childScopes[name];
        }

        public void SetVariables(IEnumerable<VCDVariable> variables)
        {
            foreach (var v in variables)
            {
                _mapSignals[v.Name] = v;
            }
        }

        public override string ToString()
        {
            return $"{Name}, {_mapSignals.Count} Vars, {_childScopes.Count} Scopes";
        }
    }
}
