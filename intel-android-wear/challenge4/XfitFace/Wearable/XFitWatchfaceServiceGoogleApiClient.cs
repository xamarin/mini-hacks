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

using Android.Gms.Common.Apis;
using Android.Gms.Fitness;
using Android.Gms.Wearable;
using Android.Support.Wearable.Watchface;
using Android.Util;

namespace XFitWatchface
{
	public partial class XFitWatchfaceService : CanvasWatchFaceService
	{

		partial class XFitWatchfaceEngine : Engine
		{
			GoogleApiClient googleApiClient;

			void ConnectGoogleApiClient()
			{
				googleApiClient = new GoogleApiClient.Builder(owner)
					.AddApi(FitnessClass.HISTORY_API)
					.AddApi(FitnessClass.RECORDING_API)
					.AddApi(WearableClass.API)
					.UseDefaultAccount()
					.AddConnectionCallbacks(
						connectionHint =>
						{
							if (Log.IsLoggable(Tag, LogPriority.Info))
							{
								Log.Info(Tag, "Connected to the Google API client");
							}

							ConnectFitnessApi();
							ConnectWatchfaceConfigUpdates();
						},
						cause =>
						{
							if (Log.IsLoggable(Tag, LogPriority.Info))
							{
								Log.Info(Tag, "Connection suspended");
							}
						}
					)
					.Build();

				googleApiClient.Connect();
			}
		}
	}
}

