using Newtonsoft.Json;
using System;

namespace LEDDto
{
    public class ReportDTO
    {
        [JsonProperty(PropertyName = "colors")]
        public uint[] Colors { get; set; }

		[JsonProperty(PropertyName = "servos")]
		public uint[] Servos { get; set; }
	}

	public class ImageDTO
	{
		public string Base64Image { get; set; }
	}
}
