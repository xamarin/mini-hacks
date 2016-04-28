using System.Collections.Generic;

using Newtonsoft.Json;

namespace Particle.Models
{
	public class ParticleDeviceObject
	{
		[JsonProperty("id")]
		public string Id { get; set; }
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("connected")]
		public bool Connected { get; set; }
		[JsonProperty("variables")]
		public Dictionary<string,string> Variables { get; set; }
		[JsonProperty("functions")]
		public List<string> Functions { get; set; }
		[JsonProperty("cc3000_patch_version")]
		public string CC3000PatchVersion { get; set; }
		[JsonProperty("product_id")]
		public int ProductId { get; set; }
		[JsonProperty("last_heard")]
		public string LastHeard { get; set; }
		[JsonProperty("requires_deep_update")]
		public bool RequiresUpdate { get; set; }
		[JsonProperty("last_iccid")]
		public string ICCID { get; set; }
		[JsonProperty("imei")]
		public string IMEI { get; set; }
	}
}