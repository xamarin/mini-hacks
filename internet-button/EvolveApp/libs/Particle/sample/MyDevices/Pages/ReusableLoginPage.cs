using System;
using System.Threading.Tasks;

using Xamarin.Forms;

using MyLoginUI.Views;

namespace MyLoginUI.Pages
{
	public class ReusableLoginPage : ContentPage
	{
		#region LoginPage Properties

		string logoFileImageSource;

		public string LogoFileImageSource
		{
			get { return logoFileImageSource; }
			set
			{
				if (logoFileImageSource == value)
					return;
				logoFileImageSource = value;
				logo.Source = ImageSource.FromFile(logoFileImageSource);
			}
		}

		#endregion

		#region Internal Global References

		Image logo;
		RelativeLayout layout;
		LoginButton loginButton, newUserSignUpButton, forgotPasswordButton;
		LoginEntry loginEntry, passwordEntry;
		Label rememberMe;
		Switch saveUsername;

		bool isInitialized = false;

		#endregion

		public ReusableLoginPage()
		{
			BackgroundColor = Color.FromHex("#3498db");
			Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
			layout = new RelativeLayout();

			CreateGlobalChildren();
			AddConstraintsToChildren();

			Content = layout;
		}

		#region UI Construction Methods

		void CreateGlobalChildren()
		{
			logo = new Image();
			loginEntry = new LoginEntry
			{
				StyleId = "usernameEntry",
				Placeholder = "Username",
			};
			passwordEntry = new LoginEntry
			{
				StyleId = "passwordEntry",
				Placeholder = "Password",
				IsPassword = true,
			};
			loginButton = new LoginButton(Borders.Thin)
			{
				StyleId = "loginButton",
				Text = "Login",
			};
			newUserSignUpButton = new LoginButton(Borders.None)
			{
				StyleId = "newUserButton",
				Text = "Sign-up",
			};
			forgotPasswordButton = new LoginButton(Borders.None)
			{
				StyleId = "forgotPasswordButton",
				Text = "Forgot Password?",
			};
			rememberMe = new Label
			{
				Opacity = 0,
				Text = "Remember Me",
				TextColor = Color.White,
				FontFamily = Device.OnPlatform(
					iOS: "AppleSDGothicNeo-Light",
					Android: "Droid Sans Mono",
					WinPhone: "Comic Sans MS"),
			};
			saveUsername = new Switch
			{
				StyleId = "saveUsernameSwitch",
				IsToggled = true,
				Opacity = 0
			};

			loginButton.Clicked += (object sender, EventArgs e) =>
			{
				if (String.IsNullOrEmpty(loginEntry.Text) || String.IsNullOrEmpty(passwordEntry.Text))
				{
					DisplayAlert("Error", "You must enter a username and password.", "Okay");
					return;
				}

				Login(loginEntry.Text, passwordEntry.Text, saveUsername.IsToggled);
			};
			newUserSignUpButton.Clicked += (object sender, EventArgs e) =>
			{
				NewUserSignUp();
			};
			forgotPasswordButton.Clicked += (object sender, EventArgs e) =>
			{
				ForgotPassword();
			};
		}

		void AddConstraintsToChildren()
		{
			Func<RelativeLayout, double> getNewUserButtonWidth = (p) => newUserSignUpButton.GetSizeRequest(layout.Width, layout.Height).Request.Width;
			Func<RelativeLayout, double> getForgotButtonWidth = (p) => forgotPasswordButton.GetSizeRequest(layout.Width, layout.Height).Request.Width;
			Func<RelativeLayout, double> getRememberMeWidth = (p) => rememberMe.GetSizeRequest(layout.Width, layout.Height).Request.Width;
			Func<RelativeLayout, double> getRememberMeHeight = (p) => rememberMe.GetSizeRequest(layout.Width, layout.Height).Request.Height;
			Func<RelativeLayout, double> getSwitchWidth = (p) => saveUsername.GetSizeRequest(layout.Width, layout.Height).Request.Width;

			layout.Children.Add(
				logo,
				xConstraint: Constraint.Constant(100),
				yConstraint: Constraint.Constant(250),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 200)
			);

