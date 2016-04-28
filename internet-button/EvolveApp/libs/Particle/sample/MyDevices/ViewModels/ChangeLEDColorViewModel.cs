using System.Collections.Generic;
using static System.Convert;

using Xamarin.Forms;

using Particle;

namespace MyDevices.ViewModels
{
	public class RGB {
		public int R { get; set; }
		public int G { get; set; }
		public int B { get; set; }
	}

	public class ChangeLEDColorViewModel : BaseViewModel
	{
		Color colorBoxColor;
		double hue,saturation,luminosity;

		public ChangeLEDColorViewModel (Dictionary<string,string> variables)
		{
			ColorBoxColor = Color.FromRgb (0,0,0);

			Makeup.R = ToInt32 (variables["red"]);
			Makeup.G = ToInt32(variables["grn"]);
			Makeup.B = ToInt32(variables["blu"]);

			ColorBoxColor = Color.FromHsla (Makeup.R, Makeup.G, Makeup.B);
		}

		public RGB Makeup { get; set; } = new RGB();

		public void Reset()
		{
			R = 0;
			G = 0;
			B = 0;

			SetNewColor();
		}

		public string GetColorString(){
			return R +"," + G + "," + B;
		}

		public void SetNewColor() {
			ColorBoxColor = Color.FromRgb (R, G, B);
		}

		public int R {
			get { return Makeup.R; }
			set {
				if (Makeup.R == value)
					return;
				Makeup.R = value;
				OnPropertyChanged ("R");
				SetNewColor ();
			}
		}

		public int G {
			get { return Makeup.G; }
			set {
				if (value == Makeup.G)
					return;
				Makeup.G = value;
				OnPropertyChanged ("G");
				SetNewColor ();
			}
		}

		public int B {
			get { return Makeup.B; }
			set {
				if (value == Makeup.B)
					return;
				Makeup.B = value;
				OnPropertyChanged ("B");
				SetNewColor ();
			}
		}

		public Color ColorBoxColor {
			get { return colorBoxColor; }
			set {
				if (value == colorBoxColor)
					return;
				colorBoxColor = value;
				OnPropertyChanged("ColorBoxColor");
			}
		}
	}
}