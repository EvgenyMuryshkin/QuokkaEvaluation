using System;
using System.IO;
using System.Runtime.InteropServices;
using Quokka.Core.Bootstrap;
using Quokka.RTL;

namespace QuokkaCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Environment: {RuntimeInformation.OSDescription}, {RuntimeInformation.OSArchitecture}, {RuntimeInformation.FrameworkDescription}, {RuntimeInformation.ProcessArchitecture}");
            Console.WriteLine($"Current location: {Directory.GetCurrentDirectory()}");
            Console.WriteLine($"Quokka.FPGA version: {typeof(QuokkaRunner).Assembly.GetName().Version}");
            Console.WriteLine($"Quokka.RTL version: {typeof(RTLBitArray).Assembly.GetName().Version}");


            Console.WriteLine("Cleaning up ...");

            var tempFolder = Path.Combine(Path.GetTempPath(), "quokka");
            if (Directory.Exists(tempFolder))
                Directory.Delete(tempFolder, true);

            QuokkaRunner.Default(args);
        }
    }
}
