using System;

using Xamarin.Forms;

namespace MyDevices.Views
{
	public class DeviceListViewHeader : Grid
	{
		public DeviceListViewHeader ()
		{
			var deviceName = new Label { 
				VerticalOptions = LayoutOptions.Center,
				Text = "Device Name"
			};
			var lastHeard = new Label { 
				VerticalOptions = LayoutOptions.Center, 
				HorizontalTextAlignment = TextAlignment.End,
				Text = "Last Head On"
			};

			ColumnDefinitions = new ColumnDefinitionCollection {
				new ColumnDefinition { Width = new GridLength (2, GridUnitType.Star) },
				new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) }
			};

			Children.Add (deviceName, 0, 0);
			Children.Add (lastHeard, 1, 0);
			Padding = new Thickness(10, 0, 5, 0);
		}
	}
}