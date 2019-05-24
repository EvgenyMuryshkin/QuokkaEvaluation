using System.Collections;
using System.Collections.Generic;

namespace Quokka.VCD
{
    public class VCDSignalsSnapshot : IEnumerable<KeyValuePair<string, object>>
    {
        public string Name { get; set; }
        Dictionary<string, object> _mapSignals = new Dictionary<string, object>();
        Dictionary<string, VCDSignalsSnapshot> _childScopes = new Dictionary<string, VCDSignalsSnapshot>();

        public VCDSignalsSnapshot(string name)
        {
            Name = name;
        }

        public VCDSignalsSnapshot(IEnumerable<KeyValuePair<string, object>> collection)
        {
            _mapSignals = new Dictionary<string, object>(collection);
        }

        public void Add(string key, object value)
        {
            _mapSignals[key] = value;
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _mapSignals.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _mapSignals.GetEnumerator();
        }

        public object this[string key]
        {
            get
            {
                return _mapSignals[key];
            }
            set
            {
                _mapSignals[key] = value;
            }
        }

        public VCDSignalsSnapshot Scope(string name)
        {
            if (!_childScopes.ContainsKey(name))
                _childScopes[name] = new VCDSignalsSnapshot(name);

            return _childScopes[name];
        }

        public override string ToString()
        {
            return $"{Name}, {_mapSignals.Count} Vars, {_childScopes.Count} Scopes";
        }
    }
}
