using System;

using Xamarin.Forms;

namespace EvolveApp
{
	public class LoginEntry : Entry
	{
		internal LoginEntry(double opacity = 0)
		{
			BackgroundColor = Color.Transparent;
			HeightRequest = 40;
			Opacity = opacity;
			PlaceholderColor = Color.FromHex("#778687");
		}
	}
}