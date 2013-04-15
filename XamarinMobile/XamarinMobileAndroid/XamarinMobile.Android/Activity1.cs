using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Xamarin.Geolocation;
using Xamarin.Media;

namespace Xam.MobileHack
{
    [Activity(Label = "Xam.MobileHack", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity
    {
        TextView LabelLocation;
        ImageView image;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.buttonLocation);
            LabelLocation = FindViewById<TextView>(Resource.Id.locationLabel);
            image = FindViewById<ImageView>(Resource.Id.imagePhoto);

            button.Click += delegate {

                var locator = new Geolocator(this) { DesiredAccuracy = 50 };
                locator.GetPositionAsync(timeout: 10000).ContinueWith(t =>
                {
                    var text = String.Format("Location : Lat: {0}, Long: {0}", t.Result.Latitude, t.Result.Longitude);
                    this.RunOnUiThread(() => LabelLocation.Text = text);
                });
            };

            Button getimge = FindViewById<Button>(Resource.Id.buttonCamera);
            getimge.Click += delegate
            {
                var camera = new MediaPicker(this);

                if (!camera.IsCameraAvailable)
                {
                    Console.WriteLine("Camera unavailable!");
                    return;
                }

                var opts = new StoreCameraMediaOptions
                {
                    Name = "test.jpg",
                    Directory = "MiniHackDemo"
                };

                camera.TakePhotoAsync(opts).ContinueWith(t =>
                {
                    if (t.IsCanceled)
                        return;

                    using (var bmp = Android.Graphics.BitmapFactory.DecodeFile(t.Result.Path))
                    {
                        this.RunOnUiThread(() => image.SetImageBitmap(bmp));
                    }
                });
                
            };
        }
    }
}

