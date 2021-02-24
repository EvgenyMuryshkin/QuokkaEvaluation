using FPGA.Attributes;
using System.Text;

namespace Tutorials
{
    [OnTranslation]
    public static class TutorialsDataSource
    {
        public static byte[] HelloWorldBytes()
        {
            return Encoding.ASCII.GetBytes("Hello World!\n");
        }

        public static byte[] HelloWorldBytesFromHandler(uint index)
        {
            return Encoding.ASCII.GetBytes($"{index}: Hello World from handler!\n");
        }
    }
}
