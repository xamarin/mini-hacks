using System.Collections.Generic;
using static System.Convert;

using Xamarin.Forms;

using Particle;
using System.Threading.Tasks;

namespace EvolveApp.ViewModels
{
	public class RGB
	{
		public int R { get; set; }
		public int G { get; set; }
		public int B { get; set; }
	}

	public class ChangeLEDColorViewModel : BaseViewModel
	{
		public ParticleDevice Device { get; internal set; }

		Color colorBoxColor;
		double hue, saturation, luminosity;

		public ChangeLEDColorViewModel(ParticleDevice device, Dictionary<string, string> variables)
		{
			ColorBoxColor = Color.FromRgb(0, 0, 0);

			Makeup.R = ToInt32(variables["red"]);
			Makeup.G = ToInt32(variables["grn"]);
			Makeup.B = ToInt32(variables["blu"]);

			ColorBoxColor = Color.FromHsla(Makeup.R, Makeup.G, Makeup.B);

			Device = device;
		}

		public RGB Makeup { get; set; } = new RGB();

		public void Reset()
		{
			R = 0;
			G = 0;
			B = 0;

			SetNewColor();
		}

		public void SetNewColor()
		{
			ColorBoxColor = Color.FromRgb(R, G, B);
		}

		public int R
		{
			get { return Makeup.R; }
			set
			{
				if (Makeup.R == value)
					return;
				Makeup.R = value;
				OnPropertyChanged("R");
				SetNewColor();
			}
		}

		public int G
		{
			get { return Makeup.G; }
			set
			{
				if (value == Makeup.G)
					return;
				Makeup.G = value;
				OnPropertyChanged("G");
				SetNewColor();
			}
		}

		public int B
		{
			get { return Makeup.B; }
			set
			{
				if (value == Makeup.B)
					return;
				Makeup.B = value;
				OnPropertyChanged("B");
				SetNewColor();
			}
		}

		public Color ColorBoxColor
		{
			get { return colorBoxColor; }
			set
			{
				if (value == colorBoxColor)
					return;
				colorBoxColor = value;
				OnPropertyChanged("ColorBoxColor");
			}
		}



		Command pushColorCommand;
		public Command PushColorCommand
		{
			get
			{
				return pushColorCommand ?? (pushColorCommand = new Command(async () => await PushColorToPhotonAsync()));
			}
		}

		Command ledsOffCommand;
		public Command LedsOffCommand
		{
			get
			{
				return ledsOffCommand ?? (ledsOffCommand = new Command(async () => await TurnOffLedsAsync()));
			}
		}

		Command lightShowCommand;
		public Command LightShowCommand
		{
			get
			{
				return lightShowCommand ?? (lightShowCommand = new Command(async () => await StartLightShowAsync()));
			}
		}

		async Task StartLightShowAsync()
		{
			IsBusy = true;

			var result = await Device.CallFunctionAsync("leds", "lightshow");

			IsBusy = false;
		}

		async Task TurnOffLedsAsync()
		{
			IsBusy = true;

			var success = await Device.CallFunctionAsync("leds", "off");
			if (success == "1")
				Reset();

            IsBusy = false;
		}

		async Task PushColorToPhotonAsync()
		{
			IsBusy = true;

			string rgb = GetColorString();
			var success = await Device.CallFunctionAsync("leds", rgb);

			IsBusy = false;
		}

		string GetColorString()
		{
			return R + "," + G + "," + B;
		}
	}
}