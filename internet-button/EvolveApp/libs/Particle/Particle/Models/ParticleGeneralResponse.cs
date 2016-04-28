using System.Collections.Generic;

using Newtonsoft.Json;

namespace Particle.Models
{
	public class ParticleGeneralResponse
	{
		[JsonProperty("ok")]
		public bool Ok { get; set; }
		[JsonProperty("errors")]
		public List<string> Errors { get; set; }
		[JsonProperty("code")]
		public int Code { get; set; }
	}
}