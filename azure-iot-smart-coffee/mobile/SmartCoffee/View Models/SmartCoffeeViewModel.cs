using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartCoffee
{
	public class SmartCoffeeViewModel : BaseViewModel
	{
		const string TURN_ON_EVENT_MESSAGE = "Turn Coffee Maker On";
		const string TURN_OFF_EVENT_MESSAGE = "Turn Coffee Maker Off";

		SmartCoffeeService smartCoffeesService;
		bool isCoffeeBrewing = false;

		Command brewCoffeeCommand;
		public Command BrewCoffeeCommand
		{
			get
			{
				return brewCoffeeCommand ??
					(brewCoffeeCommand = new Command(async () =>
				  {
					await ExecuteBrewCoffeeCommandAsync();
				  }, () =>
				  {
					  return !IsBusy;
				  }));
			}
		}

		private string buttonText = "Start Brewing";
		public string ButtonText
		{
			get { return buttonText; }
			set { buttonText = value; OnPropertyChanged("ButtonText"); }
		}

		async Task ExecuteBrewCoffeeCommandAsync()
		{
			if (IsBusy)
				return;

			IsBusy = true;

			try
			{
				ButtonText = isCoffeeBrewing ? "Start Brewing" : "Stop Brewing";

				// TODO #5: Send a message to Azure IoT Hub.

				isCoffeeBrewing = !isCoffeeBrewing;
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Oops! Something went wrong: {ex.Message}");
			}

			IsBusy = false;
		}
	}
}
