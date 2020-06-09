using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Experimental.Tests
{
    public class PathTools
    {
        public static string VCDOutputPath([CallerMemberName]string testName = "")
        {
            var projectPath = PathToProject();

            return Path.Combine(projectPath, "SimResults", $"{testName}.vcd");
        }

        public static string PathToProject(string current = null)
        {
            current = current ?? Directory.GetCurrentDirectory();

            if (Path.GetPathRoot(current) == current)
                throw new Exception("Project folder not found");

            if (Directory.EnumerateFiles(current, "*.csproj").Any())
            {
                return current;
            }

            return PathToProject(Path.GetDirectoryName(current));
        }
    }
}
