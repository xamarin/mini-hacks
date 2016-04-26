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
using Android.Content;
using Android.Gms.Common.Apis;
using Android.Gms.Wearable;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Support.Wearable.Views;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace XFitWatchface
{
	[Activity(Label = "XFit Watchface")]
	[IntentFilter(new[] { "XFitWatchface.CONFIG" },
		Categories = new[] {
			"com.google.android.wearable.watchface.category.WEARABLE_CONFIGURATION",
			"android.intent.category.DEFAULT"
		})]
	public class XFitWatchfaceWearableConfigActivity : Activity
	{

		const string Tag = "XFit Config";

		TextView header;
		WearableListView listView;

		Setting[] settings;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.XFitWatchfaceWearableConfigActivity);
			header = FindViewById<TextView>(Resource.Id.Header);

			listView = FindViewById<WearableListView>(Resource.Id.ColorPicker);
			var content = FindViewById<BoxInsetLayout>(Resource.Id.Content);

			content.ApplyWindowInsets = (v, insets) =>
			{
				if (!insets.IsRound)
				{
					v.SetPaddingRelative(
						Resources.GetDimensionPixelSize(Resource.Dimension.ContentPaddingStart),
						v.PaddingTop,
						v.PaddingEnd,
						v.PaddingBottom
					);
				}
				return v.OnApplyWindowInsets(insets);
			};

			listView.HasFixedSize = true;

			listView.Click += (object sender, WearableListView.ClickEventArgs e) =>
			{
				var viewHolder = e.P0;
				var itemViewHolder = viewHolder as ItemViewHolder;

				var state = itemViewHolder.Setting.Value;
				// TODO: save the step count visibility

				Finish();
			};

			listView.AbsoluteScrollChange += (object sender, WearableListView.AbsoluteScrollChangeEventArgs e) =>
			{
				var scroll = e.P0;
				var newTranslation = Math.Min(-scroll, 0);
				header.TranslationY = newTranslation;
			};
		}

		protected override void OnStart()
		{
			base.OnStart();

			ConnectGoogleApiClient();
		}

		GoogleApiClient googleApiClient;

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

						var stepCountOn = await XFitWatchfaceConfigHelper.ReadStepCountStatus(googleApiClient);
						var stepCountState = stepCountOn ? "ON" : "OFF";
						var stepCountSetting = new Setting($"Step count\n{stepCountState}", "stepcount", stepCountOn);
						settings = new Setting[] { stepCountSetting };
						listView.SetAdapter(new ListAdapter(this, settings));

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

	class Setting
	{
		public string DisplayName { get; set; }
		public string Key { get; set; }
		public bool Value { get; set; }

		public Setting(string displayName, string key, bool value)
		{
			DisplayName = displayName;
			Key = key;
			Value = value;
		}
	}

	class ListAdapter : WearableListView.Adapter
	{
		readonly Setting[] items;

		Context context;
		LayoutInflater inflater;

		public ListAdapter(Context context, Setting[] items)
		{
			this.context = context;
			this.items = items;
			inflater = LayoutInflater.From(context);
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			var itemViewHolder = (ItemViewHolder)holder;
			var textView = itemViewHolder.TextView;
			textView.Text = items[position].DisplayName;
			itemViewHolder.Setting = items[position];
			holder.ItemView.Tag = position;
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			return new ItemViewHolder(inflater.Inflate(Resource.Layout.XFitWatchfaceWearableConfigActivityListItem, null));
		}

		public override int ItemCount
		{
			get
			{
				return items.Length;
			}
		}
	}

	class ItemViewHolder : WearableListView.ViewHolder
	{
		public TextView TextView { get; set; }
		public Setting Setting { get; set; }

		public ItemViewHolder(View itemView) : base(itemView)
		{
			TextView = (TextView)itemView.FindViewById(Resource.Id.Label);
		}
	}
}

