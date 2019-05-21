using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.Reflection
{
    public static class MemberExtensions
    {
        public static object GetValue(this MemberInfo member, object target)
        {
            switch (member)
            {
                case PropertyInfo p: return p.GetValue(target);
                case FieldInfo f: return f.GetValue(target);
                default: throw new InvalidOperationException();
            }
        }
    }
}

namespace QuokkaTests.Experimental
{
    public static class RTLModuleHelper
    {
        public static List<MemberInfo> ModuleProperties(Type type)
        {
            var properties = type
                .GetProperties()
                .Where(p => typeof(ICombinationalRTLModule).IsAssignableFrom(p.PropertyType))
                .OfType<MemberInfo>();

            var fields = type
                .GetFields()
                .Where(f => typeof(ICombinationalRTLModule).IsAssignableFrom(f.FieldType))
                .OfType<MemberInfo>();

            return properties.Concat(fields).ToList();
        }

        public static List<MemberInfo> SignalProperties(Type type)
        {
            var properties = type
                .GetProperties()
                .Where(p => p.PropertyType.IsValueType || p.PropertyType == typeof(RTLBitArray))
                .OfType<MemberInfo>();

            var fields = type
                .GetFields()
                .Where(p => p.FieldType.IsValueType || p.FieldType == typeof(RTLBitArray))
                .OfType<MemberInfo>();

            return properties.Concat(fields).ToList();
        }
    }
}
