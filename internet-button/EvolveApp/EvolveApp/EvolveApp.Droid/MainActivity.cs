using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using TextStyles.Android;
using Xamarin.Forms;
using Plugin.Toasts;

namespace EvolveApp.Droid
{
	[Activity(Label = "EvolveApp",
			  Icon = "@drawable/icon",
			  ScreenOrientation = ScreenOrientation.Portrait,
			  ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
			  Theme = "@style/MyTheme",
			  MainLauncher = true
			 )]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			var textCheck = TextStyle.Instance.GetFont("SegoeUI-Regular");

			if (textCheck == null)
			{
				// Initialise the TextStyle Class
				TextStyle.Instance.AddFont("SegoeUI-Regular", "SegoeUIRegular.ttf");
				TextStyle.Instance.AddFont("SegoeUI-Light", "SegoeUILight.ttf");
				TextStyle.Instance.SetCSS(App.CSS);
			}

			global::Xamarin.Forms.Forms.Init(this, bundle);
			ZXing.Net.Mobile.Forms.Android.Platform.Init();
			DependencyService.Register<ToastNotificatorImplementation>(); // Register your dependency

			if (((int)Android.OS.Build.VERSION.SdkInt) >= 20)
			{
				ToastNotificatorImplementation.Init(this, new FixNavBar());
			}
			else {
				ToastNotificatorImplementation.Init(this);
			}

			LoadApplication(new App());
		}
	}
}