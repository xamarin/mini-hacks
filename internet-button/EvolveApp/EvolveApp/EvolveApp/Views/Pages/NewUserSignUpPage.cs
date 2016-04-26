using System;

using Xamarin.Forms;
using Particle;
using EvolveApp.ViewModels;

namespace EvolveApp
{
	public class NewUserSignUpPage : ContentPage
	{
		BaseViewModel ViewModel;
		//LoginEntry usernameEntry, passwordEntry, reEnterPasswordEntry;

		public NewUserSignUpPage()
		{
			ViewModel = new BaseViewModel();
			BindingContext = ViewModel;

			RelativeLayout relativeLayout = new RelativeLayout();
			ActivityIndicator indicator = new ActivityIndicator();

			var usernameEntry = new LoginEntry(1)
			{
				StyleId = "newUsernameEntry",
				Placeholder = "Email",
				Keyboard = Keyboard.Email,
				HorizontalTextAlignment = TextAlignment.Center,
			};
			var passwordEntry = new LoginEntry(1)
			{
				StyleId = "newPasswordEntry",
				Placeholder = "Password",
				IsPassword = true,
				HorizontalTextAlignment = TextAlignment.Center
			};
			var reEnterPasswordEntry = new LoginEntry(1)
			{
				StyleId = "reEnterPasswordEntry",
				Placeholder = "Re-enter password",
				IsPassword = true,
				HorizontalTextAlignment = TextAlignment.Center,
			};
			var saveUsernameButton = new StyledButton
			{
				CssStyle = "button",
				StyleId = "saveUsernameButton",
				Text = "Save Username",
				TextColor = Color.White,
				BackgroundColor = AppColors.Green,
				BorderRadius = 0,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.EndAndExpand
			};
			var usernameLabel = new StyledLabel
			{
				CssStyle = "h2",
				Text = "Please enter username",
				HorizontalOptions = LayoutOptions.Start
			};
			var passwordLabel = new StyledLabel
			{
				CssStyle = "h2",
				Text = "Please enter password",
				HorizontalOptions = LayoutOptions.Start
			};
			var reEnterPasswordLabel = new StyledLabel
			{
				CssStyle = "h2",
				Text = "Please re-enter password",
				HorizontalOptions = LayoutOptions.Start
			};

			relativeLayout.Children.Add(usernameLabel,
				xConstraint: Constraint.Constant(AppSettings.Margin),
				yConstraint: Constraint.Constant(AppSettings.Margin),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 2 * AppSettings.Margin)
			);
			relativeLayout.Children.Add(usernameEntry,
				xConstraint: Constraint.Constant(AppSettings.Margin),
				yConstraint: Constraint.RelativeToView(usernameLabel, (p, v) => v.Y + v.Height + AppSettings.ItemPadding),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 2 * AppSettings.Margin)
			);
			relativeLayout.Children.Add(passwordLabel,
				xConstraint: Constraint.Constant(AppSettings.Margin),
				yConstraint: Constraint.RelativeToView(usernameEntry, (p, v) => v.Y + v.Height + AppSettings.ItemPadding),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 2 * AppSettings.Margin)
			);
			relativeLayout.Children.Add(passwordEntry,
				xConstraint: Constraint.Constant(AppSettings.Margin),
				yConstraint: Constraint.RelativeToView(passwordLabel, (p, v) => v.Y + v.Height + AppSettings.ItemPadding),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 2 * AppSettings.Margin)
			);
			relativeLayout.Children.Add(reEnterPasswordLabel,
				xConstraint: Constraint.Constant(AppSettings.Margin),
				yConstraint: Constraint.RelativeToView(passwordEntry, (p, v) => v.Y + v.Height + AppSettings.ItemPadding),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 2 * AppSettings.Margin)
			);
			relativeLayout.Children.Add(reEnterPasswordEntry,
				xConstraint: Constraint.Constant(AppSettings.Margin),
				yConstraint: Constraint.RelativeToView(reEnterPasswordLabel, (p, v) => v.Y + v.Height + AppSettings.ItemPadding),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 2 * AppSettings.Margin)
			);
			relativeLayout.Children.Add(saveUsernameButton,
				xConstraint: Constraint.Constant(AppSettings.Margin),
				yConstraint: Constraint.RelativeToParent(p => p.Height - AppSettings.Margin - AppSettings.ButtonHeight),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 2 * AppSettings.Margin)
			);
			relativeLayout.Children.Add(indicator,
				xConstraint: Constraint.Constant(AppSettings.Margin),
				yConstraint: Constraint.RelativeToView(reEnterPasswordEntry, (p, v) => v.Y + v.Height),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 2 * AppSettings.Margin),
				heightConstraint: Constraint.RelativeToView(reEnterPasswordEntry, (p, v) => p.Height - v.Y - v.Height - AppSettings.Margin - AppSettings.ButtonHeight)
			);

			Content = relativeLayout;

			indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");

			if (Device.OS == TargetPlatform.Windows)
				indicator.SetBinding(ActivityIndicator.IsVisibleProperty, "IsBusy");

			saveUsernameButton.Clicked += async (object sender, EventArgs e) =>
			{
				ViewModel.IsBusy = true;

				if (String.IsNullOrEmpty(usernameEntry.Text))
				{
					DisplayAlert("Error", "Username cannot be blank", "Ok");
					ViewModel.IsBusy = false;
					return;
				}
				else if (!usernameEntry.Text.Contains("@"))
				{
					DisplayAlert("Error", "Username is invalid. Please enter a valid email address.", "Ok");
					ViewModel.IsBusy = false;
					return;
				}
				else if (String.IsNullOrEmpty(passwordEntry.Text))
				{
					DisplayAlert("Error", "Password cannot be blank", "Ok");
					ViewModel.IsBusy = false;
					return;
				}
				else if (String.IsNullOrEmpty(reEnterPasswordEntry.Text))
				{
					DisplayAlert("Error", "Please re-enter your password", "Ok");
					ViewModel.IsBusy = false;
					return;
				}
				else if (passwordEntry.Text != reEnterPasswordEntry.Text)
				{
					DisplayAlert("Error", "Passwords don't match.", "Ok");
					ViewModel.IsBusy = false;
					return;

				}

				var result = await ParticleCloud.SharedInstance.SignupWithUserAsync(usernameEntry.Text, passwordEntry.Text);
				ViewModel.IsBusy = false;

				if (result == "Success")
				{
					await DisplayAlert("Success", "Your account was successfully created. Now let's login.", "Ok");
					await Navigation.PopAsync();
				}
				else {
					DisplayAlert("Error", $"{result}", "Ok");
				}
			};
		}
	}
}