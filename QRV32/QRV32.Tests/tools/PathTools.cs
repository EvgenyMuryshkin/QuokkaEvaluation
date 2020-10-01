using System.IO;
using System.Linq;

namespace QRV32.Tests
{
    public class TestPathTools
    {
        public static string ProjectLocation(string current = null)
        {
            if (current == "")
                return "";

            current = current ?? Directory.GetCurrentDirectory();
            if (Directory.EnumerateFiles(current, "*.csproj").Any())
                return current;

            return ProjectLocation(Path.GetDirectoryName(current));
        }

        public static string SolutionLocation(string current = null)
        {
            if (current == "")
                return "";

            current = current ?? Directory.GetCurrentDirectory();
            if (Directory.EnumerateFiles(current, "*.sln").Any())
                return current;

            return SolutionLocation(Path.GetDirectoryName(current));
        }

    }
}
