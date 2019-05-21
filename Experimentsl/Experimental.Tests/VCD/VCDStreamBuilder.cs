using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Experimental.Tests
{
    public class VCDStreamBuilder
    {
        StringWriter _sb;
        Stack<VCDScope> _scopes = new Stack<VCDScope>();

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

        internal void BeginScope(VCDScope scope)
        {
            _scopes.Push(scope);
            var name = Underscored(scope.Name);
            Section("scope module", name);
        }

        internal void EndScope()
        {
            Section("upscope");
            _scopes.Pop();
        }

        internal void Variable(string name, int size)
        {
            var parentName = string.Join("_", _scopes.Reverse().Select(s => s.Name));

            var identifier = Underscored(name);
            var reference = $"{parentName}_" + (size > 1 ? $"{identifier}[{size - 1}:0]" : identifier);
            Section("var", $"wire {size} {reference} {reference}");
        }

        public void Scope(VCDScope scope)
        {
            BeginScope(scope);

            foreach (var s in scope.Scopes)
            {
                Scope(s);
            }

            foreach (var v in scope.Variables)
            {
                Variable(v.Name, v.Size);
            }

            EndScope();
        }

        public void SetTime(int value)
        {
            Line($"#{value}");
        }

        public void SetValue(string signal, string value)
        {
            Line($"{value}{signal}");
        }
    }
}
