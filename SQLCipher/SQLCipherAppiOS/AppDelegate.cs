using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace SQLCipherApp
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		UIWindow window;
		MainViewController viewController;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			
			viewController = new MainViewController ();
			window.RootViewController = viewController;
			window.MakeKeyAndVisible ();

			/*
			if(options != null)
			{
				var url = (NSUrl) options["UIApplicationLaunchOptionsURL"];
				if (url != null && url.IsFileUrl) 
				{
					viewController.HandleOpenUrl(url);
					Console.WriteLine(url);
				}
			}
			*/
			return true;
		}

		public override bool OpenUrl (UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
		{
			if(url.IsFileUrl) 
			{
				Console.WriteLine(url);
				viewController.HandleOpenUrl(url);
				return true;
			}
			return false;
		}

	}
}

