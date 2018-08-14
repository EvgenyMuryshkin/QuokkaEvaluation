using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace server.Services
{
    public class CommsService
    {
        private readonly BufferService _bufferService;
        public CommsService(BufferService bufferService)
        {
            _bufferService = bufferService;
        }

        public Task Run(CancellationToken ct)
        {
            return Task.Factory.StartNew(() =>
            {
                var portName = "COM5";
                Console.WriteLine($"Listening on {portName}");

                var port = new SerialPort();
                port.PortName = portName;
                port.BaudRate = 115200;// 9600;
                port.Parity = Parity.None;
                port.DataBits = 8;
                port.StopBits = StopBits.Two;
                port.Handshake = Handshake.None;

                port.Open();
                while (!ct.IsCancellationRequested)
                {
                    var address = (byte)port.ReadByte();
                    var data = _bufferService.GetData(address);
                    Console.WriteLine($"{DateTime.UtcNow}: Requested address: {address}, writing {data.Length}");

                    port.Write(data, 0, data.Length);
                }

                port.Close();
            });
        }
    }
}
