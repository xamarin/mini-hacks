using System;
using AVFoundation;
using Xamarin.Forms;
using XamarinSampleApp.iOS;

using Xamarin.Social;
using Xamarin.Social.Services;

using UIKit;

[assembly: Dependency (typeof (SocialController_iOS))]
namespace XamarinSampleApp.iOS
{
	public class SocialController_iOS : ISocialController
	{
		public SocialController_iOS()
		{
			
		}

		public void TweetWithItem(Item item)
		{
			var rootViewController = UIApplication.SharedApplication.KeyWindow.RootViewController;

			var twitter = new Twitter5Service ();
			var shareUI = twitter.GetShareUI(item, result => {
				rootViewController.DismissViewController (true, null);
			});

			rootViewController.PresentViewController(shareUI, true, null);
		}
	}
}