using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quokka.VCD
{
    public static class VCDTools
    {
        public static string FormatScopePrefix(Stack<VCDSignalsSnapshot> hierarchy)
        {
            if (!hierarchy.Any())
                return "";

            return string.Join("", hierarchy.Reverse().Select(s => $"{s.Name}_"));
        }

        static List<VCDVariable> FlatternHierarchy(
            Stack<VCDSignalsSnapshot> hierarchy,
            VCDSignalsSnapshot snapshot)
        {
            var result = new List<VCDVariable>();

            if (snapshot != null)
            {
                hierarchy.Push(snapshot);

                snapshot.Variables.ForEach(v =>
                {
                    var flat = new VCDVariable($"{FormatScopePrefix(hierarchy)}{v.Name}", v.Value, v.Size, v.Type);
                    result.Add(flat);
                });

                foreach (var scope in snapshot.Scopes)
                    result.AddRange(FlatternHierarchy(hierarchy, scope));

                hierarchy.Pop();
            }

            return result;
        }

        public static List<VCDVariable> FlatternHierarchy(VCDSignalsSnapshot snapshot)
        {
            return FlatternHierarchy(new Stack<VCDSignalsSnapshot>(), snapshot);
        }

        public static VCDVariableType VarType(object value)
        {
            switch (value)
            {
                case string v: return VCDVariableType.String;
                default: return VCDVariableType.Wire;
            }
        }

        public static int SizeOf(object value)
        {
            var type = value.GetType();
            if (type.IsEnum)
            {
                return SizeOf(Activator.CreateInstance(type.GetEnumUnderlyingType()));
            }

            switch (value)
            {
                case bool v: return 1;
                case byte v: return 8;
                case sbyte v: return 8;
                case ushort v: return 16;
                case short v: return 16;
                case uint v: return 32;
                case int v: return 32;
                case ulong v: return 64;
                case long v: return 64;
                case RTLBitArray ba: return ba.Size;
                case string v: return 1;
                default: throw new Exception($"Unsupported type: {value?.GetType()?.Name}");
            };
        }

        public static string VCDValue(VCDVariable variable)
        {
            string value = null;

            switch (variable.Value)
            {
                case Enum v:
                    value = new RTLBitArray((uint)Convert.ChangeType(variable.Value, typeof(uint)))
                        .Resized(variable.Size)
                        .AsBinaryString();
                    break;
                case bool b: value = b ? "1" : "0"; break;
                case string s: value = s; break;
                case byte v: value = new RTLBitArray(v).AsBinaryString(); break;
                case sbyte v: value = new RTLBitArray(v).AsBinaryString(); break;
                case int v: value = new RTLBitArray(v).AsBinaryString(); break;
                case uint v: value = new RTLBitArray(v).AsBinaryString(); break;
                case long v: value = new RTLBitArray(v).AsBinaryString(); break;
                case ulong v: value = new RTLBitArray(v).AsBinaryString(); break;
                case short v: value = new RTLBitArray(v).AsBinaryString(); break;
                case ushort v: value = new RTLBitArray(v).AsBinaryString(); break;
                case RTLBitArray ba: value = ba.AsBinaryString(); break;
                default: throw new Exception($"Unsupported data type: {variable.Value.GetType()}");
            }

            return value;
        }
    }
}
