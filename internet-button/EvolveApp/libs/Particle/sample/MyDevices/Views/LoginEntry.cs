using System;

using Xamarin.Forms;

namespace MyLoginUI.Views
{
	public class LoginEntry : Entry
	{
		internal LoginEntry (double opacity = 0)
		{
			BackgroundColor = Color.Transparent;
			HeightRequest = 40;
			TextColor = Color.White;
			Opacity = opacity;
			PlaceholderColor = Color.White;
        }
	}
}