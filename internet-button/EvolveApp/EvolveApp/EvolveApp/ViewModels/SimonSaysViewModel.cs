using System;
using System.Threading.Tasks;

using Particle;
using Particle.Helpers;
using Xamarin.Forms;
using System.Collections.Generic;
using Newtonsoft.Json;
using Plugin.Toasts;

namespace EvolveApp.ViewModels
{
	public class SimonSaysViewModel : BaseViewModel
	{
		public ParticleDevice InternetButton { get; internal set; }
		public bool gameRunning = false;
		public string playerEntry = "";
		public string simonMoves;
		bool buttonLock;

		public SimonSaysViewModel(ParticleDevice device)
		{
			InternetButton = device;
		}

		public bool ShowClearButton
		{
			get
			{
				if (L1 == Color.Transparent) return false;
				return true;
			}
		}

		#region Lights binding properties

		Color l1color = SimonSaysColors.Grey;
		Color l2color = SimonSaysColors.Grey;
		Color l3color = SimonSaysColors.Grey;
		Color l4color = SimonSaysColors.Grey;
		Color l5color = SimonSaysColors.Grey;
		Color l6color = SimonSaysColors.Grey;
		Color l7color = SimonSaysColors.Grey;
		Color l8color = SimonSaysColors.Grey;
		Color l9color = SimonSaysColors.Grey;
		Color l10color = SimonSaysColors.Grey;

		public Color L1
		{
			get { return l1color; }
			set
			{
				if (l1color == value)
					return;
				l1color = value;
				OnPropertyChanged("L1");
			}
		}
		public Color L2
		{
			get { return l2color; }
			set
			{
				if (l2color == value)
					return;
				l2color = value;
				OnPropertyChanged("L2");
			}
		}
		public Color L3
		{
			get { return l3color; }
			set
			{
				if (l3color == value)
					return;
				l3color = value;
				OnPropertyChanged("L3");
			}
		}
		public Color L4
		{
			get { return l4color; }
			set
			{
				if (l4color == value)
					return;
				l4color = value;
				OnPropertyChanged("L4");
			}
		}
		public Color L5
		{
			get { return l5color; }
			set
			{
				if (l5color == value)
					return;
				l5color = value;
				OnPropertyChanged("L5");
			}
		}
		public Color L6
		{
			get { return l6color; }
			set
			{
				if (l6color == value)
					return;
				l6color = value;
				OnPropertyChanged("L6");
			}
		}
		public Color L7
		{
			get { return l7color; }
			set
			{
				if (l7color == value)
					return;
				l7color = value;
				OnPropertyChanged("L7");
			}
		}
		public Color L8
		{
			get { return l8color; }
			set
			{
				if (l8color == value)
					return;
				l8color = value;
				OnPropertyChanged("L8");
			}
		}
		public Color L9
		{
			get { return l9color; }
			set
			{
				if (l9color == value)
					return;
				l9color = value;
				OnPropertyChanged("L9");
			}
		}
		public Color L10
		{
			get { return l10color; }
			set
			{
				if (l10color == value)
					return;
				l10color = value;
				OnPropertyChanged("L10");
			}
		}

		#endregion

		#region ActionButtion Implementation 

		private Command actionCommand;
		public Command ActionCommand
		{
			get
			{
				return actionCommand ?? (actionCommand = new Command(async () => await PerformAction()));
			}
		}

		public string ActionText
		{
			get
			{
				if (gameRunning) return "Submit Move".ToUpper();
				return "Start Game".ToUpper();
			}
		}

		public Color ActionColor
		{
			get
			{
				if (gameRunning) return AppColors.Blue;
				return AppColors.Green;
			}
		}

