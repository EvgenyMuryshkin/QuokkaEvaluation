using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTools
{
    // TODO: move to public library
    public static class TestConverters
    {
        public static string ToHexFloat(byte[] data)
        {
            return string.Join("", data.Reverse().Select(v => v.ToString("X02")));
        }

        public static byte[] ToByteArray(float f)
        {
            return BitConverter.GetBytes(f);
        }

        public static float FloatFromByteArray(byte[] data)
        {
            return BitConverter.ToSingle(data);
        }

        public static int Int32FromByteArray(byte[] data)
        {
            return BitConverter.ToInt32(data);
        }
    }
}
