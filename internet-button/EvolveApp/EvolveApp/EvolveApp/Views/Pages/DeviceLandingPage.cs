using System;

using Xamarin.Forms;
using Particle;
using EvolveApp.ViewModels;
using EvolveApp.Views.Controls;
using EvolveApp.Pages;

namespace EvolveApp.Views.Pages
{
	public class DeviceLandingPage : ContentPage
	{
		DeviceLandingPageViewModel ViewModel;

		public DeviceLandingPage(ParticleDevice device)
		{
			Title = "Mission Control";
			BackgroundColor = AppColors.BackgroundColor;
			ViewModel = new DeviceLandingPageViewModel(device);
			BindingContext = ViewModel;

			var refreshDevice = new ToolbarItem { Icon = "ic_cached_white_24dp.png" };
			var back = new ToolbarItem { Icon = "ic_clear_white.png" };
			var layout = new RelativeLayout();

			var indicator = new ActivityIndicator();
			var deviceName = new StyledLabel { CssStyle = "h1" };
			var deviceConnected = new Image { Source = "notconnected.png" };
			var currentAppLabel = new StyledLabel { CssStyle = "h2" };
			var variableWidget = new DashboardWidget();
			var functionWidget = new DashboardWidget();
			var appDescription = new StyledLabel { CssStyle = "body" };
			var interactButton = new StyledButton
			{
				StyleId = "startInteractionButton",
				Text = "START INTERACTION",
				BackgroundColor = AppColors.Green,
				CssStyle = "button",
				BorderRadius = 0,
				HeightRequest = AppSettings.ButtonHeight,
				IsEnabled = false
			};
			var flashButton = new StyledButton
			{
				StyleId = "flashBinaryButton",
				Text = "FLASH NEW APP",
				BackgroundColor = AppColors.Purple,
				CssStyle = "button",
				BorderRadius = 0,
				HeightRequest = AppSettings.ButtonHeight,
				IsEnabled = false
			};

			var boxConstraint = Constraint.RelativeToParent(p => p.Width / 2 - AppSettings.Margin - AppSettings.ItemPadding / 2);

			layout.Children.Add(deviceName,
				xConstraint: Constraint.Constant(AppSettings.Margin),
				yConstraint: Constraint.Constant(Device.OnPlatform(AppSettings.Margin, 10, 10)),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - AppSettings.Margin * 2)
			);
			layout.Children.Add(currentAppLabel,
				xConstraint: Constraint.Constant(AppSettings.Margin),
				yConstraint: Constraint.RelativeToView(deviceName, (p, v) => v.Y + v.Height + 5),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - AppSettings.Margin * 2),
				heightConstraint: Constraint.RelativeToView(deviceName, (p, v) => v.Height)
			);
			layout.Children.Add(variableWidget,
				xConstraint: Constraint.Constant(AppSettings.Margin),
				yConstraint: Constraint.RelativeToView(currentAppLabel, (p, v) => Device.OnPlatform(
																v.Y + v.Height + 5,
																v.Y + v.Height,
																v.Y + v.Height)
													  ),
				widthConstraint: boxConstraint,
				heightConstraint: boxConstraint
			);
			layout.Children.Add(functionWidget,
				xConstraint: Constraint.RelativeToParent(p => p.Width / 2 + AppSettings.ItemPadding / 2),
				yConstraint: Constraint.RelativeToView(variableWidget, (p, v) => v.Y),
				widthConstraint: boxConstraint,
				heightConstraint: boxConstraint
			);
			layout.Children.Add(new ScrollView { Content = appDescription },
				xConstraint: Constraint.Constant(AppSettings.Margin),
				yConstraint: Constraint.RelativeToView(functionWidget, (p, v) => v.Y + v.Height + 10),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - AppSettings.Margin * 2),
				heightConstraint: Constraint.RelativeToView(functionWidget, (p, v) => p.Height - v.Y - v.Height - 10 - AppSettings.Margin - 2 * AppSettings.ButtonHeight - 20)
			);
			layout.Children.Add(flashButton,
				xConstraint: Constraint.Constant(AppSettings.Margin),
				yConstraint: Constraint.RelativeToParent(p => p.Height - AppSettings.Margin - AppSettings.ButtonHeight),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - AppSettings.Margin * 2),
				heightConstraint: Constraint.Constant(AppSettings.ButtonHeight)
			);
			layout.Children.Add(interactButton,
				xConstraint: Constraint.Constant(AppSettings.Margin),
				yConstraint: Constraint.RelativeToView(flashButton, (p, v) => v.Y - AppSettings.ButtonHeight - 10),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - AppSettings.Margin * 2),
				heightConstraint: Constraint.Constant(AppSettings.ButtonHeight)
			);
			layout.Children.Add(indicator,
				xConstraint: Constraint.Constant(AppSettings.Margin),
				yConstraint: Constraint.RelativeToView(functionWidget, (p, v) => v.Y + v.Height + 10),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - AppSettings.Margin * 2),
				heightConstraint: Constraint.RelativeToView(functionWidget, (p, v) => p.Height - v.Y - v.Height - 10 - AppSettings.Margin - 2 * AppSettings.ButtonHeight - 20)
			);

			variableWidget.WidgetTitle.Text = "Variables";
			functionWidget.WidgetTitle.Text = "Functions";

			if (Device.OS == TargetPlatform.iOS)
			{
				interactButton.TextColor = Color.FromHex("#ffffff");
				flashButton.TextColor = Color.FromHex("#ffffff");
			}

			Content = layout;
			ToolbarItems.Add(refreshDevice);
			ToolbarItems.Add(back);

			indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");
			if (Device.OS != TargetPlatform.iOS && Device.OS != TargetPlatform.Android)
				indicator.SetBinding(ActivityIndicator.IsVisibleProperty, "IsBusy");

			deviceName.SetBinding(Label.TextProperty, "Device.Name");
			currentAppLabel.SetBinding(Label.TextProperty, "CurrentApp");
			deviceConnected.SetBinding(Image.IsVisibleProperty, "DeviceConnected");
			variableWidget.WidgetCount.SetBinding(Label.TextProperty, "VariableCount");
			functionWidget.WidgetCount.SetBinding(Label.TextProperty, "FunctionCount");
			interactButton.SetBinding(Button.IsEnabledProperty, "InteractButtonLock");
			flashButton.SetBinding(Button.IsEnabledProperty, "FlashButtonLock");
			refreshDevice.SetBinding(ToolbarItem.CommandProperty, "RefreshDeviceCommand");
			appDescription.SetBinding(Label.TextProperty, "AppDescription");

			interactButton.Clicked += async (object sender, EventArgs e) =>
			{
				if (ViewModel.CurrentApp.ToLower().Contains("rgb led picker"))
					await Navigation.PushAsync(new ChangeLEDColorPage(ViewModel.Device, ViewModel.variables));
				else if (ViewModel.CurrentApp.ToLower().Contains("simonsays"))
					await Navigation.PushAsync(new SimonSaysPage(ViewModel.Device));
				else
					DisplayAlert("Sorry...", "There isn't a mobile interaction with this IoT app. Try flashing either the 'Simon Says' or ' RBG LED' app.", "Ok");
			};

			flashButton.Clicked += async (object sender, EventArgs e) =>
			{
				var result = await DisplayActionSheet("Pick File to Flash", "Cancel", null, "RGB LED", "Shake LED", "Simon Says", "Follow me LED");
				if (result != "Cancel")
				{
					var success = await ViewModel.TryFlashFileAsync(result);
					if (!success)
					{
						await DisplayAlert("Error", "The Device connection timed out. Please try again once the device breaths a solid cyan light", "Ok");
					}
				}
			};

			back.Clicked += async (object sender, EventArgs e) =>
			{
				var success = await ViewModel.Device.UnclaimAsync();
				//if (success)
				Navigation.PopModalAsync(true);
			};
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();

			await ViewModel.RefreshDeviceAsync();
		}
	}
}