		async Task PerformAction()
		{
			if (!gameRunning)
			{
				//Start game
				gameRunning = true;
				OnPropertyChanged("ActionText");
				OnPropertyChanged("ActionColor");
				await StartGame();
			}
			else {
				var result = await InternetButton.CallFunctionAsync("buttonPress", playerEntry);

                if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
                {
                    if (result == "1")
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            var notificator = DependencyService.Get<IToastNotificator>();
                            notificator.Notify(ToastNotificationType.Success,
                                $"{InternetButton.Name} Says:", "You got that one right....", TimeSpan.FromSeconds(2));
                        });
                    }
                    else if (result == "-1")
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            var notificator = DependencyService.Get<IToastNotificator>();
                            notificator.Notify(ToastNotificationType.Warning,
                                $"{InternetButton.Name} Says:", "Don't interrupt my masterpiece!!!", TimeSpan.FromSeconds(1));
                        });
                    }
                }

				ClearPlayerEntry();
			}
		}

		#endregion

		#region Opacity Properties

		double redOpacity = 1;
		double greenOpacity = 1;
		double blueOpacity = 1;
		double yellowOpacity = 1;

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

		#region Light Box Implementation

		public void ClearPlayerEntry()
		{
			playerEntry = "";
			L1 = SimonSaysColors.Grey;
			L2 = SimonSaysColors.Grey;
			L3 = SimonSaysColors.Grey;
			L4 = SimonSaysColors.Grey;
			L5 = SimonSaysColors.Grey;
			L6 = SimonSaysColors.Grey;
			L7 = SimonSaysColors.Grey;
			L8 = SimonSaysColors.Grey;
			L9 = SimonSaysColors.Grey;
			L10 = SimonSaysColors.Grey;
			OnPropertyChanged("ShowClearButton");
		}

		void SetLightColor(Color color)
		{
			switch (playerEntry.Length)
			{
				case 1:
					L1 = color;
					OnPropertyChanged("ShowClearButton");
					break;
				case 2:
					L2 = color;
					break;
				case 3:
					L3 = color;
					break;
				case 4:
					L4 = color;
					break;
				case 5:
					L5 = color;
					break;
				case 6:
					L6 = color;
					break;
				case 7:
					L7 = color;
					break;
				case 8:
					L8 = color;
					break;
				case 9:
					L9 = color;
					break;
				case 10:
					L10 = color;
					break;
			}
		}

		#endregion

		#region IoT Interactions

		Guid gameCheckGuid;
		string gameId = "";

		public async Task StartGame()
		{
			playerEntry = "";
			gameCheckGuid = await InternetButton.SubscribeToEventsWithPrefixAsync("SimonSays", GameHandler);

            if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var notificator = DependencyService.Get<IToastNotificator>();
                    notificator.Notify(ToastNotificationType.Success,
                        $"{InternetButton.Name} Says:", "Better bring your A game!!", TimeSpan.FromSeconds(1));
                });
            }

			var success = await InternetButton.CallFunctionAsync("startSimon");

			if (success == "Timed out." )
			{
                if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        var notificator = DependencyService.Get<IToastNotificator>();
                        notificator.Notify(ToastNotificationType.Success,
                            $"{InternetButton.Name} Died", "But I'll come back to life!!", TimeSpan.FromSeconds(1));
                    });
                }
			}
			gameRunning = true;

			await Task.Delay(500);

			var simonParticle = await InternetButton.GetVariableAsync("simon");
			simonMoves = simonParticle.Result.ToString();

			Random rand = new Random();
			for (var i = 0; i < 10; i++)
			{
				gameId += rand.Next(0, 9);
			}

		}

		public async Task Winner()
		{
			playerEntry = "Winner";

            if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var notificator = DependencyService.Get<IToastNotificator>();
                    notificator.Notify(ToastNotificationType.Success,
                        "Winner", "You beat Simon!!", TimeSpan.FromSeconds(2));
                });
            }

			OnPropertyChanged("DetailText");

			await EndGame();
		}

		public async Task Loser()
		{
            if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var notificator = DependencyService.Get<IToastNotificator>();
                    notificator.Notify(ToastNotificationType.Error,
                        $"{InternetButton.Name} Says: ", "MWHUAHAHAHA I WIN!!", TimeSpan.FromSeconds(2));
                });
            } else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.DisplayAlert($"{InternetButton.Name} Says", "MWHUAHAHAHA I WIN!!", "This time...");
                });
            }

			await EndGame();
		}

		public async Task EndGame()
		{
			gameRunning = false;

			ClearPlayerEntry();
			OnPropertyChanged("ActionText");
			OnPropertyChanged("ActionColor");
			await InternetButton.UnsubscribeToEventsWithIdAsync(gameCheckGuid);
			gameId = "";
		}

		public async Task PlayerPressButtonAsync(string color)
		{
			if (buttonLock)
				return;

			buttonLock = true;

			playerEntry += color;
			OnPropertyChanged("DetailText");

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

			await Task.Delay(150);

			Color colorToDisplay = Color.Transparent;

			switch (color)
			{
				case "r":
					RedOpacity = 1;
					colorToDisplay = SimonSaysColors.Red;
					break;
				case "g":
					GreenOpacity = 1;
					colorToDisplay = SimonSaysColors.Green;
					break;
				case "b":
					BlueOpacity = 1;
					colorToDisplay = SimonSaysColors.Blue;
					break;
				case "y":
					YellowOpacity = 1;
					colorToDisplay = SimonSaysColors.Yellow;
					break;
			}

			SetLightColor(colorToDisplay);

			buttonLock = false;
		}

		async void GameHandler(object sender, ParticleEventArgs e)
		{
			var data = JsonConvert.DeserializeObject<SimonSaysActivity>(e.EventData.Data);

			if (data.Activity == SimonSaysActivity.EndSimon)
			{
				if (data.Value == "winner")
				{
					await Winner();
				}
				else {
					await Loser();
                }
			}

			System.Diagnostics.Debug.WriteLine($"{e.EventData.Event}: {e.EventData.Data}\n{e.EventData.DeviceId}");
		}

		#endregion
	}
}