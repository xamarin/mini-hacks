// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace QRScanneriOS
{
	[Register ("QRScanneriOSViewController")]
	partial class QRScanneriOSViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton buttonScan { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel labelOutput { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (buttonScan != null) {
				buttonScan.Dispose ();
				buttonScan = null;
			}

			if (labelOutput != null) {
				labelOutput.Dispose ();
				labelOutput = null;
			}
		}
	}
}
