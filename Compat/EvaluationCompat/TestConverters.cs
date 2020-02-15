using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuokkaIntegrationTests
{
    public static class TestConverters
    {
        public static string ToHexFloat(byte[] data)
        {
            return string.Join("", data.Reverse().Select(v => v.ToString("X02")));
        }

        // TODO: move to public library
        public static byte[] ToByteArray(float f)
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write(f);
                bw.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                return ms.ToArray();
            }
        }

        public static float FloatFromByteArray(byte[] data)
        {
            using (var ms = new MemoryStream())
            using (var br = new BinaryReader(ms))
            {
                ms.Write(data, 0, 4);
                ms.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                return br.ReadSingle();
            }
        }

        public static int Int32FromByteArray(byte[] data)
        {
            using (var ms = new MemoryStream())
            using (var br = new BinaryReader(ms))
            {
                ms.Write(data, 0, 4);
                ms.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                return br.ReadInt32();
            }
        }
    }
}
