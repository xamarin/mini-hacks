using Newtonsoft.Json;

namespace Particle.Models
{
	public class ParticleFlashResponse
	{
		[JsonProperty("id")]
		public string Id { get; set; }
		[JsonProperty("status")]
		public string Status { get; set; }
	}
}