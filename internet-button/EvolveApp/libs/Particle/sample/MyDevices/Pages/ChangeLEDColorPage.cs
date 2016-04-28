using System;
using System.Collections.Generic;

using Xamarin.Forms;

using Particle;
using MyDevices.ViewModels;
using MyDevices.Interfaces;

namespace MyDevices.Pages
{
	public class ChangeLEDColorPage : ContentPage
	{
		Slider redSlider, greenSlider, blueSlider;
		Button push, lightShow;
		BoxView colorBox;
		ToolbarItem off, refresh;

		ParticleDevice particleDevice;

		ChangeLEDColorViewModel ViewModel;

		public ChangeLEDColorPage (ParticleDevice device, Dictionary<string,string> variables)
		{
			var labelStyle = new Style(typeof(Label))
			{
				Setters = {
					new Setter { Property = Label.TextColorProperty, Value = Color.White },
					new Setter { Property = Label.FontFamilyProperty, Value = Device.OnPlatform("AppleSDGothicNeo-Light", "Droid Sans Mono", "Comic Sans MS") },
				}
			};

			var buttonStyle = new Style(typeof(Button))
			{
				Setters = {
					new Setter { Property = Button.TextColorProperty, Value = Color.White },
					new Setter { Property = Button.FontFamilyProperty, Value = Device.OnPlatform("AppleSDGothicNeo-Light", "Droid Sans Mono", "Comic Sans MS") },
				}
			};
			Resources = new ResourceDictionary();
			Resources.Add(labelStyle);
			Resources.Add(buttonStyle);

			ViewModel = new ChangeLEDColorViewModel (variables);
			BindingContext = ViewModel;

			redSlider = new Slider {
				Minimum = 0,
				Maximum = 255,
				Value = 0,
			};
			greenSlider = new Slider {
				Minimum = 0,
				Maximum = 255,
				Value = 0,
			};
			blueSlider = new Slider {
				Minimum = 0,
				Maximum = 255,
				Value = 0,
			};
			push = new Button {
				Text = "Push To Photon"
			};
			lightShow = new Button {
				Text = "Start a light show!"
			};

			off = new ToolbarItem { Text = "Off" };
			refresh = new ToolbarItem { Text = "Refresh" };

			StackLayout layout = new StackLayout
			{
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Padding = new Thickness(20, 0, 20, 0)
			};
			layout.Children.Add (new Label{ Text = "R Value", HorizontalOptions = LayoutOptions.Start });
			layout.Children.Add (redSlider);
			layout.Children.Add (new Label{ Text = "G Value", HorizontalOptions = LayoutOptions.Start });
			layout.Children.Add (greenSlider);
			layout.Children.Add (new Label{ Text = "B Value", HorizontalOptions = LayoutOptions.Start });
			layout.Children.Add (blueSlider);
			layout.Children.Add (push);
			layout.Children.Add (lightShow);

			Content = layout;
			Padding = new Thickness (10, 0, 10, 0);

			push.Clicked += PushColorToPhoton;
			lightShow.Clicked += StartLightShow;
			off.Clicked += TurnOffLeds;
			refresh.Clicked += RefreshFile;

			redSlider.SetBinding (Slider.ValueProperty, "R", BindingMode.TwoWay);
			greenSlider.SetBinding (Slider.ValueProperty, "G", BindingMode.TwoWay);
			blueSlider.SetBinding (Slider.ValueProperty, "B", BindingMode.TwoWay);
			this.SetBinding (ContentPage.BackgroundColorProperty, "ColorBoxColor");

			ToolbarItems.Add (off);
			ToolbarItems.Add (refresh);

			particleDevice = device;
		}

		async void StartLightShow (object sender, EventArgs e)
		{
			var success = await particleDevice.CallFunctionAsync("leds","lightshow");
			if (success != "1")
				DisplayAlert("Error", "Something went wrong\nError Response: " + success, "Dang");
		}

		async void PushColorToPhoton (object sender, EventArgs e)
		{
			string rgb = ViewModel.GetColorString ();

			var success = await particleDevice.CallFunctionAsync("leds",rgb);
			if (success != "1")
				DisplayAlert("Error", "Something went wrong\nError Response: " + success, "Dang");
		}

		async void TurnOffLeds (object sender, EventArgs e)
		{
			var success = await particleDevice.CallFunctionAsync("leds","off");
			if (success != "1")
				DisplayAlert("Error", "Something went wrong\nError Response: " + success, "Dang");
			else {
				ViewModel.Reset();
			}
		}

		async void RefreshFile(object sender, EventArgs e)
		{
			var fileBytes = DependencyService.Get<IDirectory>().GetByteArrayFromFile("firmware");

			await particleDevice.FlashFilesAsync (fileBytes, "firmware.bin");
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			ViewModel.SetNewColor();
		}
	}
}