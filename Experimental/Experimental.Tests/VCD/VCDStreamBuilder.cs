using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Quokka.VCD
{
    public class VCDStreamBuilder
    {
        StringWriter _sb;
        Stack<VCDSignalsSnapshot> _scopes = new Stack<VCDSignalsSnapshot>();

        public VCDStreamBuilder(StringWriter sb)
        {
            _sb = sb;
        }

        internal string Underscored(string value)
        {
            return value.Replace(" ", "_");
        }

        internal void SectionStart(string section)
        {
            _sb.WriteLine($"${section}");
        }

        internal void SectionEnd()
        {
            _sb.WriteLine($"$end");
        }

        internal void Line(string value)
        {
            _sb.WriteLine(value);
        }

        internal void Section(string section, string value = "")
        {
            SectionStart(section);

            if (!string.IsNullOrWhiteSpace(value))
                Line(value);

            SectionEnd();
        }

        internal void InlineSection(string section, string value)
        {
            Line($"${section} {value} $end");
        }

        public void Date(DateTime value)
        {
            Section("date", $"{value.ToLongDateString()} {value.ToLongTimeString()}");
        }

        public void Version(string version)
        {
            Section("version", version);
        }

        public void Timescale(string version)
        {
            Section("timescale", version);
        }

        public void EndDefinitions()
        {
            Section("enddefinitions", "");
        }

        internal void PushScope(VCDSignalsSnapshot scope)
        {
            _scopes.Push(scope);
        }

        internal void PopScope()
        {
            _scopes.Pop();
        }

        internal void BeginScope(VCDSignalsSnapshot scope)
        {
            PushScope(scope);
            var name = Underscored(scope.Name);
            InlineSection("scope module", name);
        }

        internal void EndScope()
        {
            Section("upscope");
            PopScope();
        }

        internal string ScopeName => string.Join("_", _scopes.Reverse().Select(s => s.Name));

        internal void Variable(VCDVariableType type, string name, int size)
        {
            name = Underscored(name);
            var identifier = $"{ScopeName}_{name}";
            var reference = $"{ScopeName}_" + (size > 1 ? $"{name}[{size - 1}:0]" : name);
            InlineSection("var", $"{type.ToString().ToLower()} {size} {identifier} {reference}");
        }

        public void Snapshot(VCDSignalsSnapshot snapshot)
        {
            if (snapshot == null)
                return;

            PushScope(snapshot);

            foreach (var variable in snapshot.Variables)
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

                if (!string.IsNullOrWhiteSpace(value))
                {
                    SetValue(variable.Type, variable.Size, variable.Name, value);
                }
            }

            foreach (var child in snapshot.Scopes)
            {
                Snapshot(child);
            }

            PopScope();
        }

        public void Scope(VCDSignalsSnapshot scope)
        {
            BeginScope(scope);

            foreach (var s in scope.Scopes)
            {
                Scope(s);
            }

            foreach (var v in scope.Variables)
            {
                Variable(v.Type, v.Name, v.Size);
            }

            EndScope();
        }

        public void SetTime(int value)
        {
            Line($"#{value}");
        }

        public void SetValue(VCDVariableType type, int size, string signal, string value)
        {
            switch (type)
            {
                case VCDVariableType.String:
                    Line($"s{value} {ScopeName}_{signal}");
                    break;
                case VCDVariableType.Wire:
                    if (size == 1)
                        Line($"{value}{ScopeName}_{signal}");
                    else
                        Line($"b{value} {ScopeName}_{signal}");
                    break;
            }
        }
    }
}
