using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using System.Threading.Tasks;
using Xamarin.Geolocation;
using Xamarin.Media;

namespace XamarinMobileiOS
{
	public partial class XamarinMobileiOSViewController : UIViewController
	{
		public XamarinMobileiOSViewController (IntPtr handle) : base (handle)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		#region View lifecycle
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			buttonLocation.TouchUpInside += (sender, e) => {
				var locator = new Geolocator { DesiredAccuracy = 50 };
				locator.GetPositionAsync (timeout: 10000).ContinueWith (t => {
					var text = String.Format("Lat: {0}, Long: {0}", t.Result.Latitude, t.Result.Longitude);
					InvokeOnMainThread(() => LabelLocation.Text = text);
				});
			};

			buttonPicture.TouchUpInside += (sender, e) => {
				var camera = new MediaPicker ();
				
				if (!camera.IsCameraAvailable) {
					Console.WriteLine("Camera unavailable!");
					return;
				}

				var opts = new StoreCameraMediaOptions {
					Name = "test.jpg",
					Directory = "MiniHackDemo"
				};

				camera.TakePhotoAsync (opts).ContinueWith (t => {
					if (t.IsCanceled)
						return;

					InvokeOnMainThread(() => imagePhoto.Image = UIImage.FromFile(t.Result.Path));
				});
			};
		}
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}
		
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}
		
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}
		
		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}
		
		#endregion
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
	}
}

