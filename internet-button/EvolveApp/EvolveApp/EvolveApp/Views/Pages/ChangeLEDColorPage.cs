using System;
using System.Collections.Generic;

using Xamarin.Forms;

using Particle;
using EvolveApp.ViewModels;

namespace EvolveApp
{
	public class ChangeLEDColorPage : ContentPage
	{
		//Slider redSlider, greenSlider, blueSlider;
		//StyledButton push, lightShow;
		//BoxView colorBox;
		//ToolbarItem off;
		//ActivityIndicator indicator;
		//ParticleDevice particleDevice;

		ChangeLEDColorViewModel ViewModel;

		public ChangeLEDColorPage(ParticleDevice device, Dictionary<string, string> variables)
		{
			Title = "RBG LED";
			BackgroundColor = AppColors.BackgroundColor;
			ViewModel = new ChangeLEDColorViewModel(device, variables);
			BindingContext = ViewModel;

			var indicator = new ActivityIndicator { HeightRequest = Device.OnPlatform(50, 30, 50) };
            var colorPreview = new BoxView { HeightRequest = 100 };
			var redSlider = new Slider { StyleId = "redSlider", Minimum = 0, Maximum = 255, Value = 0 };
			var greenSlider = new Slider { StyleId = "greenSlider", Minimum = 0, Maximum = 255, Value = 0 };
			var blueSlider = new Slider { StyleId = "blueSlider", Minimum = 0, Maximum = 255, Value = 0 };
			var push = new StyledButton
			{
				StyleId = "pushRGBvalueButton",
				Text = "PUSH TO PHOTON",
				BackgroundColor = AppColors.Blue,
				CssStyle = "button",
				BorderRadius = 0,
				HeightRequest = AppSettings.ButtonHeight,
				VerticalOptions = LayoutOptions.Fill
			};
			var lightShow = new StyledButton
			{
				StyleId = "startLightShowButton",
				Text = "START A LIGHT SHOW",
				BackgroundColor = AppColors.Green,
				CssStyle = "button",
				BorderRadius = 0,
				HeightRequest = AppSettings.ButtonHeight,
				VerticalOptions = LayoutOptions.End
			};
            var previewLabel = new StyledLabel { CssStyle = "body", Text = "Color Preview:", HorizontalOptions = LayoutOptions.Start };
            var rLabel = new StyledLabel { CssStyle = "body", Text = "R Value", HorizontalOptions = LayoutOptions.Start };
            var gLabel = new StyledLabel { CssStyle = "body", Text = "G Value", HorizontalOptions = LayoutOptions.Start };
            var bLabel = new StyledLabel { CssStyle = "body", Text = "B Value", HorizontalOptions = LayoutOptions.Start };

            Func<RelativeLayout, View, double> layoutAfterPrevious = (p, v) => Device.OnPlatform(
                                                                                    v.Y + v.Height + 5,
                                                                                    v.Y + v.Height + 5,
                                                                                    v.Y + v.Height + 2);

            RelativeLayout relativeLayout = new RelativeLayout();
            relativeLayout.Children.Add(previewLabel,
                xConstraint: Constraint.Constant(AppSettings.Margin),
                yConstraint: Constraint.Constant(10)
            );
            relativeLayout.Children.Add(colorPreview,
                xConstraint: Constraint.Constant(AppSettings.Margin),
                yConstraint: Constraint.RelativeToView(previewLabel, layoutAfterPrevious),
                widthConstraint: Constraint.RelativeToParent(p => p.Width - AppSettings.Margin * 2),
                heightConstraint: Constraint.Constant(AppSettings.ButtonHeight * 2)
            );
            relativeLayout.Children.Add(rLabel,
                xConstraint: Constraint.Constant(AppSettings.Margin),
                yConstraint: Constraint.RelativeToView(colorPreview, layoutAfterPrevious)
            );
            relativeLayout.Children.Add(redSlider,
                xConstraint: Constraint.Constant(AppSettings.Margin),
                yConstraint: Constraint.RelativeToView(rLabel, layoutAfterPrevious),
                widthConstraint: Constraint.RelativeToParent(p => p.Width - AppSettings.Margin * 2)
            );
            relativeLayout.Children.Add(gLabel,
                xConstraint: Constraint.Constant(AppSettings.Margin),
                yConstraint: Constraint.RelativeToView(redSlider, layoutAfterPrevious)
            );
            relativeLayout.Children.Add(greenSlider,
                xConstraint: Constraint.Constant(AppSettings.Margin),
                yConstraint: Constraint.RelativeToView(gLabel, layoutAfterPrevious),
                widthConstraint: Constraint.RelativeToParent(p => p.Width - AppSettings.Margin * 2)
            );
            relativeLayout.Children.Add(bLabel,
                xConstraint: Constraint.Constant(AppSettings.Margin),
                yConstraint: Constraint.RelativeToView(greenSlider, layoutAfterPrevious)
            );
            relativeLayout.Children.Add(blueSlider,
                xConstraint: Constraint.Constant(AppSettings.Margin),
                yConstraint: Constraint.RelativeToView(bLabel, layoutAfterPrevious),
                widthConstraint: Constraint.RelativeToParent(p => p.Width - AppSettings.Margin * 2)
            );
            relativeLayout.Children.Add(indicator,
                xConstraint: Constraint.Constant(AppSettings.Margin),
                yConstraint: Constraint.RelativeToView(blueSlider, layoutAfterPrevious),
                widthConstraint: Constraint.RelativeToParent(p => p.Width - AppSettings.Margin * 2),
                heightConstraint: Constraint.Constant(Device.OnPlatform(50,50,25))
            );
            relativeLayout.Children.Add(lightShow,
                xConstraint: Constraint.Constant(AppSettings.Margin),
                yConstraint: Constraint.RelativeToParent(p => p.Height - AppSettings.Margin - AppSettings.ButtonHeight),
                widthConstraint: Constraint.RelativeToParent(p => p.Width - AppSettings.Margin * 2),
                heightConstraint: Constraint.Constant(AppSettings.ButtonHeight)
            );
            relativeLayout.Children.Add(push,
                xConstraint: Constraint.Constant(AppSettings.Margin),
                yConstraint: Constraint.RelativeToView(lightShow, (p, v) => v.Y - AppSettings.ButtonHeight - 10),
                widthConstraint: Constraint.RelativeToParent(p => p.Width - AppSettings.Margin * 2),
                heightConstraint: Constraint.Constant(AppSettings.ButtonHeight)
            );
            


   //         StackLayout layout = new StackLayout
			//{
			//	VerticalOptions = LayoutOptions.CenterAndExpand,
			//	Padding = new Thickness(AppSettings.Margin, 10, AppSettings.Margin, AppSettings.Margin),
			//	Spacing = 10,
			//	Children = {
			//		new StyledLabel { CssStyle = "body", Text = "Color Preview:", HorizontalOptions = LayoutOptions.Start },
			//		colorPreview,
			//		new StyledLabel { CssStyle = "body", Text = "R Value", HorizontalOptions = LayoutOptions.Start },
			//		redSlider,
			//		new StyledLabel { CssStyle = "body", Text = "G Value", HorizontalOptions = LayoutOptions.Start },
			//		greenSlider,
			//		new StyledLabel { CssStyle = "body", Text = "B Value", HorizontalOptions = LayoutOptions.Start },
			//		blueSlider,
			//		indicator,
			//		push,
			//		lightShow
			//	}
			//};

			if (Device.OS == TargetPlatform.iOS)
			{
				push.FontFamily = "SegoeUI-Light";
				push.FontSize = 16;
				push.TextColor = Color.FromHex("#ffffff");

				lightShow.FontFamily = "SegoeUI-Light";
				lightShow.FontSize = 16;
				lightShow.TextColor = Color.FromHex("#ffffff");
			}

            var off = new ToolbarItem { Text = "LEDs Off" };

            Content = relativeLayout;


            indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");
            if (Device.OS != TargetPlatform.iOS && Device.OS != TargetPlatform.Android)
                indicator.SetBinding(ActivityIndicator.IsVisibleProperty, "IsBusy");

            redSlider.SetBinding(Slider.ValueProperty, "R", BindingMode.TwoWay);
			greenSlider.SetBinding(Slider.ValueProperty, "G", BindingMode.TwoWay);
			blueSlider.SetBinding(Slider.ValueProperty, "B", BindingMode.TwoWay);
			colorPreview.SetBinding(BoxView.BackgroundColorProperty, "ColorBoxColor");
			push.SetBinding(Button.CommandProperty, "PushColorCommand");
			lightShow.SetBinding(Button.CommandProperty, "LightShowCommand");
			off.SetBinding(ToolbarItem.CommandProperty, "LedsOffCommand");

			ToolbarItems.Add(off);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			ViewModel.SetNewColor();
		}
	}
}