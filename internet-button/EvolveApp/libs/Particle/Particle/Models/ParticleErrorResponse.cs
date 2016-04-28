using Newtonsoft.Json;

namespace Particle.Models
{
	public class ParticleErrorResponse
	{
		[JsonProperty("ok")]
		public bool Ok { get; set; }
		[JsonProperty("error")]
		public string Error { get; set; }
	}
}