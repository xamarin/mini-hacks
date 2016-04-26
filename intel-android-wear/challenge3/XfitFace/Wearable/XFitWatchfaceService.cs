//
//  Copyright 2016 Google Inc. All Rights Reserved.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Service.Wallpaper;
using Android.Support.V4.Content;
using Android.Support.Wearable.Watchface;
using Android.Views;
using Java.Util;
using Java.Util.Concurrent;

namespace XFitWatchface
{
	[Service(Label = "XFit Watchface 3", Permission = "android.permission.BIND_WALLPAPER")]
	[MetaData("android.service.wallpaper", Resource = "@xml/watch_face")]
	[MetaData("com.google.android.wearable.watchface.preview", Resource = "@drawable/preview_xfit")]
	[IntentFilter(new[] { "android.service.wallpaper.WallpaperService" }, Categories = new[] { "com.google.android.wearable.watchface.category.WATCH_FACE" })]
	public partial class XFitWatchfaceService: CanvasWatchFaceService
	{

		const string Tag = "XFitWatchfaceService";

		public override WallpaperService.Engine OnCreateEngine()
		{
			return new XFitWatchfaceEngine(this);
		}

		partial class XFitWatchfaceEngine : Engine
		{
			CanvasWatchFaceService owner;
			Paint hourPaint;
			Paint minutePaint;
			Paint secondPaint;
			Paint tickPaint;

			Bitmap backgroundBitmap;
			Bitmap backgroundScaledBitmap;

			Calendar calendar;
			System.Threading.Timer timerSeconds;

			static long InteractiveUpdateRateMs = TimeUnit.Seconds.ToMillis(1);

			public XFitWatchfaceEngine(CanvasWatchFaceService owner) : base(owner)
			{
				this.owner = owner;
			}			

			public override void OnCreate(ISurfaceHolder surfaceHolder)
			{
				SetWatchFaceStyle(new WatchFaceStyle.Builder(owner)
								  .SetCardPeekMode(WatchFaceStyle.PeekModeShort)
								  .SetBackgroundVisibility(WatchFaceStyle.BackgroundVisibilityInterruptive)
								  .SetShowSystemUiTime(false)
								  .Build()
								 );
				base.OnCreate(surfaceHolder);

				InitBackground();
				InitPaints();

				calendar = new GregorianCalendar();

				ConnectGoogleApiClient();
			}

			void InitBackground()
			{
				var backgroundDrawable = ContextCompat.GetDrawable(owner, Resource.Drawable.XamarinWatchFaceBackground);
				backgroundBitmap = (backgroundDrawable as BitmapDrawable).Bitmap;
			}

			void InitPaints()
			{
				hourPaint = new Paint();
				hourPaint.SetARGB(255, 200, 200, 200);
				hourPaint.StrokeWidth = 5.0f;
				hourPaint.AntiAlias = true;
				hourPaint.StrokeCap = Paint.Cap.Round;

				minutePaint = new Paint();
				minutePaint.SetARGB(255, 200, 200, 200);
				minutePaint.StrokeWidth = 3.0f;
				minutePaint.AntiAlias = true;
				minutePaint.StrokeCap = Paint.Cap.Round;

				secondPaint = new Paint();
				secondPaint.SetARGB(255, 50, 151, 218);
				//secondPaint.SetARGB(255, 255, 0, 0);
				secondPaint.StrokeWidth = 2.0f;
				secondPaint.AntiAlias = true;
				secondPaint.StrokeCap = Paint.Cap.Round;

				tickPaint = new Paint();
				tickPaint.SetARGB(100, 200, 200, 200);
				tickPaint.StrokeWidth = 2.0f;
				tickPaint.AntiAlias = true;

				InitStepCountPaint();
			}

			public override void OnTimeTick()
			{
				base.OnTimeTick();
				Invalidate();
			}

			public override void OnAmbientModeChanged(bool inAmbientMode)
			{
				base.OnAmbientModeChanged(inAmbientMode);
				UpdateTimer();
			}

