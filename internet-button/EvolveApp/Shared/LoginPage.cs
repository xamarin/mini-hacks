using System;
using Xamarin.Forms;
using System.Collections.Generic;
using Particle;
using Particle.Models;
using EvolveApp.Views.Pages;
using EvolveApp.ViewModels;

namespace EvolveApp
{
	public class LoginPage : ReusableLoginPage
	{
		bool isInitialized = false;

		LoginViewModel ViewModel;

		public LoginPage()
		{
			ViewModel = new LoginViewModel();
			BindingContext = ViewModel;

			indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");

            if (Device.OS == TargetPlatform.Windows)
                indicator.SetBinding(ActivityIndicator.IsVisibleProperty, "IsBusy");
        }

		protected override void OnAppearing()
		{
			base.OnAppearing();
			//Need bug fixed on Material Design for PopToRootAsync() 
			//https://bugzilla.xamarin.com/show_bug.cgi?id=36907
			if (!isInitialized)
			{
				//SetUsernameEntry("michael.watson@xamarin.com");
				//SetPasswordEntry("Da2188MW");
				isInitialized = true;
			}
		}

		public override async void Login(string userName, string passWord)
		{
			base.Login(userName, passWord);


			var response = await ViewModel.HandleLoginAsync(userName, passWord, App.Token);
			//await ParticleCloud.SharedInstance.CreateOAuthClientAsync(App.Token, "xamarin");
			//var response = await ParticleCloud.SharedInstance.LoginWithUserAsync(userName, passWord);

			if (ParticleCloud.AccessToken != null && response)
			{
				//await BlobCache.UserAccount.InsertObject("Access Token", ParticleCloud.AccessToken.Token);
				//await BlobCache.UserAccount.InsertObject("Refresh Token", ParticleCloud.AccessToken.RefreshToken);
				//await BlobCache.UserAccount.InsertObject("Expiration", ParticleCloud.AccessToken.Expiration);
				//await BlobCache.UserAccount.InsertObject("Username", userName);
				//App.HasValidToken = true;

				await Navigation.PopModalAsync();
			}
			else
				await DisplayAlert("Login Error", "Invalid Login", "Try Again");
		}

		public override async void NewUserSignUp()
		{
			base.NewUserSignUp();

			await Navigation.PushAsync(new NewUserSignUpPage());
		}

		public override async void RunAfterAnimation()
		{
			base.RunAfterAnimation();

			if (ParticleCloud.AccessToken != null)
				await Navigation.PopModalAsync();

			SetUsernameEntry("michael.watson@xamarin.com");
			SetPasswordEntry("Da2188MW");
		}
	}
}