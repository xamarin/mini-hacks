// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace XamarinMobileiOS
{
	[Register ("XamarinMobileiOSViewController")]
	partial class XamarinMobileiOSViewController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel LabelLocation { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton buttonLocation { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton buttonPicture { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView imagePhoto { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (LabelLocation != null) {
				LabelLocation.Dispose ();
				LabelLocation = null;
			}

			if (buttonLocation != null) {
				buttonLocation.Dispose ();
				buttonLocation = null;
			}

			if (buttonPicture != null) {
				buttonPicture.Dispose ();
				buttonPicture = null;
			}

			if (imagePhoto != null) {
				imagePhoto.Dispose ();
				imagePhoto = null;
			}
		}
	}
}
