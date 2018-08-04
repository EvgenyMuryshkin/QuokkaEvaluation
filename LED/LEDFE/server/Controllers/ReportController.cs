using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LEDDto;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        [HttpPost("[action]")]
        public async Task Colors([FromBody]ReportDTO dto)
        {
            foreach(var c in dto.Colors)
            {
                Console.WriteLine(c);
            }
        }
    }
}
