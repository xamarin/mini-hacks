using Android.Graphics;
using Android.Media;

namespace AndroidApp
{
	public static class BitmapHelpers
	{
		public static Bitmap GetAndRotateBitmap (string fileName)
		{
			Bitmap bitmap = BitmapFactory.DecodeFile (fileName);


			// Images are being saved in landscape, so rotate them back to portrait if they were taken in portrait
			// See https://forums.xamarin.com/discussion/5409/photo-being-saved-in-landscape-not-portrait
			// See http://developer.android.com/reference/android/media/ExifInterface.html
			using (var mtx = new Matrix ()) {
				if (Android.OS.Build.Product.Contains ("Emulator")) {
					mtx.PreRotate (90);
				} else {
					var exif = new ExifInterface (fileName);
					var orientation = (Orientation)exif.GetAttributeInt (ExifInterface.TagOrientation, (int)Orientation.Normal);

					//TODO : handle FlipHorizontal, FlipVertical, Transpose and Transverse
					switch (orientation) {
					case Orientation.Rotate90:
						mtx.PreRotate (90);
						break;
					case Orientation.Rotate180:
						mtx.PreRotate (180);
						break;
					case Orientation.Rotate270:
						mtx.PreRotate (270);
						break;
					case Orientation.Undefined:
					case Orientation.Normal:
						// Normal, do nothing
						break;
					}

					if (mtx != null)
						bitmap = Bitmap.CreateBitmap (bitmap, 0, 0, bitmap.Width, bitmap.Height, mtx, false);
				}

				return bitmap;
			}
		}
	}
}

