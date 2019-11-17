using Quokka.Public.Tools;
using SunTrackerDevice;
using System;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunTracker.Calibrate
{
    class Program
    {
        static void Main(string[] args)
        {
            var request = new SetServos()
            {
                s0 = 90,
                s1 = 90,
                s2 = 90,
                s3 = 90,
            };

            while(true)
            {
                try
                {
                    Console.WriteLine($"ServoID => value");

                    var servoId = Console.ReadLine();
                    if (servoId == "")
                        break;

                    var servoValue = Console.ReadLine();
                    if (servoValue == "")
                        break;

                    var port = new SerialPort();
                    port.PortName = "COM3";
                    port.BaudRate = 115200;// 9600;
                    port.Parity = Parity.None;
                    port.DataBits = 8;
                    port.StopBits = StopBits.Two;
                    port.Handshake = Handshake.None;

                    var data = new byte[]
                    {
                        byte.Parse(servoId),
                        byte.Parse(servoValue),
                    };

                    var prop = request.GetType().GetField($"s{data[0]}");
                    TypedReference tr = __makeref(request);
                    prop.SetValueDirect(tr, data[1]);

                    var payload = QuokkaJson.SerializeObject(new[] { request });
                    var bytes = Encoding.ASCII.GetBytes(payload);
                    port.Open();
                    port.Write(bytes, 0, bytes.Length);

                    Console.WriteLine("Waiting for response ...");

                    var buff = new byte[10];
                    var received = port.Read(buff, 0, 2);

                    foreach (var resp in buff.Take(received))
                        Console.WriteLine($"Got {resp}");

                    port.Close();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
