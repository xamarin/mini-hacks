using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Views.Animations;
using Android.Graphics;

namespace CardboardMonkey
{
	public class CardboardOverlayView : LinearLayout
	{
		CardboardOverlayEyeView mLeftView;
		CardboardOverlayEyeView mRightView;

		public CardboardOverlayView (Context context) :
			base (context)
		{
			Initialize ();
		}

		public CardboardOverlayView (Context context, IAttributeSet attrs) :
			base (context, attrs)
		{
			Initialize ();
		}

		public CardboardOverlayView (Context context, IAttributeSet attrs, int defStyle) :
			base (context, attrs, defStyle)
		{
			Initialize ();
		}

		void Initialize ()
		{
			Orientation = Orientation.Horizontal;

			var ps = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent,
			                                        LinearLayout.LayoutParams.MatchParent,
			                                        1.0f);
			ps.SetMargins (0, 0, 0, 0);

			mLeftView = new CardboardOverlayEyeView (Context);
			mLeftView.LayoutParameters = ps;
			AddView(mLeftView);

			mRightView = new CardboardOverlayEyeView (Context);
			mRightView.LayoutParameters = ps;
			AddView(mRightView);

			// Set some reasonable defaults.
			SetDepthOffset (0.016f);

			SetColor (Color.Rgb (150, 255, 180));
			Visibility = ViewStates.Visible;
		}

		/// <summary>
		/// Display a toast-like message suitable for Cardboard display.
		/// </summary>
		/// <param name="message">the text to display</param>
		public void Show3DToast (string message)
		{
			SetText (message);
			SetTextAlpha (1f);
			FadeOut ();
		}

		/// <summary>
		/// Set the color displayed by the temperature indicator at the top of the screen.
		/// </summary>
		/// <param name="temperature">The color to display</param>
		public void SetTemperatureColor (Color temperature)
		{
			mLeftView.SetTemperature (temperature);
			mRightView.SetTemperature (temperature);
		}

		void SetDepthOffset (float offset)
		{
			mLeftView.SetOffset (offset);
			mRightView.SetOffset (-offset);
		}

		void SetText (string text)
		{
			mLeftView.SetText (text);
			mRightView.SetText (text);
		}

		void SetTextAlpha (float alpha)
		{
			mLeftView.SetTextViewAlpha(alpha);
			mRightView.SetTextViewAlpha(alpha);
		}

		void SetColor (Color color)
		{
			mLeftView.SetColor (color);
			mRightView.SetColor (color);
		}

		void FadeOut ()
		{
			mLeftView.FadeOut ();
			mRightView.FadeOut ();
		}

		class CardboardOverlayEyeView: ViewGroup
		{
			const int FadeDuration = 5000;

			readonly TextView textView;
			float offset;
			Paint temperaturePaint;

			public CardboardOverlayEyeView (Context context) : base (context)
			{
				textView = new TextView(context);
				textView.SetTextSize (ComplexUnitType.Dip, 14f);
				textView.SetTypeface (textView.Typeface, TypefaceStyle.Bold);
				textView.Gravity = GravityFlags.Center;
				textView.SetShadowLayer(3.0f, 0.0f, 0.0f, Color.DarkGray);
				AddView(textView);

				temperaturePaint = new Paint {
					Color = Color.Transparent,
					AntiAlias = true,
				};

				SetWillNotDraw (false);
			}

			public void SetColor (Color color)
			{
				textView.SetTextColor (color);
			}

			public void SetText (string text)
			{
				textView.Text = text;
			}

			public void SetTextViewAlpha (float alpha)
			{
				textView.Alpha = alpha;
			}

			public void SetOffset (float offset)
			{
				this.offset = offset;
			}

			public void SetTemperature (Color temperature)
			{
				temperaturePaint.Color = temperature;
				PostInvalidate ();
			}

			public void FadeOut ()
			{
				textView.Animate ().Alpha (0).SetDuration (FadeDuration).Start ();
			}

			protected override void OnLayout (bool changed, int left, int top, int right, int bottom)
			{
				// Width and height of this ViewGroup.
				int width = right - left;
				int height = bottom - top;

				// Vertical position of the text, specified in fractions of this ViewGroup's height.
				float verticalTextPos = 0.52f;

				// Layout TextView
				var leftMargin = offset * width;
				var topMargin = height * verticalTextPos;
				textView.Layout ((int) leftMargin, (int) topMargin,
				                 (int) (leftMargin + width), (int) (topMargin + height * (1.0f - verticalTextPos)));
			}

			protected override void OnDraw (Canvas canvas)
			{
				base.OnDraw (canvas);

				var radius = TypedValue.ApplyDimension (ComplexUnitType.Dip, 12, Resources.DisplayMetrics);
				var left = (int)(0.5 * Width);
				var top = (int)(0.2 * Height);

				canvas.DrawCircle (left, top, radius, temperaturePaint);
			}
		}
	}
}

