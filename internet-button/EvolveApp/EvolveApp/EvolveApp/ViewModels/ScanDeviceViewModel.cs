using System;
using System.Threading.Tasks;
using Particle;
using Xamarin.Forms;
using EvolveApp.Helpers;
namespace EvolveApp.ViewModels
{
	public class ScanDeviceViewModel : BaseViewModel
	{
		public ParticleDevice Device { get; internal set; }

		public async Task<bool> GetDevice(string id)
		{
			IsBusy = true;

			Device = await ParticleCloud.SharedInstance.GetDeviceAsync(id);

			if (Device == null)
			{
				//device not owned
				var success = await ParticleCloud.SharedInstance.ClaimDeviceAsync(id);
				if (success)
				{
					Device = await ParticleCloud.SharedInstance.GetDeviceAsync(id);
					IsBusy = false;
					return true;
				}
				else
				{
					IsBusy = false;
					var device = InternetButtonHelper.GetDeviceName(id);
					Application.Current.MainPage.DisplayAlert("Uh Oh", $"Can you get a Xamarin to help reset {device}?", "Will do");
					return false;
				}
			}
			else if (Device.Connected)
			{
				//Device is owned and connected
				IsBusy = false;
				return true;
			}
			else if (!Device.Connected)
			{
				//Device is owned but disconnected
				IsBusy = false;
				return true;
			}

			return false;
			//var success = await ParticleCloud.SharedInstance.ClaimDeviceAsync(id);
			//if (success)
			//{
			//	Device = await ParticleCloud.SharedInstance.GetDeviceAsync(id);
			//	IsBusy = false;
			//	return true;
			//}
			//else
			//{
			//	IsBusy = false;
			//	var device = InternetButtonHelper.GetDeviceName(id);
			//	Application.Current.MainPage.DisplayAlert("Uh Oh", $"Someone already has this device claimed, can you please ask a Xamarin to reset {device}", "Will do");
			//	return false;
			//}
		}

		public void SetLock()
		{
			IsBusy = true;
			OnPropertyChanged("ButtonLock");
		}

		public void ClearLock()
		{
			IsBusy = false;
			OnPropertyChanged("ButtonLock");
		}

		public bool ButtonLock
		{
			get { return !IsBusy; }
		}
	}
}