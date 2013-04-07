using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using ZXing;

namespace QRScannerAndroid {
	[Activity (Label = "QRScannerAndroid", MainLauncher = true)]
	public class Activity1 : Activity {
		int count = 1;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Main);

			Button buttonScan = FindViewById<Button> (Resource.Id.buttonScan);
			TextView labelOutput = FindViewById<TextView> (Resource.Id.labelOutput);

			buttonScan.Click += delegate
			{
				var scanner = new ZXing.Mobile.MobileBarcodeScanner (this);

				scanner.UseCustomOverlay = false;
				scanner.TopText = "Hold the camera up to the barcode\nAbout 6 inches away";
				scanner.BottomText = "Wait for the barcode to automatically scan!\nEvolve 2013";

				scanner.Scan ().ContinueWith (t => {   
					if (t.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
						RunOnUiThread (() => labelOutput.Text = t.Result.Text);
				});

			};
		}
	}
}


