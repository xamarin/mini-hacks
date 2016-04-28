using Newtonsoft.Json;

namespace Particle.Models
{
	public class ParticleClient
	{
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("type")]
		public string Type { get; set; }
		[JsonProperty("id")]
		public string Id { get; set; }
		[JsonProperty("secret")]
		public string Secret { get; set; }
	}

	public class ParticleOAuthResponse
	{
		[JsonProperty("ok")]
		public bool Success { get; set; }
		[JsonProperty("client")]
		public ParticleClient ClientDetails { get; set; }
	}
}