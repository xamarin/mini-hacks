using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using RadialProgress;

namespace RadialProgressAndroid {
	[Activity (Label = "RadialProgressAndroid", MainLauncher = true)]
	public class Activity1 : Activity {
		int count = 1;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Main);

			Button button = FindViewById<Button> (Resource.Id.myButton);
			var progressView = FindViewById<RadialProgressView> (Resource.Id.progressView);

			button.Click += delegate
			{
				if (progressView.IsDone)
					progressView.Reset();
				else
					progressView.Value += 10;
				//button.Text = string.Format ("{0} clicks!", count++);
			};
		}
	}
}


