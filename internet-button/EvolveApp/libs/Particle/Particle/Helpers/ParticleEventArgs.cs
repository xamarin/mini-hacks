using System;

namespace Particle.Helpers
{
	public class ParticleEventArgs : EventArgs
	{
		public ParticleEventArgs()
		{
		}
		public ParticleEventArgs(string error)
		{
			Error = error;
		}

		public ParticleEventArgs(ParticleEvent eventData)
		{
			EventData = eventData;
		}


		public ParticleEvent EventData { get; set; }
		public string Error { get; set; }
	}
}