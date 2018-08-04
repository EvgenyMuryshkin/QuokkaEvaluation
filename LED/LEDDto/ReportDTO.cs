using Newtonsoft.Json;
using System;

namespace LEDDto
{
    public class ReportDTO
    {
        [JsonProperty(PropertyName = "colors")]
        public int[] Colors { get; set; }
    }
}