			layout.Children.Add(
				loginEntry,
				xConstraint: Constraint.Constant(40),
				yConstraint: Constraint.RelativeToParent(p => p.Height * 0.4),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 80)
			);
			layout.Children.Add(
				passwordEntry,
				xConstraint: Constraint.Constant(40),
				yConstraint: Constraint.RelativeToView(loginEntry, (p, v) => v.Y + v.Height + 10),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 80)
			);

			layout.Children.Add(
				rememberMe,
				xConstraint: Constraint.RelativeToParent(p => p.Width - 40 - getSwitchWidth(p) - getRememberMeWidth(p) - 20),
				yConstraint: Constraint.RelativeToView(passwordEntry, (p, v) => v.Y + v.Height + 25 + getRememberMeHeight(p) / 2)
			);
			layout.Children.Add(
				saveUsername,
				xConstraint: Constraint.RelativeToParent(p => p.Width - 40 - getSwitchWidth(p)),
				yConstraint: Constraint.RelativeToView(passwordEntry, (p, v) => v.Y + v.Height + 25)
			);

			layout.Children.Add(
				loginButton,
				xConstraint: Constraint.Constant(40),
				yConstraint: Constraint.RelativeToView(saveUsername, (p, v) => v.Y + v.Height + 25),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 80)
			);
			layout.Children.Add(
				forgotPasswordButton,
				xConstraint: Constraint.RelativeToParent(p => (p.Width / 2) - (getForgotButtonWidth(p) / 2)),
				yConstraint: Constraint.RelativeToParent(p => p.Height - 50)
			);
			layout.Children.Add(
				newUserSignUpButton,
				xConstraint: Constraint.RelativeToParent(p => (p.Width / 2) - (getNewUserButtonWidth(p) / 2)),
				yConstraint: Constraint.RelativeToView(forgotPasswordButton, (p, v) => v.Y - v.Height)
			);
		}

		#endregion

		#region Virual Methods to Expose Override Methods

		public virtual void RunAfterAnimation()
		{
		}

		public virtual void Login(string userName, string passWord, bool saveUserName)
		{
		}

		public virtual void NewUserSignUp()
		{
		}

		public virtual void ForgotPassword()
		{
		}

		#endregion

		#region Page Overrides

		protected async override void OnAppearing()
		{
			base.OnAppearing();

			if (String.IsNullOrEmpty(LogoFileImageSource))
				throw new Exception("You must set the LogoFileImageSource property to specify the logo");

			logo.Source = LogoFileImageSource;

			if (!isInitialized)
			{
				await Task.Delay(500);
				await logo.TranslateTo(0, -layout.Height * 0.3 - 10, 250);
				await logo.TranslateTo(0, -layout.Height * 0.3 + 5, 100);
				await logo.TranslateTo(0, -layout.Height * 0.3, 50);

				await logo.TranslateTo(0, -200 + 5, 100);
				await logo.TranslateTo(0, -200, 50);

				newUserSignUpButton.FadeTo(1, 250);
				forgotPasswordButton.FadeTo(1, 250);
				loginEntry.FadeTo(1, 250);
				passwordEntry.FadeTo(1, 250);
				saveUsername.FadeTo(1, 250);
				rememberMe.FadeTo(1, 250);
				await loginButton.FadeTo(1, 249);

				isInitialized = true;
				RunAfterAnimation();
			}
		}

		#endregion

		#region Extension Methods

		public void SetUsernameEntry(string username)
		{
			if (!String.IsNullOrEmpty(username))
				loginEntry.Text = username;
		}

		public void SetPasswordEntry(string password)
		{
			if (!String.IsNullOrEmpty(password))
				passwordEntry.Text = password;
		}

		#endregion
	}
}