using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace RadialProgress.iOS.Sample
{
	public partial class SampleViewController : UIViewController
	{
		UIButton startProgressButton;
		RadialProgressView bigRadialProgressView;
		RadialProgressView smallRadialProgressView;
		RadialProgressView tinyRadialProgressView;
		UIProgressView standardProgressView;
		NSTimer timer;

		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public SampleViewController ()
			: base ()
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			AddBackground ();

			// Add our different styles of RadialProgressViews
			bigRadialProgressView = new RadialProgressView ();
			bigRadialProgressView.Center = new PointF (View.Center.X, View.Center.Y - 100);
			bigRadialProgressView.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
			View.AddSubview (bigRadialProgressView);

			smallRadialProgressView = new RadialProgressView (RadialProgressViewStyle.Small);
			smallRadialProgressView.ProgressColor = UIColor.Gray;
			smallRadialProgressView.Center = new PointF (bigRadialProgressView.Frame.Left / 2, bigRadialProgressView.Center.Y);
			smallRadialProgressView.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
			View.AddSubview (smallRadialProgressView);

			tinyRadialProgressView = new RadialProgressView (RadialProgressViewStyle.Tiny);
			tinyRadialProgressView.ProgressColor = UIColor.White;
			tinyRadialProgressView.Center = new PointF (bigRadialProgressView.Frame.Right + (View.Frame.Width - bigRadialProgressView.Frame.Right) / 2, bigRadialProgressView.Center.Y);
			tinyRadialProgressView.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
			View.AddSubview (tinyRadialProgressView);

			standardProgressView = new UIProgressView (UIProgressViewStyle.Bar);
			standardProgressView.Frame = new RectangleF (30, bigRadialProgressView.Frame.Bottom + 80, View.Frame.Width - 60, 10);
			standardProgressView.AutoresizingMask = UIViewAutoresizing.FlexibleBottomMargin | UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleWidth;
			View.AddSubview (standardProgressView);

			startProgressButton = UIButton.FromType (UIButtonType.RoundedRect);
			startProgressButton.Frame = new RectangleF (50, standardProgressView.Frame.Bottom + 40, View.Frame.Width - 100, 30);
			startProgressButton.SetTitle ("Start Progress", UIControlState.Normal);
			startProgressButton.AutoresizingMask = UIViewAutoresizing.FlexibleBottomMargin | UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleWidth;
			startProgressButton.TouchUpInside += OnStartProgressTapped;
			View.AddSubview (startProgressButton);
		}

		void OnStartProgressTapped (object sender, EventArgs e)
		{
			standardProgressView.Progress = 0;
			bigRadialProgressView.Reset ();
			smallRadialProgressView.Reset ();
			tinyRadialProgressView.Reset ();

			if (timer != null) {
				timer.Invalidate ();
				timer = null;
			}

			// Start a timer to increment progress regularly until complete
			timer = NSTimer.CreateRepeatingScheduledTimer (1.0 / 30.0, () => {
				bigRadialProgressView.Value += 0.005f;
				smallRadialProgressView.Value += 0.005f;
				tinyRadialProgressView.Value += 0.005f;
				standardProgressView.Progress += 0.005f;

				if (bigRadialProgressView.IsDone) {
					timer.Invalidate ();
					timer = null;
				}
			});
		}

		void AddBackground ()
		{
			var bgImg = UIImage.FromFile ("Images/bg.png");
			bgImg = bgImg.StretchableImage ((int)bgImg.Size.Width / 2 - 1, (int)bgImg.Size.Height / 2 - 1);

			var bgView = new UIImageView (View.Bounds);
			bgView.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
			bgView.Image = bgImg;
			View.AddSubview (bgView);
			View.SendSubviewToBack (bgView);
		}

		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Clear any references to subviews of the main view in order to
			// allow the Garbage Collector to collect them sooner.
			//
			// e.g. myOutlet.Dispose (); myOutlet = null;
			
			ReleaseDesignerOutlets ();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			if (UserInterfaceIdiomIsPhone) {
				return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
			} else {
				return true;
			}
		}
	}
}

