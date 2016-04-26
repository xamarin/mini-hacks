using System;
using AVFoundation;
using Xamarin.Forms;
using XamarinSampleApp.iOS;

using UIKit;

[assembly: Dependency (typeof (Appearance_iOS))]
namespace XamarinSampleApp.iOS
{
	public class Appearance_iOS : IAppearance
	{
		public Appearance_iOS ()
		{
		}

		public void UpdateBackground(double r, double g, double b) {
			var rootViewController = UIApplication.SharedApplication.KeyWindow.RootViewController;
			rootViewController.View.BackgroundColor = new UIColor ((float)r, (float)g, (float)b, 1);
		}
	}
}
