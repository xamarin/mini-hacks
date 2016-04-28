using System;
using System.Threading.Tasks;

using Particle;
using Particle.Helpers;

namespace MyDevices.ViewModels
{
	public class SimonSaysViewModel : BaseViewModel
	{
		public ParticleDevice Device { get; internal set; }
		public bool gameRunning = false;
		public string playerEntry = "";
		string deviceGameRunning = "-1";
		public string simonMoves;
		int playerMoveCount = 0;

		public SimonSaysViewModel(ParticleDevice device)
		{
			Device = device;
		}

		public string DetailText
		{
			get
			{
				return playerEntry;
			}
		}

		#region Opacity Properties

		double redOpacity = 0.5;
		double greenOpacity = 0.5;
		double blueOpacity = 0.5;
		double yellowOpacity = 0.5;

		public double RedOpacity
		{
			get
			{
				return redOpacity;
			}
			set
			{
				redOpacity = value;
				OnPropertyChanged("RedOpacity");
			}
		}

		public double GreenOpacity
		{
			get
			{
				return greenOpacity;
			}
			set
			{
				greenOpacity = value;
				OnPropertyChanged("GreenOpacity");
			}
		}

		public double BlueOpacity
		{
			get
			{
				return blueOpacity;
			}
			set
			{
				blueOpacity = value;
				OnPropertyChanged("BlueOpacity");
			}
		}

		public double YellowOpacity
		{
			get
			{
				return yellowOpacity;
			}
			set
			{
				yellowOpacity = value;
				OnPropertyChanged("YellowOpacity");
			}
		}

		#endregion

		public void ClearEntry()
		{
			playerEntry = "";
			OnPropertyChanged("DetailText");
		}

		public async Task StartGame()
		{
			playerEntry = "";
			await ScheduleGameCheckAsync();
			await Device.CallFunctionAsync("startSimon");
			gameRunning = true;

			await Task.Delay(500);

			var simonParticle = await Device.GetVariableAsync("simon");
			simonMoves = simonParticle.Result.ToString();
			System.Diagnostics.Debug.WriteLine(simonMoves);
		}

		public void Winner()
		{
			playerEntry = "Winner";
			OnPropertyChanged("DetailText");

			EndGame();
		}

		public void EndGame()
		{
			gameRunning = false;
			deviceGameRunning = "-1";
			playerMoveCount = 0;
		}

		public async Task PlayerPressButtonAsync(string color)
		{
			playerEntry += color;
			OnPropertyChanged("DetailText");

			switch (color)
			{
				case "r":
					RedOpacity = 1;
					break;
				case "g":
					GreenOpacity = 1;
					break;
				case "b":
					BlueOpacity = 1;
					break;
				case "y":
					YellowOpacity = 1;
					break;
			}

			await Task.Delay(250);

			switch (color)
			{
				case "r":
					RedOpacity = 0.5;
					break;
				case "g":
					GreenOpacity = 0.5;
					break;
				case "b":
					BlueOpacity = 0.5;
					break;
				case "y":
					YellowOpacity = 0.5;
					break;
			}
		}

		public async Task<string> PlayMoveAsync()
		{
			var response = await Device.CallFunctionAsync("buttonPress", playerEntry);
			//if (response == "0")
			//{
			//	EndGame();
			//}
			if (response == "1")
			{
				playerEntry = "Correct!";
				OnPropertyChanged("DetailText");
				playerEntry = "";
			}
			//else if (response == "2")
			//{
			//	EndGame();
			//}

			return response;
		}

		public async Task PlaySimonMovesAsync()
		{
			for (var x = 0; x < playerMoveCount + 1; x++)
			{
				switch (simonMoves.Substring(x, x + 1))
				{
					case "r":
						//detailLabel.BackgroundColor = Color.Red;
						break;
					case "g":
						//detailLabel.BackgroundColor = Color.Green;
						break;
					case "b":
						//detailLabel.BackgroundColor = Color.Blue;
						break;
					case "y":
						//detailLabel.BackgroundColor = Color.Yellow;
						break;
				}

				await Task.Delay(250);
			}
			playerMoveCount++;
		}

		async void GameHandler(object sender, ParticleEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine($"{e.EventData.Event}: {e.EventData.Data}\n{e.EventData.DeviceId}");
			//await Device.UnsubscribeToEventsWithIdAsync(gameCheckGuid);
		}

		Guid gameCheckGuid;
		public async Task ScheduleGameCheckAsync()
		{
			gameCheckGuid = await Device.SubscribeToEventsWithPrefixAsync("endgame", GameHandler);
			//var keepChecking = true;
			//while (keepChecking)
			//{
			//	deviceGameRunning = await Device.CallFunctionAsync("checkIfLoss");

			//	if (deviceGameRunning == "0")
			//	{
			//		EndGame();
			//		keepChecking = false;
			//	}
			//	else if (deviceGameRunning == "2")
			//	{
			//		Winner();
			//		keepChecking = false;
			//	}

			//	if (keepChecking)
			//		await Task.Delay(TimeSpan.FromMilliseconds(100));
			//}
		}
	}
}