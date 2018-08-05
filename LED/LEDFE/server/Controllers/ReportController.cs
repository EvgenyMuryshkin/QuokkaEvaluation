using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using LEDDto;
using Microsoft.AspNetCore.Mvc;
using server.Cognitive;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        public static byte[] ToByteArray(uint data)
        {
          using (var ms = new MemoryStream())
          using (var bw = new BinaryWriter(ms))
          {
            bw.Write(data);
            bw.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            return ms.ToArray();
          }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Image([FromBody]ImageDTO dto)
        {
            var response = await AzureFaceAPI.MakeAnalysisRequest(dto.Base64Image);

            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task Colors([FromBody]ReportDTO dto)
        {
            var port = new SerialPort();
            port.PortName = "COM5";
            port.BaudRate = 115200;// 9600;
            port.Parity = Parity.None;
            port.DataBits = 8;
            port.StopBits = StopBits.Two;
            port.Handshake = Handshake.None;

            port.Open();
            foreach (var c in dto.Colors)
            {
                var bytes = ToByteArray(c);
                port.Write(bytes, 0, bytes.Length);
            }
            port.Close();
        }
    }
}
