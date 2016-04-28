using Newtonsoft.Json;

namespace Particle.Models
{
	public class ParticleFunctionResponse
	{
		[JsonProperty("id")]
		public string Id { get; set; }
		[JsonProperty("last_app")]
		public string LastApp { get; set; }
		[JsonProperty("connected")]
		public bool Connected { get; set; }
		[JsonProperty("return_value")]
		public int Value { get; set; }
	}
}