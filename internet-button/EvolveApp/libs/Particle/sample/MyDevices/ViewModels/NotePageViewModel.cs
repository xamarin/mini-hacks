using Particle;

namespace MyDevices.ViewModels
{
	public class NotePageViewModel : BaseViewModel
	{
		public NotePageViewModel(ParticleDevice device)
		{
			Octive = 5;
			Device = device;
		}

		public ParticleDevice Device { get; internal set; }
		public int Octive { get; set; }

		public string OctiveText
		{
			get
			{
				return "Octive: " + Octive;
			}
		}

		public void IncreaseOctive()
		{
			Octive++;
			OnPropertyChanged("OctiveText");
		}

		public void DecreaseOctive()
		{
			if (Octive == 0)
				return;
			
			Octive--;
			OnPropertyChanged("OctiveText");
		}

		public void SendNote(string note)
		{
			if (Device == null)
				return;

			Device.CallFunctionAsync("note", note + Octive);
		}
	}
}