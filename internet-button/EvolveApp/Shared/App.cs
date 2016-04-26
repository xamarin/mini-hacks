using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using EvolveApp.Pages;
using Particle;

namespace EvolveApp
{
	public class App : Application
	{
		public static string Token = "7770f37e1e767f9eb4a72ca81a933b4026957e02";
		public static string RefreshToken = "9b5bf63ceca02eb5c1513b3848595e90ad9e8fd9";
		public DateTime Expiration = DateTime.Parse("07/11/2016");
		public static string User = "michael.watson@xamarin.com";

#if DEBUG
		public static ScanDevicePage XTCBackDoor;
#endif

		public static string CSS
		{
			get
			{
				// Load the CSS
				var assembly = typeof(App).GetTypeInfo().Assembly;

#if __ANDROID__
				var stream = assembly.GetManifestResourceStream("EvolveApp.Droid.Style.css");

				string css = "";

				using (var reader = new System.IO.StreamReader(stream))
				{
					css = reader.ReadToEnd();
				}

				return css;
#endif

#if __IOS__
				var stream = assembly.GetManifestResourceStream("EvolveApp.iOS.Style.css");

				string css = "";

				using (var reader = new System.IO.StreamReader(stream))
				{
					css = reader.ReadToEnd();
				}

				return css;
#endif
				return "";
			}
		}

		public App()
		{
			var page = new ScanDevicePage();
			NavigationPage.SetHasNavigationBar(page, false);
			var navPage = new NavigationPage(page);

			MainPage = navPage;

#if __IOS__
			navPage.BarBackgroundColor = AppColors.Blue;
			navPage.BarTextColor = Color.White;
#endif
			// The root page of your application
			//MainPage = navPage;
		}

		protected override void OnStart()
		{
			// Handle when your app starts
			//ParticleCloud.AccessToken = new ParticleAccessToken(Token, RefreshToken, Expiration);
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
			//ParticleCloud.AccessToken = new ParticleAccessToken(Token, RefreshToken, Expiration);
		}
	}
}
