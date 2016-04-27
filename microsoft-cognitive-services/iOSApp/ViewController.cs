using System;
using System.IO;
using Foundation;
using UIKit;

namespace iOSApp
{
	public partial class ViewController : UIViewController
	{
		protected ViewController (IntPtr handle) : base (handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}
		
		public override void ViewDidAppear (bool animated)
		{
    			TakePhotoButton.TouchDown += OnTakePhotoPressed;
		}

		void OnTakePhotoPressed (object sender, EventArgs eventArgs)
		{
			TakePhotoButton.Enabled = false;

			UIImagePickerController picker = new UIImagePickerController ();
			picker.SourceType = UIImagePickerControllerSourceType.Camera;

			picker.FinishedPickingMedia += async (o, e) => {
				// Create a moderate quality version of the image
				byte [] dataBytes;
				using (NSData data = e.OriginalImage.AsJPEG (.5f)) {
					dataBytes = new byte [data.Length];
					System.Runtime.InteropServices.Marshal.Copy (data.Bytes, dataBytes, 0, Convert.ToInt32 (data.Length));
				}

				ThePhoto.Image = e.OriginalImage;
				DetailsText.Text = "Processing...";

				((UIImagePickerController)o).DismissViewController (true, null);

				// Create a stream, send it to MCS, and get back 
				using (MemoryStream stream = new MemoryStream (dataBytes)) {
					try {
						float happyValue = await SharedProject.Core.GetAverageHappinessScore (stream);
						DetailsText.Text = SharedProject.Core.GetHappinessMessage (happyValue);
					} catch (Exception ex) {
						DetailsText.Text = ex.Message;
					}
					TakePhotoButton.Enabled = true;

				}
			};
			PresentModalViewController (picker, true);
		}
	}
}

