using System;

using Xamarin.Forms;
using Particle;

using System.Threading.Tasks;
using System.Collections.Generic;

using MyDevices.Interfaces;

namespace MyDevices.ViewModels
{
	public class DeviceDetailViewModel : BaseViewModel
	{
		public DeviceDetailViewModel(ParticleDevice device)
		{
			Device = device;
			Variables = Device.Variables;
		}

		ParticleDevice _device;
		public ParticleDevice Device
		{
			get
			{
				return _device;
			}
			set
			{
				if (value == _device)
					return;
				_device = value;
			}
		}

		bool isBusy = false;
		public bool IsBusy
		{
			get
			{
				return isBusy;
			}
			set
			{
				if (isBusy == value)
					return;
				isBusy = value;
				OnPropertyChanged("IsBusy");
			}
		}

		public Dictionary<string, string> Variables;

		public async Task RefreshVariableValuesAsync()
		{
			if (!Device.Connected)
				return;
			
			Dictionary<string, string> updatedValues = new Dictionary<string, string>();
			foreach (var variable in Variables)
			{
				var currentValue = await _device.GetVariableAsync(variable.Key);
				System.Diagnostics.Debug.WriteLine(currentValue.Result.ToString());
				updatedValues.Add(variable.Key, currentValue.Result.ToString());
			}

			Variables = updatedValues;
		}

		public async Task RefreshDeviceAsync()
		{
			Device = await ParticleCloud.SharedInstance.GetDeviceAsync(Device.Id);
			Variables = Device?.Variables;
			if (Device != null)
				await RefreshVariableValuesAsync();
		}

		public async Task<bool> TryFlashFileAsync(string fileSelected)
		{
			IsBusy = true;
			bool response = false;
			switch (fileSelected)
			{
				case "Tinker":
					response = await Device.FlashKnownAppAsync("tinker");
					break;
				case "RGB LED":
					var rgbBytes = DependencyService.Get<IDirectory>().GetByteArrayFromFile("firmware");
					response = await Device.FlashFilesAsync(rgbBytes, "firmware.bin");
					break;
				case "Sound Check":
					var pianoBytes = DependencyService.Get<IDirectory>().GetByteArrayFromFile("piano");
					response = await Device.FlashFilesAsync(pianoBytes, "piano.bin");
					break;
				case "Simon Says":
					var simonBytes = DependencyService.Get<IDirectory>().GetByteArrayFromFile("simonsays");
					response = await Device.FlashFilesAsync(simonBytes, "simonsays.bin");
					break;
				case "Follow me LED":
					var followMeBytes = DependencyService.Get<IDirectory>().GetByteArrayFromFile("followMeLED");
					response = await Device.FlashFilesAsync(followMeBytes, "followMeLED.bin");
					break;
				case "Shake LED":
					var shakeLEDBytes = DependencyService.Get<IDirectory>().GetByteArrayFromFile("shakeled");
					response = await Device.FlashFilesAsync(shakeLEDBytes, "shakeled.bin");
					break;
			}

			IsBusy = false;
			return response;
		}
	}
}