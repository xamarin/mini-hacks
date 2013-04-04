using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Nfc;

namespace NFCReader {
	[Activity (Label = "NFCReader", MainLauncher = true)]
	public class Activity1 : Activity {
		int count = 1;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);

			// Get a reference to the default NFC adapter for this device. This adapter 
			// is how an Android application will interact with the actual hardware.
			var nfcAdapter = NfcAdapter.GetDefaultAdapter(this);



			button.Click += delegate
			{
				button.Text = string.Format ("{0} clicks!", count++);
			};
		}


	}
}


