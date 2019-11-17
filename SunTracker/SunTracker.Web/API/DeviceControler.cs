using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Quokka.Public.Tools;
using SunTrackerDevice;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunTracker.Web.API
{
    public class SetServosRequest// : SetServos
    {

    }

    [Route("api/[controller]")]
    [ApiController]

    public class DeviceController : ControllerBase
    {
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Works!");
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send(/*[FromBody] SetServos request*/) // cannot deserialize into struct
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var payload = await reader.ReadToEndAsync();
                var request = JsonConvert.DeserializeObject<SetServos>(payload);

                var quokkPayload = QuokkaJson.SerializeObject(new[] { request });
                var quokkPayloadBytes = Encoding.ASCII.GetBytes(quokkPayload);

                var port = new SerialPort();
                port.PortName = "COM3";
                port.BaudRate = 115200;// 9600;
                port.Parity = Parity.None;
                port.DataBits = 8;
                port.StopBits = StopBits.Two;
                port.Handshake = Handshake.None;
                port.Open();
                port.Write(quokkPayloadBytes, 0, quokkPayloadBytes.Length);
                port.Close();

                return Ok("Ok");
            }
        }
    }
}
