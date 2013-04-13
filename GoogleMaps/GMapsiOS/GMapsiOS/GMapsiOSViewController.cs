using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using Google.Maps;

namespace GMapsiOS {
	public partial class GMapsiOSViewController : UIViewController {

		MapView mapView;
		UISwitch satelliteSwitch;
		UIButton currentLocNow;

		public GMapsiOSViewController (IntPtr handle) : base (handle)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();			
		}

		#region View lifecycle

		public override void LoadView ()
		{
			base.LoadView ();
			
			var camera = CameraPosition.FromCamera (latitude: 30.2652898, 
			                                        longitude: -97.7386826, 
			                                        zoom: 17,
			                                        bearing: 15,
			                                        viewingAngle: 15);
			mapView = MapView.FromCamera (View.Bounds, camera);
			mapView.MapType = MapViewType.Normal;
			mapView.MyLocationEnabled = true;
			//mapView.Frame = View.Frame;

			var hiltonOptions = new MarkerOptions();

			hiltonOptions.Position = new MonoTouch.CoreLocation.CLLocationCoordinate2D(30.2652898, -97.7386826);
			hiltonOptions.Title = "Evolve 2013";
			hiltonOptions.Snippet = "Austin, TX";
			mapView.AddMarker(hiltonOptions);

			View.Add (mapView);

			satelliteSwitch = new UISwitch ();
			satelliteSwitch.Frame = new RectangleF(11.0f, 11.0f, satelliteSwitch.Frame.Width, satelliteSwitch.Frame.Height);
			satelliteSwitch.On = false;
			View.Add (satelliteSwitch);

			currentLocNow = UIButton.FromType (UIButtonType.RoundedRect);
			currentLocNow.Frame = new RectangleF(50, 100, 100, 20);
			currentLocNow.Center = new PointF (View.Frame.Width/2, View.Frame.Height - 20.0f);
			currentLocNow.SetTitle ("ShowMyLoc", UIControlState.Normal);
			View.AddSubview (currentLocNow);
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();


//			currentLocNow.Frame = new RectangleF(100f, 100f, 100f, 100f);
			
			currentLocNow.TouchUpInside += (sender, e) => 
			{
				mapView.AnimateToLocation(mapView.MyLocation.Coordinate);
			};

			satelliteSwitch.ValueChanged += (sender, e) => 
			{
				if (satelliteSwitch.On) {
					mapView.MapType = MapViewType.Hybrid;
				} else {
					mapView.MapType = MapViewType.Normal;
				}
			};
			// Perform any additional setup after loading the view, typically from a nib.
		}
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			mapView.StartRendering ();
		}
		
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}
		
		public override void ViewWillDisappear (bool animated)
		{
			mapView.StopRendering ();
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

