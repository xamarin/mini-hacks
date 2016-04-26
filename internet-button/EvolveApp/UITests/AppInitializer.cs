using System;
using System.IO;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace UITests
{
	public class AppInitializer
	{
		public static IApp StartApp(Platform platform)
		{
			if (platform == Platform.Android)
			{
				return ConfigureApp
					.Android
					.ApkFile("../../../Droid/bin/Debug/com.xamarin.EvolveApp-Signed.apk")
					.StartApp(Xamarin.UITest.Configuration.AppDataMode.Clear);
			}

			return ConfigureApp
				.iOS
				.AppBundle("../../../iOS/bin/iPhoneSimulator/Debug/EvolveApp.iOS.app")
				.DeviceIdentifier("60C821AB-EAEB-452B-9C3F-5B86F69E65BA")
				//				.DeviceIdentifier ("XTC API Key")
				//				.InstalledApp ("com.michaelwatson.myexpenses")
				.EnableLocalScreenshots()
				.StartApp();
		}
	}
}

