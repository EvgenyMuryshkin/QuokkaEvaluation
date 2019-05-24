using System;

namespace Quokka.VCD
{
    public class VCDVariable
    {
        public VCDVariable(string name, object value)
        {
            Name = name;
            Value = value;
            Size = VCDTools.SizeOf(value);
            Type = VCDTools.VarType(value);
        }

        public string Name { get; }
        public int Size { get; set; }
        public VCDVariableType Type { get; set; }

        object _value;
        public object Value {
            get
            {
                return _value;
            }
            set
            {
                if (_value == null)
                {
                    _value = value;
                }
                else
                {
                    if (_value.GetType() != value.GetType())
                        throw new Exception($"Value type change detected in {Name}");

                    _value = value;
                }
            }
        }

        public override string ToString()
        {
            return $"{Name} = {Value}, {Type}, {Size} bits";
        }
    }
}
