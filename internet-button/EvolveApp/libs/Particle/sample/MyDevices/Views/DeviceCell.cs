using System;

using Xamarin.Forms;

namespace MyDevices.Views
{
	public class DeviceCell : ViewCell
	{
		Label deviceName, lastHeard;

		public DeviceCell ()
		{
			deviceName = new Label { VerticalOptions = LayoutOptions.Center };
			lastHeard = new Label { VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.End };

			Grid layout = new Grid {
				ColumnDefinitions = new ColumnDefinitionCollection {
					new ColumnDefinition { Width = new GridLength (2, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) }
				},
				Padding = new Thickness (20, 0, 10, 0)
			};

			layout.Children.Add (deviceName, 0, 0);
			layout.Children.Add (lastHeard, 1, 0);

			deviceName.SetBinding (Label.TextProperty, "Name");
			lastHeard.SetBinding (Label.TextProperty, "LastHeard");

			View = layout;
		}
	}
}

