using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using RadialProgress;

namespace RadialProgressiOS
{
	public partial class RadialProgressiOSViewController : UIViewController
	{
		public RadialProgressiOSViewController (IntPtr handle) : base (handle)
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

			var progressView = new RadialProgressView {
				Center = new PointF (View.Center.X, View.Center.Y - 100)
			};

			View.AddSubview(progressView);

			buttonAdd.TouchUpInside += (sender, e) => {
				progressView.Value += 0.05f;
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

