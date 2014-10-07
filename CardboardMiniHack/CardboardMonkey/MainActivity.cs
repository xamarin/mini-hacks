using System;
using System.IO;
using System.Linq;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;

using Google.VRToolkit.Carboard;
using Google.VRToolkit.Carboard.Sensors;

namespace CardboardMonkey
{
	[Activity (Label = "CardboardMonkey", MainLauncher = true, Icon = "@drawable/g_cardboard_icon",
	           ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
	public class MainActivity : CardboardActivity, CardboardView.IStereoRenderer
	{
		const string Tag = "MainActivity";

		Game game;
		Vibrator vibrator;

		float[] headView;
		CardboardOverlayView overlayView;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView (Resource.Layout.common_ui);
			CardboardView cardboardView = FindViewById<CardboardView> (Resource.Id.cardboard_view);
			cardboardView.SetRenderer (this);
			CardboardView = cardboardView;

			vibrator = (Vibrator)GetSystemService (Context.VibratorService);

			overlayView = FindViewById<CardboardOverlayView> (Resource.Id.overlay);
			overlayView.Show3DToast ("Pull the magnet\nwhen you find an object.");

			headView = new float[16];
			game = new Game (this, headView);

			VolumeKeysMode = VolumeKeys.DisabledWhileInCardboard;
		}

		public void OnRendererShutdown ()
		{
			Log.Info (Tag, "onRendererShutdown");
		}

		public void OnSurfaceChanged (int width, int height)
		{
			Log.Info (Tag, "onSurfaceChanged");
		}

		public void OnSurfaceCreated (Javax.Microedition.Khronos.Egl.EGLConfig config)
		{
			Log.Info (Tag, "onSurfaceCreated");
			game.Initialize ();
		}

		public void OnNewFrame (HeadTransform headTransform)
		{
			headTransform.GetHeadView (headView, 0);
			game.PrepareFrame ();
		}

		public void OnDrawEye (EyeTransform transform)
		{
			game.Draw (transform.GetEyeView (), transform.GetPerspective ());
		}

		public void OnFinishFrame (Viewport viewport)
		{
			game.FinishFrame ();
		}

		public override void OnCardboardTrigger ()
		{
			Log.Info (Tag, "onCardboardTrigger");

			// Always give user feedback
			vibrator.Vibrate(50);
		}
	}
}


