using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using Google.Maps;

namespace GMapsiOS {

	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate {
		// class-level declarations

		const string MapsApiKey = "AIzaSyDniXYRop4xXSlrS8rdshEkSnQw2W0gGhI";

		public override UIWindow Window {
			get;
			set;
		}

		public override void FinishedLaunching (UIApplication application)
		{
			MapServices.ProvideAPIKey (MapsApiKey);
		}

	}
}

