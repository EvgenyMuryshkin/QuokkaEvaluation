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

        public string Name { get; set; }
        public int Size { get; set; }
        public VCDVariableType Type { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return $"{Name} = {Value}, {Type}, {Size} bits";
        }
    }
}
