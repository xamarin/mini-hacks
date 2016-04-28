using System;

using Xamarin.Forms;

namespace MyDevices.Views
{
	public class DeviceViewCell : ViewCell
	{
		Label deviceName, functions,variables;
		Image connected;

		public DeviceViewCell ()
		{
			deviceName = new Label ();
			connected = new Image { Source = "ic_signal_wifi_off_black_24dp.png" };
			functions = new Label { FontSize = 12 };
			variables = new Label { FontSize = 12 };

			RelativeLayout relativeLayout = new RelativeLayout {
				VerticalOptions = LayoutOptions.FillAndExpand,
			};
					
			relativeLayout.Children.Add (deviceName, 
				Constraint.RelativeToParent ((parent) => {
					return parent.Width * 0.05;
				}),
				Constraint.RelativeToParent ((parent) => {
					return parent.Height * 0.1;
				}),
				Constraint.RelativeToParent ((parent) => {
					return parent.Width / 2;
				}),
				Constraint.RelativeToParent ((parent) => {
					return parent.Height * 0.4;
				}));

			relativeLayout.Children.Add (connected,
				Constraint.RelativeToParent ((parent) => {
					return parent.Width * 0.7;
				}),
				Constraint.RelativeToParent ((parent) => {
					return parent.Height * 0.2;
				}),
				Constraint.RelativeToParent ((parent) => {
					return parent.Width * 0.5;
				}),
				Constraint.RelativeToParent ((parent) => {
					return parent.Height * 0.5;
				}));
			relativeLayout.Children.Add (functions, 
				Constraint.RelativeToParent ((parent) => {
					return parent.Width * 0.05;
				}), 
				Constraint.RelativeToView (deviceName,(parent,deviceName) => {
					return deviceName.Height + deviceName.Y + 5;
				}), 
				null, 
				Constraint.RelativeToParent ((parent) => {
					return parent.Height * 0.40;
				}));
			relativeLayout.Children.Add (variables, 
				Constraint.RelativeToView (functions,(parent,functions) => {
					return functions.Width + functions.X + 10;
				}), 
				Constraint.RelativeToView (deviceName,(parent,deviceName) => {
					return deviceName.Height + deviceName.Y + 5;
				}), 
				null, 
				Constraint.RelativeToParent ((parent) => {
					return parent.Height * 0.40;
				}));
//				Constraint.RelativeToParent ((parent) => {
//					return parent.Width * 2;
//				}),
//				Constraint.RelativeToParent ((parent) => {
//					return parent.Height * 0.6;
//				}),
//				Constraint.RelativeToParent((parent) => {
//					return parent.Width * 0.8;
//				}),
//				Constraint.RelativeToParent ((parent) => {
//					return parent.Height * 0.4;
//				}));
//			relativeLayout.Children.Add (variables,
//				Constraint.RelativeToView (functions,(parent,functions) => {
//					return parent.Width / 2 + functions.Width + 10;
//				}),
//				Constraint.RelativeToView (connected,(parent,connected) => {
//					return parent.Height * 0.2 + connected.Height;
//				}),
//				Constraint.RelativeToParent((parent) => {
//					return parent.Width * 0.3;
//				}),
//				Constraint.RelativeToParent ((parent) => {
//					return parent.Height * 0.2;
//				}));
			View = relativeLayout;
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			var test = this.BindingContext;
			deviceName.SetBinding (Label.TextProperty, "Name");
			connected.SetBinding (Image.SourceProperty, "ConnectedImageSource");
			functions.SetBinding (Label.TextProperty, "FunctionsString");
//				, BindingMode.OneWay, null, "Functions: {0}");
			variables.SetBinding (Label.TextProperty, "VariablesString");
//				, BindingMode.OneWay, null, "Variables: {0}");
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
			deviceName.RemoveBinding(Label.TextProperty);

		}
	}
}

