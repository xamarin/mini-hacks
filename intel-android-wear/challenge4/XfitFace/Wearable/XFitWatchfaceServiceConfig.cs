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

using Android.App;
using Android.Gms.Common.Apis;
using Android.Gms.Fitness;
using Android.Gms.Wearable;
using Android.Support.Wearable.Watchface;
using Android.Util;

namespace XFitWatchface
{
	[MetaData("com.google.android.wearable.watchface.wearableConfigurationAction", Value = "XFitWatchface.CONFIG")]
	// TODO: add metadata to register mobile companion config activity
	public partial class XFitWatchfaceService : CanvasWatchFaceService
	{

		partial class XFitWatchfaceEngine : Engine, IDataApiDataListener
		{
			void ConnectWatchfaceConfigUpdates()
			{
				WearableClass.DataApi.AddListener(googleApiClient, this);
			}

			public void OnDataChanged(DataEventBuffer dataEvents)
			{
				Log.Debug(Tag, "On Data Changed");
				foreach (var dataEvent in dataEvents)
				{
					var dataItem = dataEvent.DataItem;
					if (dataItem.Uri.Path == "/xfit_watchface")
					{
						var dataMap = DataMapItem.FromDataItem(dataItem).DataMap;
						displayStepCount = dataMap.GetBoolean("stepcount");
						Invalidate(); // force redraw in case we're in ambient mode
					}
				}

			}
		}
	}
}

