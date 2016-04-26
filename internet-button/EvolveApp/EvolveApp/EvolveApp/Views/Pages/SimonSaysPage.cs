using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Particle;
using EvolveApp.ViewModels;
namespace EvolveApp.Pages
{
	public class SimonSaysPage : ContentPage
	{
		Button red, blue, green, yellow;
		ContentView l1, l2, l3, l4, l5, l6, l7, l8, l9, l10;
		SimonSaysViewModel ViewModel;

		public SimonSaysPage(ParticleDevice device)
		{
			ViewModel = new SimonSaysViewModel(device);
			BindingContext = ViewModel;
			BackgroundColor = AppColors.BackgroundColor;
			Title = $"{device.Name} Says";

			red = new Button { StyleId = "red", BackgroundColor = SimonSaysColors.Red, BorderRadius = 0 };
			blue = new Button { StyleId = "blue", BackgroundColor = SimonSaysColors.Blue, BorderRadius = 0 };
			green = new Button { StyleId = "green", BackgroundColor = SimonSaysColors.Green, BorderRadius = 0 };
			yellow = new Button { StyleId = "yellow", BackgroundColor = SimonSaysColors.Yellow, BorderRadius = 0 };

			l1 = new ContentView { HorizontalOptions = LayoutOptions.FillAndExpand };
			l2 = new ContentView { HorizontalOptions = LayoutOptions.FillAndExpand };
			l3 = new ContentView { HorizontalOptions = LayoutOptions.FillAndExpand };
			l4 = new ContentView { HorizontalOptions = LayoutOptions.FillAndExpand };
			l5 = new ContentView { HorizontalOptions = LayoutOptions.FillAndExpand };
			l6 = new ContentView { HorizontalOptions = LayoutOptions.FillAndExpand };
			l7 = new ContentView { HorizontalOptions = LayoutOptions.FillAndExpand };
			l8 = new ContentView { HorizontalOptions = LayoutOptions.FillAndExpand };
			l9 = new ContentView { HorizontalOptions = LayoutOptions.FillAndExpand };
			l10 = new ContentView { HorizontalOptions = LayoutOptions.FillAndExpand };

			var stackPadding = 2d;
			StackLayout lightStack = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Children = { l1, l2, l3, l4, l5, l6, l7, l8, l9, l10 },
				Padding = new Thickness(stackPadding, 0, stackPadding, 0),
				BackgroundColor = Color.Transparent
			};

			var clearSubmission = new Button
			{
				StyleId = "clearMoveButton",
				Text = "X",
				FontSize = Device.OnPlatform(10, 8, 10),
				TextColor = Color.Black,
				FontAttributes = FontAttributes.Bold,
				BorderRadius = 10,
				BackgroundColor = Color.White,
				BorderColor = Color.Black,
				BorderWidth = 1
			};
			StyledButton actionButton = new StyledButton { StyleId = "actionButton", BorderRadius = 0, TextColor = Color.White, CssStyle = "button", BorderColor = AppColors.Blue };

			var layout = new RelativeLayout();

			var buttonConstraint = Constraint.RelativeToParent((p) => (p.Width / 2) - AppSettings.Margin - AppSettings.ItemPadding / 2);

			layout.Children.Add(red,
				xConstraint: Constraint.Constant(AppSettings.Margin),
				yConstraint: Constraint.Constant(AppSettings.Margin),
				widthConstraint: buttonConstraint,
				heightConstraint: buttonConstraint
			);

			layout.Children.Add(yellow,
				xConstraint: Constraint.RelativeToParent((p) => (p.Width / 2) + AppSettings.ItemPadding / 2),
				yConstraint: Constraint.Constant(AppSettings.Margin),
				widthConstraint: buttonConstraint,
				heightConstraint: buttonConstraint
			);

			layout.Children.Add(blue,
				xConstraint: Constraint.Constant(AppSettings.Margin),
				yConstraint: Constraint.RelativeToView(red, (p, v) => v.Height + v.Y + AppSettings.ItemPadding),
				widthConstraint: buttonConstraint,
				heightConstraint: buttonConstraint
			);

