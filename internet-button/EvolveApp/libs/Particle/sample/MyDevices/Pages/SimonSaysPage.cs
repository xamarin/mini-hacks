using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Particle;
using MyDevices.ViewModels;
namespace MyDevices.Pages
{
	public class SimonSaysPage : BasePage
	{
		Label detailLabel;
		Button red, blue, green, yellow;
		SimonSaysViewModel ViewModel;

		public SimonSaysPage(ParticleDevice device)
		{
			ViewModel = new SimonSaysViewModel(device);
			BindingContext = ViewModel;

			red = new Button { Opacity = 0.5, BackgroundColor = Color.Red, BorderRadius = 0 };
			blue = new Button { Opacity = 0.5, BackgroundColor = Color.Blue, BorderRadius = 0 };
			green = new Button { Opacity = 0.5, BackgroundColor = Color.Green, BorderRadius = 0 };
			yellow = new Button { Opacity = 0.5, BackgroundColor = Color.Yellow, BorderRadius = 0 };

			var submitButton = new Button { Text = "Submit Move" };
			var startButton = new Button { Text = "Start" };
			var clearSubmission = new Button { Text = "Clear" };

			detailLabel = new Label { HorizontalTextAlignment = TextAlignment.Center };

			var layout = new RelativeLayout();

			layout.Children.Add(red,
								xConstraint: Constraint.Constant(20),
								yConstraint: Constraint.RelativeToParent((p) => (p.Height * 0.4) - 50),
								widthConstraint: Constraint.Constant(100),
								heightConstraint: Constraint.Constant(100));
			layout.Children.Add(green,
								xConstraint: Constraint.RelativeToParent((p) => (p.Width * 0.5) - 50),
								yConstraint: Constraint.RelativeToParent((p) => p.Width * 0.3),
								widthConstraint: Constraint.Constant(100),
								heightConstraint: Constraint.Constant(100));
			layout.Children.Add(blue,
								xConstraint: Constraint.RelativeToParent((p) => p.Width - 120),
								yConstraint: Constraint.RelativeToParent((p) => (p.Height * 0.4) - 50),
								widthConstraint: Constraint.Constant(100),
								heightConstraint: Constraint.Constant(100));
			layout.Children.Add(yellow,
								xConstraint: Constraint.RelativeToParent((p) => (p.Width * 0.5) - 50),
								yConstraint: Constraint.RelativeToView(green, (p, v) => v.Y + v.Height + 20),
								widthConstraint: Constraint.Constant(100),
								heightConstraint: Constraint.Constant(100));
			layout.Children.Add(submitButton,
								xConstraint: Constraint.Constant(20),
								yConstraint: Constraint.RelativeToParent((p) => p.Height - 110),
								widthConstraint: Constraint.RelativeToParent((p) => p.Width - 40),
								heightConstraint: Constraint.Constant(40));
			layout.Children.Add(startButton,
								xConstraint: Constraint.Constant(20),
								yConstraint: Constraint.RelativeToParent((p) => p.Height - 60),
								widthConstraint: Constraint.RelativeToParent((p) => p.Width - 40),
								heightConstraint: Constraint.Constant(40));
			layout.Children.Add(detailLabel,
								xConstraint: Constraint.Constant(20),
								yConstraint: Constraint.RelativeToView(green, (p, v) => v.Y - 60),
								widthConstraint: Constraint.RelativeToParent((p) => p.Width - 40),
								heightConstraint: Constraint.Constant(40));
			layout.Children.Add(clearSubmission,
								xConstraint: Constraint.Constant(20),
								yConstraint: Constraint.Constant(20));
			Content = layout;

			submitButton.Clicked += PressButtonAsync;
			startButton.Clicked += async (object sender, EventArgs e) =>
			{
				await ViewModel.StartGame();
			};
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
				ViewModel.ClearEntry();
			};

			detailLabel.SetBinding(Label.TextProperty, "DetailText");
			red.SetBinding(Button.OpacityProperty, "RedOpacity");
			green.SetBinding(Button.OpacityProperty, "GreenOpacity");
			blue.SetBinding(Button.OpacityProperty, "BlueOpacity");
			yellow.SetBinding(Button.OpacityProperty, "YellowOpacity");

		}

		async void PressButtonAsync(object sender, EventArgs e)
		{
			var response = await ViewModel.PlayMoveAsync();
			if (response == "0")
			{
				DisplayAlert("Incorrect", "Sorry, you entered the wrong pattern and lose", "Ok");
			}
			else if (response == "2")
			{
				DisplayAlert("Winner!!!", "You Won", "Yay!");
			}
			else if (response == "-2)")
				DisplayAlert("Error", "No Game Running", "Whoops!");
		}
	}
}