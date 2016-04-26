using System;
using Newtonsoft.Json;
namespace EvolveApp
{
	public class SimonSaysActivity
	{
		[JsonIgnore]
		public readonly static string StartSimon = "startsimon";
		[JsonIgnore]
		public readonly static string PlayerMove = "playermove";
		[JsonIgnore]
		public readonly static string EndSimon = "endsimon";
		[JsonIgnore]
		public readonly static string ButtonPress = "buttonpress";
		[JsonIgnore]
		public readonly static string CorrectMove = "correctmove";
		[JsonIgnore]
		public readonly static string LocalMove = "checklocalmove";

		[JsonProperty(PropertyName = "g")]
		public string GameId { get; set; }
		[JsonProperty(PropertyName = "a")]
		public string Activity { get; set; }
		[JsonProperty(PropertyName = "u")]
		public string User { get; set; }
		[JsonProperty(PropertyName = "v")]
		public string Value { get; set; }
	}
}