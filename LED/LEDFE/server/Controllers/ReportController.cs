using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using LEDDto;
using Microsoft.AspNetCore.Mvc;
using server.Cognitive;
using server.Services;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        private readonly BufferService _bufferService; 
        public ReportController(BufferService bufferService)
        {
            _bufferService = bufferService;
        }

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

        [HttpGet("[action]")]
        public async Task<IActionResult> Pid()
        {
            return Ok(Process.GetCurrentProcess().Id);
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
            var set = dto.Colors
                .Select(c => ToByteArray(c))
                .SelectMany(c => c);

            _bufferService.SetData(1, set.ToArray());
            _bufferService.SetData(2, dto.Servos.Select(v => (byte)v).ToArray());
        }
    }
}
