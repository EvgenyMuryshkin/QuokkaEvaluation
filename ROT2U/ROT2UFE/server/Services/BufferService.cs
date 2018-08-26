using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Services
{
    public class BufferService
    {
        ConcurrentDictionary<byte, byte[]> Outgoing = new ConcurrentDictionary<byte, byte[]>();

        public BufferService()
        {
            //LED
            SetData(1, Enumerable.Range(0, 256).Select(i => (byte)0).ToArray());
            // SERVOS
            SetData(2, Enumerable.Range(0, 5).Select(i => (byte)0).ToArray());
        }

        public void SetData(byte address, byte[] data)
        {
            Outgoing.AddOrUpdate(address, data, (key, value) => data);
        }

        public byte[] GetData(byte address)
        {
            Outgoing.TryGetValue(address, out byte[] data);
            return data ?? Array.Empty<byte>();
        }
    }
}
