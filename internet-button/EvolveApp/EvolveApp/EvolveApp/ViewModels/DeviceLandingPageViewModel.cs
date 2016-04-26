using Particle;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using EvolveApp.Helpers;
using System;

namespace EvolveApp.ViewModels
{
	public class DeviceLandingPageViewModel : BaseViewModel
	{
		public ParticleDevice Device { get; internal set; }

		public DeviceLandingPageViewModel(ParticleDevice device)
		{
			Device = device;

			if (!device.Connected)
			{
				SetLock();
				IsBusy = false;
			}
		}

		string currentApp;
		public string CurrentApp
		{
			get
			{
				var success = variables.TryGetValue("currentApp", out currentApp);

				if (success && currentApp != "string")
				{
					System.Diagnostics.Debug.WriteLine("Connected");
					return $"CURRENT APP: {currentApp.ToUpper()}";
				}
				System.Diagnostics.Debug.WriteLine("Disconnected");
				return "CURRENT APP: DISCONNECTED";
			}
		}

		public string AppDescription
		{
			get
			{
				var success = variables.TryGetValue("currentApp", out currentApp);
				if (success && currentApp != "string")
					return InternetButtonHelper.GetAppDescription(currentApp);
				return "";
			}
		}

		public string VariableCount
		{
			get { return $"{variables.Count}" ?? "0"; }
		}

		public string FunctionCount
		{
			get
			{
				if (Device.Functions == null) return "0";
				return $"{Device.Functions?.Count}";
			}
		}

		public bool DeviceConnected
		{
			get
			{
				return !Device.Connected;
				if (Device.Connected)
					return false;
				return true;
			}
		}

		Command refreshDeviceCommand;
		public Command RefreshDeviceCommand
		{
			get
			{
				return refreshDeviceCommand ?? (refreshDeviceCommand = new Command(async () => await RefreshDeviceAsync()));
			}
		}

		//Command unclaimDeviceCommand;
		//public Command UnclaimDeviceCommand
		//{
		//	get
		//	{
		//		return unclaimDeviceCommand ?? (unclaimDeviceCommand = new Command(async () => await UnclaimDeviceAsync()));
		//	}
		//}

		//public async Task UnclaimDeviceAsync()
		//{
		//	await Device.UnclaimAsync();
		//}

		public bool InteractButtonLock
		{
			get
			{
				if (IsBusy)
					return false;
				if (CurrentApp.ToLower().Contains("followmeled") || CurrentApp.ToLower().Contains("shake led"))
					return false;
				return true;
			}
		}

		public bool FlashButtonLock
		{
			get { return !IsBusy; }
		}

		public void SetLock(bool locked = true)
		{
			IsBusy = locked;
			OnPropertyChanged("InteractButtonLock");
			OnPropertyChanged("FlashButtonLock");
		}

		public Dictionary<string, string> variables = new Dictionary<string, string>();

		public async Task RefreshDeviceAsync()
		{
			if (IsBusy)
				return;

			SetLock();

			Device = await ParticleCloud.SharedInstance.GetDeviceAsync(Device.Id);
			variables.Clear();

			if (Device.Connected)
			{
				foreach (var variable in Device.Variables)
				{
					var variableValue = await Device.GetVariableAsync(variable.Key);

					if (variableValue.Result != null)
					{
						variables.Add(variable.Key, variableValue.Result.ToString());

						if (variable.Key == "currentApp")
							OnPropertyChanged("CurrentApp");
					}
					else {
						Device.Connected = false;
						break;
					}
				}
			}

			OnPropertyChanged("VariableCount");
			OnPropertyChanged("FunctionCount");
			OnPropertyChanged("DeviceConnected");
			OnPropertyChanged("AppDescription");

			if (Device.Connected)
			{
				System.Diagnostics.Debug.WriteLine("Device verified Connected");
				SetLock(false);
			}
			else {
				System.Diagnostics.Debug.WriteLine("Device verified Disconnected");
				IsBusy = false;
				OnPropertyChanged("CurrentApp");
			}
		}

		public async Task<bool> TryFlashFileAsync(string fileSelected)
		{
			if (IsBusy)
				return false;

			variables.Remove("currentApp");
			OnPropertyChanged("AppDescription");

			SetLock();
			bool response = false;

			var assembly = typeof(DeviceLandingPageViewModel).GetTypeInfo().Assembly;
			Stream stream = null;
			string filename = "";

			switch (fileSelected)
			{
				case "RGB LED":
					filename = "rgbled.bin";
					break;
				case "Simon Says":
					filename = "simonsays.bin";
					break;
				case "Follow me LED":
					filename = "followme.bin";
					break;
				case "Shake LED":
					filename = "shakeled.bin";
					break;
			}

			stream = assembly.GetManifestResourceStream($"EvolveApp.Binaries.{filename}");
			if (stream == null)
				return false;

			DateTime lastDate = DateTime.Now;

			using (var reader = new System.IO.BinaryReader(stream))
			{
				response = await Device.FlashFilesAsync(reader.ReadBytes(((int)stream.Length)), filename);
			}

			await Device.RefreshAsync();
			response = await WaitForFlashCompleteAsync(Device.LastHeard);

			if (response)
			{
				SetPrivateBusy(false);
				await RefreshDeviceAsync();
			}

			SetLock(false);

			return response;
		}

		async Task<bool> WaitForFlashCompleteAsync(DateTime lastDate)
		{
			bool flashComplete = false;
			int counter = 0;

			await Task.Delay(10000);

			while (!flashComplete)
			{
				var currentApp = await Device.GetVariableAsync("currentApp");

				if (currentApp != null)
					if (currentApp.Result != null)
						flashComplete = true;

				counter++;

				if (counter >= 20)
					return false;
			}

			return true;
		}
	}
}