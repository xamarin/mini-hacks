using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using Google.Maps;

namespace GMapsiOS {
	public partial class GMapsiOSViewController : UIViewController {

		MapView mapView;
		UISwitch satelliteSwitch;

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
			                                        zoom:17,
			                                        bearing:15,
			                                        viewingAngle:15);
			mapView = MapView.FromCamera (RectangleF.Empty, camera);
			mapView.MapType = MapViewType.Normal;
			mapView.MyLocationEnabled = true;
			View = mapView;

			satelliteSwitch = new UISwitch();
			satelliteSwitch.On = true;
			View.Add (satelliteSwitch);

		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			satelliteSwitch.ValueChanged += (sender, e) => 
			{
				if (satelliteSwitch.On)
				{
					mapView.MapType = MapViewType.Hybrid;
				}
				else
				{
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

