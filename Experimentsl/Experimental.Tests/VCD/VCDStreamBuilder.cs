using System;
using System.Text;

namespace Experimental.Tests
{
    public class VCDStreamBuilder
    {
        StringBuilder _sb = new StringBuilder();

        internal string Underscored(string value)
        {
            return value.Replace(" ", "_");
        }

        internal void SectionStart(string section)
        {
            _sb.AppendLine($"${section}");
        }

        internal void SectionEnd()
        {
            _sb.AppendLine($"$end");
        }

        internal void Line(string value)
        {
            _sb.AppendLine(value);
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

        internal void BeginScope(string name)
        {
            name = Underscored(name);
            Section("scope module", name);
        }

        internal void EndScope()
        {
            Section("upscope");
        }

        internal void Variable(string name, int size)
        {
            name = Underscored(name);
            var reference = size > 1 ? $"{name}[{size - 1}:0]" : name;
            Section("var", $"wire {size} {name} {reference}");
        }

        public void Scope(VCDScope scope)
        {
            BeginScope(scope.Name);

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

        public override string ToString()
        {
            return _sb.ToString();
        }
    }
}
