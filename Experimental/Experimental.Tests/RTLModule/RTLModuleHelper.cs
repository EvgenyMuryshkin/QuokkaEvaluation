using Quokka.RTL;
using Quokka.VCD;
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

        public static Type GetMemberType(this MemberInfo member)
        {
            switch (member)
            {
                case PropertyInfo p: return p.PropertyType;
                case FieldInfo f: return f.FieldType;
                default: throw new InvalidOperationException();
            }
        }
    }
}

namespace Quokka.RTL
{
    public static class RTLModuleHelper
    {
        public static IEnumerable<MemberInfo> SynthesizableMembers(Type type)
        {
            var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).OfType<MemberInfo>();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).OfType<MemberInfo>();

            return props.Concat(fields);
        }

        public static List<MemberInfo> ModuleProperties(Type type)
        {
            return SynthesizableMembers(type)
                .Where(m => typeof(IRTLCombinationalModule).IsAssignableFrom(m.GetMemberType()))
                .ToList();
        }

        public static List<MemberInfo> SignalProperties(Type type)
        {
            return SynthesizableMembers(type)
                .Where(m => m.GetMemberType().IsValueType || m.GetMemberType() == typeof(RTLBitArray))
                .ToList();
        }

        public static int SizeOfEnum(Type enumType)
        {
            var values = Enum.GetValues(enumType);

            // defaults to single bit variable
            if (values.Length == 0)
                return 1;

            var maxValue = Enumerable
                .Range(0, values.Length)
                .Select(idx => (uint)Convert.ChangeType(values.GetValue(idx), typeof(uint)))
                .Max();

            return (int)Math.Max(1, Math.Ceiling(Math.Log(maxValue + 1, 2)));
        }
    }
}
