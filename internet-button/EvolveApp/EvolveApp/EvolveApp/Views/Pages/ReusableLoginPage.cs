using System;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace EvolveApp
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
		StyledButton loginButton, newUserSignUpButton;
		StyledLabel loginLabel;
		LoginEntry loginEntry, passwordEntry;
		public ActivityIndicator indicator = new ActivityIndicator();

		bool isInitialized = false;

		#endregion

		public ReusableLoginPage()
		{
			BackgroundColor = AppColors.BackgroundColor;
			layout = new RelativeLayout { BackgroundColor = AppColors.BackgroundColor };

			CreateGlobalChildren();
			AddConstraintsToChildren();

			Content = layout;
		}

		#region UI Construction Methods

		void CreateGlobalChildren()
		{
			logo = new Image { HeightRequest = 30, Aspect = Aspect.AspectFit };
			loginLabel = new StyledLabel
			{
				CssStyle = "h1",
				Text = "Login with your particle username",
				Opacity = 0
			};
			loginEntry = new LoginEntry
			{
				StyleId = "usernameEntry",
				Placeholder = "Email",
				Keyboard = Keyboard.Email
			};
			passwordEntry = new LoginEntry
			{
				StyleId = "passwordEntry",
				Placeholder = "Password",
				IsPassword = true,
			};
			loginButton = new StyledButton
			{
				StyleId = "loginButton",
				Text = "LOGIN",
				TextColor = Color.White,
				BackgroundColor = AppColors.Green,
				CssStyle = "button",
				BorderRadius = 0,
				HeightRequest = AppSettings.ButtonHeight,
				Opacity = 0
			};
			newUserSignUpButton = new StyledButton
			{
				StyleId = "newUserButton",
				Text = "SIGN-UP",
				TextColor = Color.White,
				BackgroundColor = AppColors.Purple,
				CssStyle = "button",
				BorderRadius = 0,
				HeightRequest = AppSettings.ButtonHeight,
				Opacity = 0
			};

			loginButton.Clicked += (object sender, EventArgs e) =>
			{
				if (String.IsNullOrEmpty(loginEntry.Text) || String.IsNullOrEmpty(passwordEntry.Text))
				{
					DisplayAlert("Error", "You must enter a username and password.", "Okay");
					return;
				}

				Login(loginEntry.Text, passwordEntry.Text);
			};
			newUserSignUpButton.Clicked += (object sender, EventArgs e) =>
			{
				NewUserSignUp();
			};
		}

		void AddConstraintsToChildren()
		{
			layout.Children.Add(
				logo,
				xConstraint: Constraint.Constant(AppSettings.Margin),
				yConstraint: Constraint.Constant(250),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 2 * AppSettings.Margin)
			);
			layout.Children.Add(
				loginLabel,
				xConstraint: Constraint.Constant(AppSettings.Margin),
				yConstraint: Constraint.Constant(2 * AppSettings.Margin),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 2 * AppSettings.Margin)
			);
			layout.Children.Add(
				loginEntry,
				xConstraint: Constraint.Constant(AppSettings.Margin),
				yConstraint: Constraint.RelativeToView(loginLabel, (p, v) => v.Height + v.Y + AppSettings.Margin),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 2 * AppSettings.Margin)
			);
			layout.Children.Add(
				passwordEntry,
				xConstraint: Constraint.Constant(AppSettings.Margin),
				yConstraint: Constraint.RelativeToView(loginEntry, (p, v) => v.Y + v.Height + 10),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 2 * AppSettings.Margin)
			);
			layout.Children.Add(
				indicator,
				xConstraint: Constraint.Constant(AppSettings.Margin),
				yConstraint: Constraint.RelativeToView(passwordEntry, (p, v) => v.Y + v.Height + 5),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 2 * AppSettings.Margin),
				heightConstraint: Constraint.RelativeToView(passwordEntry, (p, v) => p.Height - v.Y - v.Height - AppSettings.Margin - 2 * AppSettings.ButtonHeight - 10)

			);
			layout.Children.Add(
				loginButton,
				xConstraint: Constraint.Constant(AppSettings.Margin),
				yConstraint: Constraint.RelativeToView(newUserSignUpButton, (p, v) => v.Y - AppSettings.ButtonHeight - 10),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 2 * AppSettings.Margin)
			);
			layout.Children.Add(
				newUserSignUpButton,
				xConstraint: Constraint.Constant(AppSettings.Margin),
				yConstraint: Constraint.RelativeToParent(p => p.Height - AppSettings.Margin - AppSettings.ButtonHeight),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 2 * AppSettings.Margin)
			);
		}

		#endregion

		#region Virual Methods to Expose Override Methods

		public virtual void RunAfterAnimation()
		{
		}

		public virtual void Login(string userName, string passWord)
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

		uint fadeTime = 100;

		protected async override void OnAppearing()
		{
			base.OnAppearing();

			if (String.IsNullOrEmpty(LogoFileImageSource))
				throw new Exception("You must set the LogoFileImageSource property to specify the logo");

			logo.Source = LogoFileImageSource;

			if (!isInitialized)
			{
				await Task.Delay(500);
				await logo.TranslateTo(0, -(layout.Height - logo.Y), 250);

				loginLabel.FadeTo(1, fadeTime);
				newUserSignUpButton.FadeTo(1, fadeTime);
				loginEntry.FadeTo(1, fadeTime);
				passwordEntry.FadeTo(1, fadeTime);
				await loginButton.FadeTo(1, fadeTime);

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