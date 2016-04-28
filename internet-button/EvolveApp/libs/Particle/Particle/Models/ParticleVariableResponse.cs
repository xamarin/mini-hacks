using Newtonsoft.Json;

namespace Particle.Models
{
	public class CoreInfo
	{
		[JsonProperty("last_app")]
		public string LastApp { get; set; }
		[JsonProperty("last_heard")]
		public string LastHeard { get; set; }
		[JsonProperty("connected")]
		public bool Connected { get; set; }
		[JsonProperty("last_handshake_at")]
		public string LastHandshake { get; set; }
		[JsonProperty("deviceID")]
		public string DeviceId { get; set; }
		[JsonProperty("product_id")]
		public int ProductId { get; set; }
	}

	public class ParticleVariableResponse
	{
		[JsonProperty("cmd")]
		public string Command { get; set; }
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("result")]
		public object Result { get; set; }
		[JsonProperty("coreInfo")]
		public CoreInfo Core { get; set; }
	}
}