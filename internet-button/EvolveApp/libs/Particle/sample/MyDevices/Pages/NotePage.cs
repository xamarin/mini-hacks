using Xamarin.Forms;

using Particle;

using MyDevices.ViewModels;
using MyDevices.Views;

namespace MyDevices.Pages
{
	public class NotePage : BasePage
	{
		NotePageViewModel ViewModel;

		public NotePage(ParticleDevice device)
		{
			ViewModel = new NotePageViewModel(device);
			BindingContext = ViewModel;

			var piano = new Piano();
			piano.upOctive.Command = new Command(() => ViewModel.IncreaseOctive());
			piano.downOctive.Command = new Command(() => ViewModel.DecreaseOctive());
			piano.cKey.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => ViewModel.SendNote("C")) });
			piano.bKey.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => ViewModel.SendNote("B")) });
			piano.aKey.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => ViewModel.SendNote("A")) });
			piano.gKey.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => ViewModel.SendNote("G")) });
			piano.fKey.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => ViewModel.SendNote("F")) });
			piano.eKey.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => ViewModel.SendNote("E")) });
			piano.dKey.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => ViewModel.SendNote("D")) });

			Content = piano;
		}
	}
}