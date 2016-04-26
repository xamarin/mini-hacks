using System;
using Xamarin.Forms;
using XamarinSampleApp.Droid;

using Xamarin.Social;
using Xamarin.Social.Services;

[assembly: Dependency (typeof (SocialController_Droid))]
namespace XamarinSampleApp.Droid
{
	public class SocialController_Droid : ISocialController
	{
		public SocialController_Droid()
		{

		}

		public void TweetWithItem(Item item)
		{
//			var twitter = new Twitter5Service ();
//			var shareUI = twitter.GetShareUI(item, result => {
//				rootViewController.DismissViewController (true, null);
//			});

//			rootViewController.PresentViewController(shareUI, true, null);
		}
	}
}