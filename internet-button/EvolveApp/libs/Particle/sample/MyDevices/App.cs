using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xamarin.Forms;
using Akavache;

using Particle;

using MyDevices.Pages;
using MyDevices.Models;

namespace MyDevices
{
	public class App : Application
	{
		public static string Token = "1a0cc48d234e7242883a558b4d890e8bc726b6d3";

		public static bool HasValidToken;
		public static bool ForceLogin = false;
		public static bool IsInitialized = false;

		public static double ScreenWidth;
		public static double ScreenHeight;

		public INavigation Navigation;

		public App()
		{
			BlobCache.ApplicationName = "ParticleXamarin";

			var page = new LoginPage { LogoFileImageSource = "particle.png" };
			NavigationPage.SetHasNavigationBar(page, false);
			var navPage = new NavigationPage(page)
			{
				BarBackgroundColor = Color.FromHex("#3498db"),
				BarTextColor = Color.White,
			};
			Navigation = navPage.Navigation;
			MainPage = navPage;

			//MainPage = new TestPage();

			IsInitialized = true;
		}

		protected override async void OnResume()
		{
			base.OnResume();

			//var result = await CheckIfTokenIsValidAsync();

			//if (result == TokenResult.Invalid || result == TokenResult.Error || result == TokenResult.Empty)
			//{
			//	Navigation.PushModalAsync(new LoginPage());
			//}
		}

		public static async Task<TokenResult> CheckIfTokenIsValidAsync()
		{
			TokenResult tokenResult = TokenResult.Empty;
			try
			{
				string accessToken = await BlobCache.UserAccount.GetObject<string>("Access Token");
				string refreshToken = await BlobCache.UserAccount.GetObject<string>("Refresh Token");
				DateTime expiration = await BlobCache.UserAccount.GetObject<DateTime>("Expiration");
				ParticleCloud.AccessToken = new ParticleAccessToken(accessToken, refreshToken, expiration);

				if (expiration > DateTime.Now || accessToken == null || accessToken == "")
				{
					HasValidToken = false;
					tokenResult = TokenResult.Invalid;
				}
				else {
					HasValidToken = true;
				}
			}
			catch (KeyNotFoundException e)
			{
				return TokenResult.Error;
			}

			tokenResult = await CheckIfRefreshIsNeededAsync(tokenResult);

			return tokenResult;
		}

		public static async Task<TokenResult> CheckIfRefreshIsNeededAsync(TokenResult tokenResult = TokenResult.Empty)
		{
			try
			{
				DateTime expiration = await BlobCache.UserAccount.GetObject<DateTime>("Expiration");

				if (expiration > DateTime.Now.Subtract(TimeSpan.FromDays(7)))
				{
					tokenResult = TokenResult.NeedsRefresh;
					string access = await BlobCache.UserAccount.GetObject<string>("Access Token");
					string refresh = await BlobCache.UserAccount.GetObject<string>("Refresh Token");
					ParticleCloud.AccessToken = new ParticleAccessToken(access, refresh, expiration);

					var accessToken = await ParticleCloud.SharedInstance.RefreshTokenAsync("xamarin");
					if (accessToken != null)
					{
						await BlobCache.UserAccount.InsertObject("Access Token", accessToken.Token);
						await BlobCache.UserAccount.InsertObject("Refresh Token", accessToken.RefreshToken);
						await BlobCache.UserAccount.InsertObject("Expiration", accessToken.Expiration);
						HasValidToken = true;
						tokenResult = TokenResult.RefreshedSuccessfully;
					}
					else {
						HasValidToken = false;
						tokenResult = TokenResult.Error;
					}
				}
			}
			catch (KeyNotFoundException e)
			{
				tokenResult = TokenResult.Error;
			}

			return tokenResult;
		}
	}
}

