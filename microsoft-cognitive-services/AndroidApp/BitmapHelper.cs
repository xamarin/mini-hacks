using Android.Graphics;
using Android.Media;

namespace AndroidApp
{
    public static class BitmapHelpers
    {
        public static Bitmap GetAndRotateBitmap(string fileName)
        {
            Bitmap bitmap = BitmapFactory.DecodeFile(fileName);

            // Images are being saved in landscape, so rotate them back to portrait if they were taken in portrait
            // See https://forums.xamarin.com/discussion/5409/photo-being-saved-in-landscape-not-portrait
            // See http://developer.android.com/reference/android/media/ExifInterface.html
            using (Matrix mtx = new Matrix())
            {
                ExifInterface exif = new ExifInterface(fileName);
                var orientation = (Orientation)exif.GetAttributeInt(ExifInterface.TagOrientation, (int)Orientation.Undefined);

                //TODO : handle FlipHorizontal, FlipVertical, Transpose and Transverse
                //Undefined might be an emulator issue. Taking the assumption that the picture has been taken in portrait mode
                switch (orientation)
                {
                    case Orientation.Undefined:
                    case Orientation.Rotate90:
                        mtx.PreRotate(90);
                        break;
                    case Orientation.Rotate180:
                        mtx.PreRotate(180);
                        break;
                    case Orientation.Rotate270:
                        mtx.PreRotate(270);
                        break;
                    case Orientation.Normal:
                        // Normal, do nothing
                        break;
                    default:
                        break;
                }

                if (mtx != null)
                    bitmap = Bitmap.CreateBitmap(bitmap, 0, 0, bitmap.Width, bitmap.Height, mtx, false);
            }

            return bitmap;
        }
    }
}