			public override void OnDraw(Canvas canvas, Rect bounds)
			{
				DrawFace(canvas, bounds);
				DrawStepCount(canvas, bounds);
				DrawHands(canvas, bounds);
			}

			void DrawFace(Canvas canvas, Rect bounds)
			{
				var width = bounds.Width();
				var height = bounds.Height();

				// Draw the background, scaled to fit.
				if (backgroundScaledBitmap == null
					|| backgroundScaledBitmap.Width != width
					|| backgroundScaledBitmap.Height != height)
				{
					backgroundScaledBitmap = Bitmap.CreateScaledBitmap(backgroundBitmap,
						width, height, true /* filter */);
				}
				canvas.DrawColor(Color.Black);
				canvas.DrawBitmap(backgroundScaledBitmap, 0, 0, null);

				var centerX = width / 2.0f;
				var centerY = height / 2.0f;

				// Draw the ticks.
				var innerTickRadius = centerX - 10;
				var outerTickRadius = centerX;
				for (var tickIndex = 0; tickIndex < 12; tickIndex++)
				{
					var tickRot = (float)(tickIndex * Math.PI * 2 / 12);
					var innerX = (float)Math.Sin(tickRot) * innerTickRadius;
					var innerY = (float)-Math.Cos(tickRot) * innerTickRadius;
					var outerX = (float)Math.Sin(tickRot) * outerTickRadius;
					var outerY = (float)-Math.Cos(tickRot) * outerTickRadius;
					canvas.DrawLine(centerX + innerX, centerY + innerY,
						centerX + outerX, centerY + outerY, tickPaint);
				}
			}

			void DrawHands(Canvas canvas, Rect bounds)
			{
				var width = bounds.Width();
				var height = bounds.Height();
				var centerX = width / 2.0f;
				var centerY = height / 2.0f;

				calendar.TimeInMillis = Java.Lang.JavaSystem.CurrentTimeMillis();
				var hour = calendar.Get(CalendarField.Hour);
				var minute = calendar.Get(CalendarField.Minute);
				var second = calendar.Get(CalendarField.Second);

				var secRot = second / 30f * (float)Math.PI;
				var minutes = minute;
				var minRot = minutes / 30f * (float)Math.PI;
				var hrRot = ((hour + (minutes / 60f)) / 6f) * (float)Math.PI;

				var secLength = centerX - 20;
				var minLength = centerX - 40;
				var hrLength = centerX - 80;

				if (!IsInAmbientMode)
				{
					var secX = (float)Math.Sin(secRot) * secLength;
					var secY = (float)-Math.Cos(secRot) * secLength;
					canvas.DrawLine(centerX, centerY, centerX + secX, centerY + secY, secondPaint);
				}

				var minX = (float)Math.Sin(minRot) * minLength;
				var minY = (float)-Math.Cos(minRot) * minLength;
				canvas.DrawLine(centerX, centerY, centerX + minX, centerY + minY, minutePaint);

				var hrX = (float)Math.Sin(hrRot) * hrLength;
				var hrY = (float)-Math.Cos(hrRot) * hrLength;
				canvas.DrawLine(centerX, centerY, centerX + hrX, centerY + hrY, hourPaint);
			}

			public override void OnVisibilityChanged(bool visible)
			{
				base.OnVisibilityChanged(visible);
				UpdateTimer();
			}

			void UpdateTimer()
			{
				if (timerSeconds == null)
				{
					timerSeconds = new System.Threading.Timer(
						state =>
						{
							Invalidate();
						},
						null,
						TimeSpan.FromMilliseconds(InteractiveUpdateRateMs),
						TimeSpan.FromMilliseconds(InteractiveUpdateRateMs));
				}
				else {
					if (ShouldTimerBeRunning())
					{
						timerSeconds.Change(0, InteractiveUpdateRateMs);
					}
					else {
						timerSeconds.Change(System.Threading.Timeout.Infinite, 0);
					}
				}
			}

			bool ShouldTimerBeRunning()
			{
				return IsVisible && !IsInAmbientMode;
			}

		}
	}
}

