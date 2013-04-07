using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ZXing.YuvTool
{
	[Activity(Label = "Captured Image")]
	public class AnalyzeImageActivity : Activity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Create your application here
			SetContentView(Resource.Layout.AnalyzeImageLayout);

			var button = this.FindViewById<Button>(Resource.Id.buttonShare);

			button.Click += (sender, args) =>
				{

					var file = this.GetFileStreamPath("YuvData.txt");

					Console.WriteLine("FILE: " + file.ToURI());


					Intent intent = new Intent(Intent.ActionSend);
					intent.SetType("text/plain");
					intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse(file.ToURI().ToString()));
					intent.PutExtra(Intent.ExtraSubject, "YUV Data");

					StartActivity(Intent.CreateChooser(intent, "Share YUV data using"));
				};

			var iv = this.FindViewById<ImageView>(Resource.Id.imageViewPreview);



			var bitmap = BitmapFactory.DecodeFile(this.GetFileStreamPath("YuvImage.jpg").ToString());

			iv.SetImageBitmap(bitmap);


			var width = this.Intent.GetIntExtra("IMGWIDTH", 0);
			var height = this.Intent.GetIntExtra("IMGHEIGHT", 0);

			Console.WriteLine("IMAGE WIDTH: " + width + "  HEIGHT: " + height);

		}
	}
}