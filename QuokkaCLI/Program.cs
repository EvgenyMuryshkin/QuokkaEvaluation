using System;
using System.IO;
using Quokka.Core.Bootstrap;
using Quokka.RTL;

namespace QuokkaCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Running from {Directory.GetCurrentDirectory()}");
            Console.WriteLine($"Quokka.FPGA version: {typeof(QuokkaRunner).Assembly.GetName().Version}");
            Console.WriteLine($"Quokka.RTL version: {typeof(RTLBitArray).Assembly.GetName().Version}");
      
            QuokkaRunner.Default(args);
        }
    }
}
