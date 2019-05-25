using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quokka.VCD
{
    public static class VCDTools
    {
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
    }
}
