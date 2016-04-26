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

using System.Linq;
using Android.Gms.Common.Apis;
using Android.Gms.Fitness;
using Android.Gms.Fitness.Data;
using Android.Gms.Fitness.Result;
using Android.Graphics;
using Android.Support.Wearable.Watchface;
using Android.Util;

namespace XFitWatchface
{
	public partial class XFitWatchfaceService : CanvasWatchFaceService
	{

		partial class XFitWatchfaceEngine : Engine
		{
			bool stepsRequested;
			int stepCount;
			bool displayStepCount = true;

			Paint stepcountPaint;

			void InitStepCountPaint()
			{
				stepcountPaint = new Paint();
				//stepcountPaint.Color = Color.White;
				stepcountPaint.SetARGB(255, 50, 151, 218);
				stepcountPaint.SetTypeface(Typeface.Create(Typeface.SansSerif, TypefaceStyle.Normal));
				stepcountPaint.AntiAlias = true;
				stepcountPaint.TextSize = owner.Resources.GetDimension(Resource.Dimension.StepCountTextSize);
			}

			void ConnectFitnessApi()
			{
				// Step Count
				stepsRequested = false;
				SubscribeToSteps();
				GetTotalSteps();
			}

			async void SubscribeToSteps()
			{
				var status = await FitnessClass.RecordingApi.SubscribeAsync(googleApiClient, Android.Gms.Fitness.Data.DataType.TypeStepCountDelta);
				if (status.Status.IsSuccess)
				{
					if (status.Status.StatusCode == FitnessStatusCodes.SuccessAlreadySubscribed)
					{
						if (Log.IsLoggable(Tag, LogPriority.Debug))
						{
							Log.Debug(Tag, "Already suscribed");
						}

					}
					else {
						if (Log.IsLoggable(Tag, LogPriority.Debug))
						{
							Log.Debug(Tag, "Successfully suscribed");
						}

					}
				}
				else {
					if (Log.IsLoggable(Tag, LogPriority.Debug))
					{
						Log.Debug(Tag, "Problem suscribing");
					}
				}
			}

			void UpdateStepCount()
			{
				GetTotalSteps();
			}

			async void GetTotalSteps()
			{
				if ((googleApiClient != null) && (googleApiClient.IsConnected) && !stepsRequested)
				{
					stepsRequested = true;

					// Handy tip: ReadDailyTotal doesn't require user permission
					var result = await FitnessClass.HistoryApi.ReadDailyTotalAsync(googleApiClient, Android.Gms.Fitness.Data.DataType.TypeStepCountDelta);
					var ds = result.Total;
					stepCount = ExtractStepValue(ds);

					stepsRequested = false;
				}
			}

			int ExtractStepValue(Bucket bucket)
			{
				// Using LINQ to simplify looping through the data set
				return bucket.DataSets.Sum(ExtractStepValue);
			}

			int ExtractStepValue(DataSet dataSet)
			{
				// Using LINQ to loop through two levels of lists (data points & fields)
				return dataSet.DataPoints.Sum(dp => dp.DataType.Fields.Sum(f => dp.GetValue(f).AsInt()));
			}

			void DrawStepCount(Canvas canvas, Rect bounds)
			{
				if (displayStepCount)
				{
					UpdateStepCount();

					var width = bounds.Width();
					var height = bounds.Height();

					var steps = stepCount.ToString();
					var stepsWidth = stepcountPaint.MeasureText(steps);

					var x = width - stepsWidth - owner.Resources.GetDimension(Resource.Dimension.StepCountOffset);
					var y = (height + owner.Resources.GetDimension(Resource.Dimension.StepCountTextSize)) / 2.0f;

					canvas.DrawText(steps, x, y, stepcountPaint);
				}
			}

		}
	}
}

