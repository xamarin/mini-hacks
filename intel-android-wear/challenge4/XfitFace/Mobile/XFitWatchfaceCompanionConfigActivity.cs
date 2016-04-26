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
using Android.Content;
using Android.Gms.Common.Apis;
using Android.Gms.Wearable;
using Android.OS;
using Android.Util;
using Android.Widget;


namespace XFitWatchface
{
	[Activity(Label = "XFit Watchface")]
	[IntentFilter(new[] { "XFitWatchface.CONFIG" },
		Categories = new[] {
			"com.google.android.wearable.watchface.category.COMPANION_CONFIGURATION",
			"android.intent.category.DEFAULT"
		})]
	public class XFitWatchfaceCompanionConfigActivity : Activity, IDataApiDataListener
	{

		const string Tag = "XFit Config Mobile";

		CheckBox checkbox;
		GoogleApiClient googleApiClient;
		bool stepCountOn;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.XFitWatchfaceCompanionConfigActivity);
			checkbox = FindViewById<CheckBox>(Resource.Id.stepCountCheckbox);
			checkbox.Click += (sender, e) =>
			{
				// TODO save step count visibility
			};

			ConnectGoogleApiClient();
			ConnectWatchfaceConfigUpdates();
		}

		void UpdateUI()
		{
			checkbox.Checked = stepCountOn;
		}

		void ConnectGoogleApiClient()
		{
			googleApiClient = new GoogleApiClient.Builder(this)
				.AddApi(WearableClass.API)
				.AddConnectionCallbacks(
					async connectionHint =>
					{
						if (Log.IsLoggable(Tag, LogPriority.Info))
						{
							Log.Info(Tag, "Connected to the Google API client");
						}

						stepCountOn = await XFitWatchfaceConfigHelper.ReadStepCountStatus(googleApiClient);
						UpdateUI();
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
					stepCountOn = dataMap.GetBoolean("stepcount");
					UpdateUI();
				}
			}

		}

	}
}