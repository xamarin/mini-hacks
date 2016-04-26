using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

using TextStyles.iOS;
using Xamarin.Forms;
using System.Threading.Tasks;
using Plugin.Toasts;

namespace EvolveApp.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		EvolveApp.App formsApp;
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();
			ZXing.Net.Mobile.Forms.iOS.Platform.Init();

			EnumerateFonts();

			// Initalise TextStyle
			TextStyle.Instance.SetCSS(App.CSS);

#if DEBUG
			Xamarin.Calabash.Start();
#endif
			Forms.ViewInitialized += (object sender, ViewInitializedEventArgs e) =>
			{
				// http://developer.xamarin.com/recipes/testcloud/set-accessibilityidentifier-ios/
				if (null != e.View.StyleId)
					e.NativeView.AccessibilityIdentifier = e.View.StyleId;
			};

			DependencyService.Register<ToastNotificatorImplementation>(); // Register your dependency
			ToastNotificatorImplementation.Init();

			formsApp = new App();
			LoadApplication(formsApp);

			return base.FinishedLaunching(app, options);
		}

		void EnumerateFonts()
		{
			var familyNames = UIFont.FamilyNames;
			foreach (var familyName in familyNames)
			{
				Console.WriteLine("Family: {0}\n", familyName);

				var fontNames = UIFont.FontNamesForFamilyName(familyName);
				foreach (var fontName in fontNames)
				{
					Console.WriteLine("\tFont: {0}\n", fontName);
				}
			};
		}

		#region Xamarin Test Cloud Back Door Methods

#if DEBUG
		[Export("skipScanner:")] // notice the colon at the end of the method name
		public NSString SkipScanner(NSString reportName)
		{

			//Task.Run(async () => { await App.XTCBackDoor.BackDoor()}).Wait();

			return new NSString();
		}
#endif
		#endregion
	}
}