using Newtonsoft.Json;

namespace Particle.Models
{
	public class ParticleLoginResponse
	{
		[JsonProperty("access_token")]
		public string AccessToken { get; set; }
		[JsonProperty("token_type")]
		public string Type { get; set; }
		[JsonProperty("expires_in")]
		public int Expiration { get; set; }
		[JsonProperty("refresh_token")]
		public string RefreshToken { get; set; }
	}
}