			layout.Children.Add(green,
				xConstraint: Constraint.RelativeToParent((p) => (p.Width / 2) + AppSettings.ItemPadding / 2),
				yConstraint: Constraint.RelativeToView(yellow, (p, v) => v.Height + v.Y + AppSettings.ItemPadding),
				widthConstraint: buttonConstraint,
				heightConstraint: buttonConstraint
			);

			layout.Children.Add(lightStack,
				xConstraint: Constraint.Constant(AppSettings.Margin - stackPadding),
				yConstraint: Constraint.RelativeToView(blue, (p, v) => v.Height + v.Y + AppSettings.ItemPadding * 2),
				widthConstraint: Constraint.RelativeToParent((p) => p.Width - AppSettings.Margin * 2 + stackPadding * 2),
				heightConstraint: Constraint.Constant(25) // TODO calculate the square size based on the width of the view
			);
			layout.Children.Add(clearSubmission,
				xConstraint: Constraint.RelativeToParent((p) => p.Width - AppSettings.Margin - Device.OnPlatform(10, 15, 15)),
				yConstraint: Constraint.RelativeToView(lightStack, (p, v) => Device.OnPlatform(
																				v.Y - 10,
																				v.Y - 15,
																				v.Y - 15
																			)
				),
				widthConstraint: Constraint.Constant(Device.OnPlatform(25, 30, 30)),
				heightConstraint: Constraint.Constant(Device.OnPlatform(25, 30, 30))
			);

			layout.Children.Add(actionButton,
				xConstraint: Constraint.Constant(AppSettings.Margin),
				yConstraint: Constraint.RelativeToParent(p => p.Height - AppSettings.Margin - AppSettings.ButtonHeight),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - AppSettings.Margin * 2),
				heightConstraint: Constraint.Constant(50)
			);

			Content = layout;

			red.Clicked += async (object sender, EventArgs e) =>
			{
				await ViewModel.PlayerPressButtonAsync("r");
			};
			blue.Clicked += async (object sender, EventArgs e) =>
			{
				await ViewModel.PlayerPressButtonAsync("b");
			};
			green.Clicked += async (object sender, EventArgs e) =>
			{
				await ViewModel.PlayerPressButtonAsync("g");
			};
			yellow.Clicked += async (object sender, EventArgs e) =>
			{
				await ViewModel.PlayerPressButtonAsync("y");
			};
			clearSubmission.Clicked += (object sender, EventArgs e) =>
			{
				ViewModel.ClearPlayerEntry();
			};

			red.SetBinding(Button.OpacityProperty, "RedOpacity");
			green.SetBinding(Button.OpacityProperty, "GreenOpacity");
			blue.SetBinding(Button.OpacityProperty, "BlueOpacity");
			yellow.SetBinding(Button.OpacityProperty, "YellowOpacity");

			l1.SetBinding(ContentView.BackgroundColorProperty, "L1");
			l2.SetBinding(ContentView.BackgroundColorProperty, "L2");
			l3.SetBinding(ContentView.BackgroundColorProperty, "L3");
			l4.SetBinding(ContentView.BackgroundColorProperty, "L4");
			l5.SetBinding(ContentView.BackgroundColorProperty, "L5");
			l6.SetBinding(ContentView.BackgroundColorProperty, "L6");
			l7.SetBinding(ContentView.BackgroundColorProperty, "L7");
			l8.SetBinding(ContentView.BackgroundColorProperty, "L8");
			l9.SetBinding(ContentView.BackgroundColorProperty, "L9");
			l10.SetBinding(ContentView.BackgroundColorProperty, "L10");

			clearSubmission.SetBinding(Button.IsVisibleProperty, "ShowClearButton");
			actionButton.SetBinding(Button.BackgroundColorProperty, "ActionColor");
			actionButton.SetBinding(Button.TextProperty, "ActionText");
			actionButton.SetBinding(Button.CommandProperty, "ActionCommand");
		}
	}
}